using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using EmergencyData.MicroOrm.Attributes;
using EmergencyData.MicroOrm.Extensions;
using EmergencyData.MicroOrm.SqlGenerator;

namespace EmergencyData
{
    public class BulkInsertRepository<T> where T : class
    {
        public BulkInsertRepository(DapperDbContext context)
        {
            Connection = context.Connection;
            AllProperties = typeof(T).GetProperties();
            var props =
                AllProperties.Where(
                    p =>
                        p.PropertyType.IsValueType() ||
                        p.PropertyType.Name.Equals("String", StringComparison.OrdinalIgnoreCase)).ToArray();
            var identityProperty = props.FirstOrDefault(p => p.GetCustomAttributes<IdentityAttribute>().Any());
            IdentityProperty = identityProperty != null
                ? new PropertyMetadata(identityProperty)
                : null;
            BaseProperties =
                props.Where(p => !p.GetCustomAttributes<NotMappedAttribute>().Any())
                    .Select(p => new PropertyMetadata(p));
        }
        public IDbConnection Connection { get; private set; }
        public PropertyMetadata IdentityProperty { get; private set; }
        public IEnumerable<PropertyMetadata> BaseProperties { get; private set; }
        public PropertyInfo[] AllProperties { get; private set; }
        public bool IsIdentity
        {
            get { return IdentityProperty != null; }
        }
        public void BulkInsertAll(IEnumerable<T> entities, IDbTransaction trans = null)
        {
            using (var bulkCopy = new SqlBulkCopy((SqlConnection)Connection, SqlBulkCopyOptions.Default, (SqlTransaction)trans))
            {
                var t = typeof(T);
                var tableAttribute = (TableAttribute)t.GetCustomAttributes(
                    typeof(TableAttribute), false).Single();
                bulkCopy.DestinationTableName = tableAttribute.Name;
                var properties = BaseProperties.ToArray();
                var table = new DataTable();
                foreach (var property in properties)
                {
                    var propertyType = property.PropertyInfo.PropertyType;
                    if (propertyType.IsGenericType &&
                        propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        propertyType = Nullable.GetUnderlyingType(propertyType);
                    }

                    table.Columns.Add(new DataColumn(property.ColumnName, propertyType));
                }
                foreach (var entity in entities)
                {
                    table.Rows.Add(
                        properties.Select(
                        property => property.PropertyInfo.GetValue(entity, null) ?? DBNull.Value
                        ).ToArray());
                }
                bulkCopy.WriteToServer(table);

            }
        }

        private bool EventTypeFilter(System.Reflection.PropertyInfo p)
        {
            var attribute = Attribute.GetCustomAttribute(p,
                typeof(AssociationAttribute)) as AssociationAttribute;

            if (attribute == null) return true;
            if (attribute.IsForeignKey == false) return true;

            return false;
        }

    }
}
