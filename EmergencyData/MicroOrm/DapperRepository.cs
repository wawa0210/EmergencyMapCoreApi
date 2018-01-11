﻿#if COREFX
using IDbConnection = System.Data.Common.DbConnection;
#endif

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Dapper;
using EmergencyData.MicroOrm.Enums;
using EmergencyData.MicroOrm.Extensions;
using EmergencyData.MicroOrm.SqlGenerator;

namespace EmergencyData.MicroOrm
{
    public class DapperRepository<TEntity> : IDapperRepository<TEntity> where TEntity : class
    {
        public DapperRepository(IDbConnection connection)
        {
            Connection = connection;
            SqlGenerator = new SqlGenerator<TEntity>();
        }

        public DapperRepository(IDbConnection connection, ESqlConnector sqlConnector)
        {
            Connection = connection;
            SqlGenerator = new SqlGenerator<TEntity>(sqlConnector);
        }

        public DapperRepository(IDbConnection connection, ISqlGenerator<TEntity> sqlGenerator)
        {
            Connection = connection;
            SqlGenerator = sqlGenerator;
        }

        public IDbConnection Connection { get; private set; }
        public ISqlGenerator<TEntity> SqlGenerator { get; private set; }

        #region Delete

        public virtual bool Delete(TEntity instance, IDbTransaction transaction = null)
        {
            var queryResult = SqlGenerator.GetDelete(instance);
            var deleted = Connection.Execute(queryResult.Sql, queryResult.Param,transaction) > 0;
            return deleted;
        }

        #endregion Delete

        #region Find

        public TEntity Find(SqlQuery sqlQuery, IDbTransaction transaction = null)
        {
           return Connection.Query<TEntity>(sqlQuery.Sql, sqlQuery.Param,transaction: transaction).FirstOrDefault();
        }

        public virtual TEntity Find(Expression<Func<TEntity, bool>> expression,
            List<Expression<Func<TEntity, object>>> selectColumns)
        {
            var queryResult = SqlGenerator.GetSelectFirst(expression, selectColumns);
            return FindAll(queryResult).FirstOrDefault();
        }

        public virtual TEntity Find(Expression<Func<TEntity, bool>> expression)
        {
            var queryResult = SqlGenerator.GetSelectFirst(expression);
            return FindAll(queryResult).FirstOrDefault();
        }

        public TEntity Find(Expression<Func<TEntity, bool>> predicate,
            List<Expression<Func<TEntity, object>>> selectColumns, params Expression<Func<TEntity, object>>[] includes)
        {
            var queryResult = SqlGenerator.GetSelectFirst(predicate, selectColumns);
            return FindAll(queryResult).FirstOrDefault();
        }

        public TEntity Find(Expression<Func<TEntity, bool>> predicate,
           List<Expression<Func<TEntity, object>>> selectColumns,
            Expression<Func<TEntity, object>> orderColumn,
            OrderDirection direction,
            params Expression<Func<TEntity, object>>[] includes)
        {
            var queryResult = SqlGenerator.GetSelectFirst(predicate, selectColumns, orderColumn, direction,includes);
            return FindAll(queryResult).FirstOrDefault();
        }

        public virtual TEntity Find<TChild1>(Expression<Func<TEntity, bool>> expression,
            Expression<Func<TEntity, object>> tChild1)
        {
            var queryResult = SqlGenerator.GetSelectFirst(expression);
            return FindAll<TChild1>(queryResult, tChild1).FirstOrDefault();
        }

        public virtual IEnumerable<TEntity> FindAll()
        {
            var queryResult = SqlGenerator.GetSelectAll(null);
            return FindAll(queryResult);
        }

        public virtual IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> expression, IDbTransaction transaction = null)
        {
            var queryResult = SqlGenerator.GetSelectAll(expression);
            return Connection.Query<TEntity>(queryResult.Sql, queryResult.Param, transaction);
        }

