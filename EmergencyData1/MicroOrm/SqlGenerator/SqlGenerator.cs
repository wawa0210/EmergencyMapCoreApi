using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using EmergencyData.MicroOrm.Attributes;
using EmergencyData.MicroOrm.Attributes.Joins;
using EmergencyData.MicroOrm.Attributes.LogicalDelete;
using EmergencyData.MicroOrm.Enums;
using EmergencyData.MicroOrm.Extensions;

namespace EmergencyData.MicroOrm.SqlGenerator
{
    public class SqlGenerator<TEntity> : ISqlGenerator<TEntity>
        where TEntity : class
    {
        public SqlGenerator(ESqlConnector sqlConnector)
        {
            SqlConnector = sqlConnector;
            var entityType = typeof (TEntity);
            var entityTypeInfo = entityType.GetTypeInfo();
            var aliasAttribute = entityTypeInfo.GetCustomAttribute<TableAttribute>();

            TableName = aliasAttribute != null ? aliasAttribute.Name : entityTypeInfo.Name;
            AllProperties = entityType.GetProperties();
            //Load all the "primitive" entity properties
            var props = AllProperties.Where(ExpressionHelper.GetPrimitivePropertiesPredicate()).ToArray();

            //Filter the non stored properties
            BaseProperties =
                props.Where(p => !p.GetCustomAttributes<NotMappedAttribute>().Any())
                    .Select(p => new PropertyMetadata(p));

            //Filter key properties
            KeyProperties =
                props.Where(p => p.GetCustomAttributes<KeyAttribute>().Any())
                    .Select(p => new PropertyMetadata(p));

            //Use identity as key pattern
            var identityProperty = props.FirstOrDefault(p => p.GetCustomAttributes<IdentityAttribute>().Any());
            IdentityProperty = identityProperty != null
                ? new PropertyMetadata(identityProperty)
                : null;

            //Status property (if exists, and if it does, it must be an enumeration)
            var statusProperty = props.FirstOrDefault(p => p.GetCustomAttributes<StatusAttribute>().Any());

            if (statusProperty == null) return;
            StatusProperty = new PropertyMetadata(statusProperty);

            if (statusProperty.PropertyType.IsBool())
            {
                var deleteProperty = props.FirstOrDefault(p => p.GetCustomAttributes<DeletedAttribute>().Any());
                if (deleteProperty == null) return;

                LogicalDelete = true;
                LogicalDeleteValue = 1; // true
            }
            else if (statusProperty.PropertyType.IsEnum())
            {
                var deleteOption =
                    statusProperty.PropertyType.GetFields()
                        .FirstOrDefault(f => f.GetCustomAttribute<DeletedAttribute>() != null);

                if (deleteOption == null) return;

                var enumValue = Enum.Parse(statusProperty.PropertyType, deleteOption.Name);

                if (enumValue != null)
                    LogicalDeleteValue = Convert.ChangeType(enumValue,
                        Enum.GetUnderlyingType(statusProperty.PropertyType));

                LogicalDelete = true;
            }
        }

        public SqlGenerator()
            : this(ESqlConnector.Mssql)
        {
        }

        public PropertyInfo[] AllProperties { get; private set; }
        public ESqlConnector SqlConnector { get; set; }

        public bool IsIdentity
        {
            get { return IdentityProperty != null; }
        }

        public bool LogicalDelete { get; private set; }
        public string TableName { get; private set; }
        public PropertyMetadata IdentityProperty { get; private set; }
        public IEnumerable<PropertyMetadata> KeyProperties { get; private set; }
        public IEnumerable<PropertyMetadata> BaseProperties { get; private set; }
        public PropertyMetadata StatusProperty { get; private set; }
        public object LogicalDeleteValue { get; private set; }

        private string GetPropertyValue(TEntity entity, PropertyInfo property)
        {
            if (property.GetValue(entity) == null)
                return null;
            if (property.PropertyType == typeof(String)
                || property.PropertyType == typeof(DateTime)
                || property.PropertyType == typeof(Guid)
                || property.PropertyType == typeof(DateTime?)
                )
            {
                return string.Format("'{0}'", property.GetValue(entity));
            }
            var result = property.GetValue(entity) == null
                ? null
                : property.GetValue(entity).ToString();
            return result;
        }

        private string GetPropertyValue(QueryParameter parameter)
        {
            if (parameter.PropertyValue is string ||
                parameter.PropertyValue is DateTime ||
                parameter.PropertyValue is Guid)
            {
                return string.Format("'{0}'", parameter.PropertyValue);
            }

            return parameter.PropertyValue == null ? null : parameter.PropertyValue.ToString();
        }


        public virtual SqlQuery GetInsert(TEntity entity)
        {
            var properties = (IsIdentity
                ? BaseProperties.Where(
                    p => !p.Name.Equals(IdentityProperty.Name, StringComparison.OrdinalIgnoreCase))
                : BaseProperties).ToList();

            var columNames = string.Join(", ", properties.Select(p => p.ColumnName));
            var values = string.Join(", ", properties.Select(p => GetPropertyValue(entity, p.PropertyInfo) ?? "NULL"));
            var sqlBuilder = new StringBuilder();
            sqlBuilder.AppendFormat("INSERT INTO {0} {1} {2} ",
                TableName,
                string.IsNullOrEmpty(columNames) ? "" : "(" + columNames + ")",
                string.IsNullOrEmpty(values) ? "" : " VALUES (" + values + ")");

            if (IsIdentity)
            {
                switch (SqlConnector)
                {
                    case ESqlConnector.Mssql:
                        sqlBuilder.Append("SELECT CAST(SCOPE_IDENTITY() AS INT) AS " + IdentityProperty.ColumnName);
                        break;

                    case ESqlConnector.MySql:
                        sqlBuilder.Append("; SELECT CONVERT(LAST_INSERT_ID(), SIGNED INTEGER) AS " +
                                          IdentityProperty.ColumnName);
                        break;

                    case ESqlConnector.PostgreSql:
                        sqlBuilder.Append("RETURNING " + IdentityProperty.ColumnName);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return new SqlQuery(sqlBuilder.ToString(), entity);
        }

        
        public virtual SqlQuery GetUpdate(TEntity entity)
        {
            var properties =
                BaseProperties.Where(
                    p => !KeyProperties.Any(k => k.Name.Equals(p.Name, StringComparison.OrdinalIgnoreCase)));

            var sqlBuilder = new StringBuilder();
            sqlBuilder.AppendFormat("UPDATE {0} SET {1} WHERE {2}", TableName,
                string.Join(", ",
                    properties.Select(
                        p => string.Format("{0}={1}", p.ColumnName, GetPropertyValue(entity, p.PropertyInfo) ?? "NULL"))),
                string.Join(" AND ",
                    KeyProperties.Select(
                        p => string.Format("{0}={1}", p.ColumnName, GetPropertyValue(entity, p.PropertyInfo) ?? "NULL"))));

            return new SqlQuery(sqlBuilder.ToString().TrimEnd(), entity);
        }

        public virtual SqlQuery GetUpdate<T>(TEntity entity, Expression<Func<T, dynamic>> fields)
        {
            var targetProperties = GetPropertyInfos(fields);
            var targetPropertyNames = new List<string>();
            if (targetProperties != null && targetProperties.Any())
            {
                targetPropertyNames = targetProperties.Select(p => p.Name).ToList();
            }

            var properties =
                BaseProperties.Where(
                    p => !KeyProperties.Any(k => k.Name.Equals(p.Name, StringComparison.OrdinalIgnoreCase)));

            if (targetPropertyNames.Any())
            {
                properties = properties.Where(p => targetPropertyNames.Contains(p.Name));
            }

            var sqlBuilder = new StringBuilder();
            sqlBuilder.AppendFormat("UPDATE {0} SET {1} WHERE {2}", TableName,
                string.Join(", ",
                    properties.Select(
                        p => string.Format("{0}={1}", p.ColumnName, GetPropertyValue(entity, p.PropertyInfo) ?? "NULL"))),
                string.Join(" AND ",
                    KeyProperties.Select(
                        p => string.Format("{0}={1}", p.ColumnName, GetPropertyValue(entity, p.PropertyInfo) ?? "NULL"))));

            return new SqlQuery(sqlBuilder.ToString().TrimEnd(), entity);
        }

    

        /// <summary>
        ///     Fill query properties
        /// </summary>
        /// <param name="body">The body.</param>
        /// <param name="linkingType">Type of the linking.</param>
        /// <param name="queryProperties">The query properties.</param>
        private static void FillQueryProperties(BinaryExpression body, ExpressionType linkingType,
            ref List<QueryParameter> queryProperties)
        {
            if (body.NodeType != ExpressionType.AndAlso && body.NodeType != ExpressionType.OrElse)
            {
                //string propertyName = ExpressionHelper.GetPropertyName(body);
                var propertyName = ExpressionHelper.GetColumnName(body);
                var propertyValue = ExpressionHelper.GetValue(body.Right);
                var opr = ExpressionHelper.GetSqlOperator(body.NodeType);
                var link = ExpressionHelper.GetSqlOperator(linkingType);

                queryProperties.Add(new QueryParameter(link, propertyName, propertyValue, opr));
            }
            else
            {
                FillQueryProperties(ExpressionHelper.GetBinaryExpression(body.Left), body.NodeType, ref queryProperties);
                FillQueryProperties(ExpressionHelper.GetBinaryExpression(body.Right), body.NodeType, ref queryProperties);
            }
        }

        #region Get Select

        public virtual SqlQuery GetSelectFirst(Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includes)
        {
            return GetSelect(predicate, true, null,null, OrderDirection.Asc , includes);
        }

        public virtual SqlQuery GetSelectFirst(Expression<Func<TEntity, bool>> predicate,
            List<Expression<Func<TEntity, object>>> selectColumns, params Expression<Func<TEntity, object>>[] includes)
        {
            return GetSelect(predicate, true, selectColumns,null, OrderDirection.Asc ,includes);
        }

        public virtual SqlQuery GetSelectFirst(Expression<Func<TEntity, bool>> predicate,
           List<Expression<Func<TEntity, object>>> selectColumns, Expression<Func<TEntity,object>> orderColumn=null, OrderDirection direction= OrderDirection.Asc, params Expression<Func<TEntity, object>>[] includes)
        {
            return GetSelect(predicate, true, selectColumns, orderColumn, direction, includes);
        }

        public virtual SqlQuery GetSelectAll(Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includes)
        {
            return GetSelect(predicate, false, null,null, OrderDirection.Asc, includes);
        }

        public virtual SqlQuery GetSelectAll(Expression<Func<TEntity, bool>> predicate,
            List<Expression<Func<TEntity, object>>> selectColumns, params Expression<Func<TEntity, object>>[] includes)
        {
            return GetSelect(predicate, false, selectColumns,null, OrderDirection.Asc, includes);
        }

        private static MemberExpression GetMemberInfo(Expression method)
        {
            var lambda = method as LambdaExpression;
            if (lambda == null)
                throw new ArgumentNullException("method");

            MemberExpression memberExpr = null;

            if (lambda.Body.NodeType == ExpressionType.Convert)
            {
                memberExpr =
                    ((UnaryExpression) lambda.Body).Operand as MemberExpression;
            }
            else if (lambda.Body.NodeType == ExpressionType.MemberAccess)
            {
                memberExpr = lambda.Body as MemberExpression;
            }

            if (memberExpr == null)
                throw new ArgumentException("method");

            return memberExpr;
        }


        private static PropertyInfo[] GetPropertyInfos<T>(Expression<Func<T, dynamic>> select)
        {
                var body = select.Body;
            if (body.NodeType == ExpressionType.Parameter)
            {
                return (body as ParameterExpression).Type.GetProperties();
            }
            if (body.NodeType == ExpressionType.New)
            {
                return (body as NewExpression).Members.Select(m => m as PropertyInfo).ToArray();
            }
            if (body.NodeType == ExpressionType.Convert)
            {
                var memberAccess = (body as UnaryExpression).Operand as MemberExpression;
                return new PropertyInfo[] { memberAccess.Member as PropertyInfo };
            }
            return null;
        }

        private StringBuilder InitBuilderSelect(bool firstOnly, List<Expression<Func<TEntity, object>>> selectColumns)
        {
            var builder = new StringBuilder();
            var select = "SELECT ";

            if (firstOnly && SqlConnector == ESqlConnector.Mssql)
                select += "TOP 1 ";

            // convert the query parms into a SQL string and dynamic property object
            //builder.Append("{select} {GetFieldsSelect(TableName, BaseProperties)}");
            if (selectColumns != null)
            {
                var columns = selectColumns.Select(c => GetMemberInfo(c).Member.Name).ToList();
                var properties = BaseProperties.Where(p => columns.Contains(p.Name));
                builder.AppendFormat("{0} {1}", select, GetFieldsSelect(TableName, properties));
                return builder;
            }

            builder.AppendFormat("{0} {1}", select, GetFieldsSelect(TableName, BaseProperties));

            return builder;
        }

        private StringBuilder AppendJoinToSelect(StringBuilder originalBuilder,
            params Expression<Func<TEntity, object>>[] includes)
        {
            var joinsBuilder = new StringBuilder();

            foreach (var include in includes)
            {
                var propertyName = ExpressionHelper.GetPropertyName(include);
                var joinProperty = AllProperties.First(x => x.Name == propertyName);
                var attrJoin = joinProperty.GetCustomAttribute<JoinAttributeBase>();
                if (attrJoin != null)
                {
                    var joinString = "";
                    if (attrJoin is LeftJoinAttribute)
                    {
                        joinString = "LEFT JOIN ";
                    }
                    else if (attrJoin is InnerJoinAttribute)
                    {
                        joinString = "INNER JOIN ";
                    }
                    else if (attrJoin is RightJoinAttribute)
                    {
                        joinString = "RIGHT JOIN ";
                    }

                    var joinType = joinProperty.PropertyType.IsGenericType()
                        ? joinProperty.PropertyType.GenericTypeArguments[0]
                        : joinProperty.PropertyType;

                    var properties = joinType.GetProperties().Where(ExpressionHelper.GetPrimitivePropertiesPredicate());
                    var props =
                        properties.Where(p => !p.GetCustomAttributes<NotMappedAttribute>().Any())
                            .Select(p => new PropertyMetadata(p));
                    originalBuilder.Append(", " + GetFieldsSelect(attrJoin.TableName, props));


                    //joinsBuilder.Append("{joinString} {attrJoin.TableName} ON {TableName}.{attrJoin.Key} = {attrJoin.TableName}.{attrJoin.ExternalKey} ");
                    if (SqlConnector == ESqlConnector.Mssql)
                    {
                        joinsBuilder.AppendFormat("{0} {1} ON {2}.{3} = {4}.{5} ", joinString, attrJoin.TableName+ " (NOLOCK) ", TableName,
                        attrJoin.Key, attrJoin.TableName, attrJoin.ExternalKey);
                    }
                    else
                    {
                        joinsBuilder.AppendFormat("{0} {1} ON {2}.{3} = {4}.{5} ", joinString, attrJoin.TableName, TableName,
                        attrJoin.Key, attrJoin.TableName, attrJoin.ExternalKey);
                    }
                }
            }
            return joinsBuilder;
        }

        private static string GetFieldsSelect(string tableName,
            IEnumerable<PropertyMetadata> properties)
        {
            //Projection function
            Func<PropertyMetadata, string> projectionFunction =
                p => !string.IsNullOrEmpty(p.Alias)
                    ? string.Format("{0} AS {1}", p.ColumnName, p.Name)
                    : string.Format("{0}", p.ColumnName);

            return string.Join(", ", properties.Select(projectionFunction));
        }


        private SqlQuery GetSelect(Expression<Func<TEntity, bool>> predicate, bool firstOnly,
            List<Expression<Func<TEntity, object>>> selectColumns, Expression<Func<TEntity, object>> orderColumn=null, OrderDirection direction= OrderDirection.Asc,  params Expression<Func<TEntity, object>>[] includes)
        {
            var builder = InitBuilderSelect(firstOnly, selectColumns);

            if (includes.Any())
            {
                var joinsBuilder = AppendJoinToSelect(builder, includes);
                builder.Append(" FROM " + TableName);
                if (SqlConnector== ESqlConnector.Mssql)
                {
                    builder.Append(" (NOLOCK) ");
                }
                builder.Append(joinsBuilder);
            }
            else
            {
                builder.Append(" FROM " + TableName);
                if (SqlConnector == ESqlConnector.Mssql)
                {
                    builder.Append(" (NOLOCK) ");
                }
            }

            IDictionary<string, object> expando = new ExpandoObject();

            if (predicate != null)
            {
                // WHERE
                var queryProperties = new List<QueryParameter>();
                FillQueryProperties(ExpressionHelper.GetBinaryExpression(predicate.Body), ExpressionType.Default,
                    ref queryProperties);

                builder.Append(" WHERE ");


                for (var i = 0; i < queryProperties.Count; i++)
                {
                    var item = queryProperties[i];
                    if (!string.IsNullOrEmpty(item.LinkingOperator) && i > 0)
                    {
                        builder.Append(string.Format("{0} {1}.{2} {3} {4} ", item.LinkingOperator, TableName,
                            item.PropertyName, item.QueryOperator, GetPropertyValue(item)));
                    }
                    else
                    {
                        builder.Append(string.Format("{0}.{1} {2} {3} ", TableName, item.PropertyName,
                            item.QueryOperator, GetPropertyValue(item)));
                    }

                    expando[item.PropertyName] = item.PropertyValue;
                }
            }

            if (orderColumn != null)
            {
                var orderProperties = GetPropertyInfos<TEntity>(orderColumn);
                if(orderProperties==null || !orderProperties.Any())
                {
                    throw new Exception("获取排序字段失败");
                }
                var orderProperty = orderProperties.First();
                var orderColumnAttribute = orderProperty.GetCustomAttribute<ColumnAttribute>();
                var orderColumnName = orderColumnAttribute == null ? orderProperty.Name: orderColumnAttribute.Name;

                builder.AppendFormat(" ORDER BY {0} {1}", orderColumnName, direction == OrderDirection.Asc ? "ASC" : "DESC");
            }

            if (firstOnly &&
                (SqlConnector == ESqlConnector.MySql ||
                 SqlConnector == ESqlConnector.PostgreSql))
                builder.Append("LIMIT 1");


            return new SqlQuery(builder.ToString().TrimEnd(), expando);
        }

        public virtual SqlQuery GetSelectBetween(object from, object to, Expression<Func<TEntity, object>> btwField,
            Expression<Func<TEntity, bool>> expression)
        {
            var filedName = ExpressionHelper.GetPropertyName(btwField);
            var queryResult = GetSelectAll(expression);
            var op = expression == null ? "WHERE" : "AND";

            queryResult.AppendToSql(string.Format(" {0} {1} BETWEEN '{2}' AND '{3}'", op, filedName, from, to));

            return queryResult;
        }

        public virtual SqlQuery GetDelete(TEntity entity)
        {
            var sqlBuilder = new StringBuilder();

            if (!LogicalDelete)
            {
                sqlBuilder.AppendFormat("DELETE FROM {0} WHERE {1}", TableName,
                    string.Join(" AND ",
                        KeyProperties.Select(
                            p =>
                                string.Format("{0} = {1}", p.ColumnName,
                                    GetPropertyValue(entity, p.PropertyInfo) ?? "NULL"))));
            }
            else
            {
                sqlBuilder.AppendFormat("UPDATE {0} SET {1} WHERE {2}", TableName,
                    string.Format("{0}={1}", StatusProperty.ColumnName, LogicalDeleteValue),
                    string.Join(" AND ",
                        KeyProperties.Select(
                            p =>
                                string.Format("{0}={1}", p.ColumnName,
                                    GetPropertyValue(entity, p.PropertyInfo) ?? "NULL"))));
            }

            return new SqlQuery(sqlBuilder.ToString(), entity);
        }

        #endregion Get Select
    }
}