        public virtual IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> expression,
            List<Expression<Func<TEntity, object>>> selectColumns)
        {
            var queryResult = SqlGenerator.GetSelectAll(expression, selectColumns);
            return Connection.Query<TEntity>(queryResult.Sql, queryResult.Param);
        }

        public virtual IEnumerable<TEntity> FindAll(SqlQuery sqlQuery, IDbTransaction transaction = null)
        {
            return Connection.Query<TEntity>(sqlQuery.Sql, sqlQuery.Param, transaction);
        }

        public virtual IEnumerable<TEntity> FindAll<TChild1>(Expression<Func<TEntity, object>> tChild1)
        {
            var queryResult = SqlGenerator.GetSelectAll(null, tChild1);
            return FindAll<TChild1>(queryResult, tChild1);
        }

        public virtual IEnumerable<TEntity> FindAll<TChild1>(Expression<Func<TEntity, bool>> expression,
            Expression<Func<TEntity, object>> tChild1)
        {
            var queryResult = SqlGenerator.GetSelectAll(expression, tChild1);
            return FindAll<TChild1>(queryResult, tChild1);
        }

        public virtual IEnumerable<TEntity> FindAll<TChild1>(SqlQuery sqlQuery,
            Expression<Func<TEntity, object>> tChild1)
        {
            var type = typeof (TEntity);
            IEnumerable<TEntity> result;
            var propertyName = ExpressionHelper.GetPropertyName(tChild1);
            var tj1Property = type.GetProperty(propertyName);
            if (tj1Property.PropertyType.IsGenericType())
            {
                var lookup = new Dictionary<object, TEntity>();

                var keyPropertyMeta = SqlGenerator.KeyProperties.FirstOrDefault();
                if (keyPropertyMeta == null)
                    throw new Exception("key not found");

                var keyProperty = keyPropertyMeta.PropertyInfo;

                Connection.Query<TEntity, TChild1, TEntity>(sqlQuery.Sql, (entity, j1) =>
                {
                    var key = keyProperty.GetValue(entity);

                    TEntity en;
                    if (!lookup.TryGetValue(key, out en))
                    {
                        lookup.Add(key, en = entity);
                    }

                    var list = (List<TChild1>) tj1Property.GetValue(en) ?? new List<TChild1>();
                    if (j1 != null)
                        list.Add(j1);

                    tj1Property.SetValue(en, list);

                    return en;
                }, sqlQuery.Param);

                result = lookup.Values;
            }
            else
            {
                result = Connection.Query<TEntity, TChild1, TEntity>(sqlQuery.Sql, (entity, j1) =>
                {
                    type.GetProperty(propertyName).SetValue(entity, j1);
                    return entity;
                }, sqlQuery.Param);
            }

            return result;
        }

        public virtual async Task<IEnumerable<TEntity>> FindAllAsync()
        {
            var queryResult = SqlGenerator.GetSelectAll(null);
            return await FindAllAsync(queryResult);
        }

        public virtual async Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> expression)
        {
            var queryResult = SqlGenerator.GetSelectAll(expression);
            return await FindAllAsync(queryResult);
        }

        public virtual async Task<IEnumerable<TEntity>> FindAllAsync(SqlQuery sqlQuery)
        {
            return await Connection.QueryAsync<TEntity>(sqlQuery.Sql, sqlQuery.Param);
        }

        public virtual async Task<IEnumerable<TEntity>> FindAllAsync<TChild1>(Expression<Func<TEntity, object>> tChild1)
        {
            var queryResult = SqlGenerator.GetSelectAll(null, tChild1);
            return await FindAllAsync<TChild1>(queryResult, tChild1);
        }

        public virtual async Task<IEnumerable<TEntity>> FindAllAsync<TChild1>(
            Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, object>> tChild1)
        {
            var queryResult = SqlGenerator.GetSelectAll(expression, tChild1);
            return await FindAllAsync<TChild1>(queryResult, tChild1);
        }

        public virtual async Task<IEnumerable<TEntity>> FindAllAsync<TChild1>(SqlQuery sqlQuery,
            Expression<Func<TEntity, object>> tChild1)
        {
            var type = typeof (TEntity);
            var propertyName = ExpressionHelper.GetPropertyName(tChild1);

            IEnumerable<TEntity> result = null;
            var tj1Property = type.GetProperty(propertyName);
            if (tj1Property.PropertyType.IsGenericType())
            {
                var lookup = new Dictionary<object, TEntity>();

                var keyPropertyMeta = SqlGenerator.KeyProperties.FirstOrDefault();
                if (keyPropertyMeta == null)
                    throw new Exception("key not found");

                var keyProperty = keyPropertyMeta.PropertyInfo;

                await Connection.QueryAsync<TEntity, TChild1, TEntity>(sqlQuery.Sql, (entity, j1) =>
                {
                    var key = keyProperty.GetValue(entity);

                    TEntity en;
                    if (!lookup.TryGetValue(key, out en))
                    {
                        lookup.Add(key, en = entity);
                    }

                    var list = (List<TChild1>) tj1Property.GetValue(en) ?? new List<TChild1>();
                    if (j1 != null)
                        list.Add(j1);

                    tj1Property.SetValue(en, list);

                    return en;
                }, sqlQuery.Param);

                result = lookup.Values;
            }
            else
            {
                result = await Connection.QueryAsync<TEntity, TChild1, TEntity>(sqlQuery.Sql, (entity, j1) =>
                {
                    type.GetProperty(propertyName).SetValue(entity, j1);
                    return entity;
                }, sqlQuery.Param);
            }

            return result;
        }

        public virtual async Task<TEntity> FindAsync<TChild1>(Expression<Func<TEntity, object>> tChild1)
        {
            var queryResult = SqlGenerator.GetSelectFirst(null, tChild1);
            return (await FindAllAsync<TChild1>(queryResult, tChild1)).FirstOrDefault();
        }

        public virtual async Task<TEntity> FindAsync<TChild1>(Expression<Func<TEntity, bool>> expression,
            Expression<Func<TEntity, object>> tChild1)
        {
            var queryResult = SqlGenerator.GetSelectFirst(expression, tChild1);
            return (await FindAllAsync<TChild1>(queryResult, tChild1)).FirstOrDefault();
        }

        public virtual async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> expression)
        {
            var queryResult = SqlGenerator.GetSelectFirst(expression);
            return (await FindAllAsync(queryResult)).FirstOrDefault();
        }

        public virtual async Task<TEntity> FindAsync()
        {
            var queryResult = SqlGenerator.GetSelectFirst(null);
            return (await FindAllAsync(queryResult)).FirstOrDefault();
        }

        #endregion Find

        #region Insert

        public virtual bool Insert(TEntity instance, IDbTransaction transaction = null)
        {
            bool added;

            var queryResult = SqlGenerator.GetInsert(instance);

            if (SqlGenerator.IsIdentity)
            {
                var newId = Connection.Query<int>(queryResult.Sql, queryResult.Param, transaction).Single();
                added = newId > 0;

                if (added)
                {
                    var newParsedId = Convert.ChangeType(newId, SqlGenerator.IdentityProperty.PropertyInfo.PropertyType);
                    SqlGenerator.IdentityProperty.PropertyInfo.SetValue(instance, newParsedId);
                }
            }
            else
            {
                added = Connection.Execute(queryResult.Sql, instance) > 0;
            }

            return true;
        }

        public bool BulkInsert(IEnumerable<TEntity> instancEntities)
        {
            throw new NotImplementedException();
        }


        public virtual async Task<bool> InsertAsync(TEntity instance, IDbTransaction transaction = null)
        {
            bool added;

            var queryResult = SqlGenerator.GetInsert(instance);

            if (SqlGenerator.IsIdentity)
            {
                var newId = (await Connection.QueryAsync<long>(queryResult.Sql, queryResult.Param,transaction)).FirstOrDefault();
                added = newId > 0;

                if (added)
                {
                    var newParsedId = Convert.ChangeType(newId, SqlGenerator.IdentityProperty.PropertyInfo.PropertyType);
                    SqlGenerator.IdentityProperty.PropertyInfo.SetValue(instance, newParsedId);
                }
            }
            else
            {
                added = Connection.Execute(queryResult.Sql, instance) > 0;
            }

            return added;
        }

        public int Insert(SqlQuery sqlQuery, IDbTransaction transaction = null)
        {
            return Connection.Execute(sqlQuery.Sql, sqlQuery.Param,transaction);
        }

        #endregion Insert

        #region Update

        public virtual bool Update(TEntity instance, IDbTransaction transaction = null)
        {
            var query = SqlGenerator.GetUpdate(instance);
            var updated = Connection.Execute(query.Sql, instance,transaction) > 0;
            return updated;
        }

        public bool Update(SqlQuery sqlQuery, IDbTransaction transaction = null)
        {
            var updated = Connection.Execute(sqlQuery.Sql, sqlQuery.Param, transaction) > 0;
            return updated;
        }

        public virtual bool Update<T>(TEntity instance, Expression<Func<T, dynamic>> fields, IDbTransaction transaction = null)
        {
            var query = SqlGenerator.GetUpdate(instance, fields);
            var updated = Connection.Execute(query.Sql, instance,transaction) > 0;


            return updated;
        }



        #endregion Update


        #region Beetwen

        private const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

        public IEnumerable<TEntity> FindAllBetween(object from, object to, Expression<Func<TEntity, object>> btwField)
        {
            return FindAllBetween(from, to, btwField, null);
        }

        public IEnumerable<TEntity> FindAllBetween(object from, object to, Expression<Func<TEntity, object>> btwField,
            Expression<Func<TEntity, bool>> expression)
        {
            var queryResult = SqlGenerator.GetSelectBetween(from, to, btwField, expression);
            var data = Connection.Query<TEntity>(queryResult.Sql, queryResult.Param);
            return data;
        }

        public IEnumerable<TEntity> FindAllBetween(DateTime from, DateTime to,
            Expression<Func<TEntity, object>> btwField)
        {
            return FindAllBetween(from, to, btwField, null);
        }

        public IEnumerable<TEntity> FindAllBetween(DateTime from, DateTime to,
            Expression<Func<TEntity, object>> btwField, Expression<Func<TEntity, bool>> expression)
        {
            var fromString = from.ToString(DateTimeFormat);
            var toString = to.ToString(DateTimeFormat);
            return FindAllBetween(fromString, toString, btwField, expression);
        }

        public async Task<IEnumerable<TEntity>> FindAllBetweenAsync(object from, object to,
            Expression<Func<TEntity, object>> btwField)
        {
            return await FindAllBetweenAsync(from, to, btwField, null);
        }

        public async Task<IEnumerable<TEntity>> FindAllBetweenAsync(object from, object to,
            Expression<Func<TEntity, object>> btwField, Expression<Func<TEntity, bool>> expression)
        {
            var queryResult = SqlGenerator.GetSelectBetween(from, to, btwField, expression);
            var data = await Connection.QueryAsync<TEntity>(queryResult.Sql, queryResult.Param);
            return data;
        }

        public async Task<IEnumerable<TEntity>> FindAllBetweenAsync(DateTime from, DateTime to,
            Expression<Func<TEntity, object>> btwField)
        {
            return await FindAllBetweenAsync(from, to, btwField, null);
        }

        public async Task<IEnumerable<TEntity>> FindAllBetweenAsync(DateTime from, DateTime to,
            Expression<Func<TEntity, object>> btwField, Expression<Func<TEntity, bool>> expression)
        {
            var fromString = from.ToString(DateTimeFormat);
            var toString = to.ToString(DateTimeFormat);
            return await FindAllBetweenAsync(fromString, toString, btwField, expression);
        }

        #endregion Beetwen
    }
}