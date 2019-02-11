using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using Dapper.Repositories.Attributes;
using Dapper.Repositories.Attributes.Joins;
using Dapper.Repositories.Extensions;

namespace Dapper.Repositories.SqlGenerator
{
    /// <inheritdoc />
    public partial class SqlGenerator<TEntity>
        where TEntity : class
    {
        private void InitProperties()
        {
            var entityType = typeof(TEntity);
            var entityTypeInfo = entityType.GetTypeInfo();
            var tableAttribute = entityTypeInfo.GetCustomAttribute<TableAttribute>();

            TableName = tableAttribute != null ? tableAttribute.Name : entityTypeInfo.Name;
            TableSchema = tableAttribute != null ? tableAttribute.Schema : string.Empty;

            AllProperties = entityType.FindClassProperties().Where(q => q.CanWrite).ToArray();

            var props = AllProperties.Where(ExpressionHelper.GetPrimitivePropertiesPredicate()).ToArray();

            var joinProperties = AllProperties.Where(p => p.GetCustomAttributes<JoinAttributeBase>().Any()).ToArray();

            SqlJoinProperties = GetJoinPropertyMetadata(joinProperties);

            // Filter the non stored properties
            SqlProperties = props.Where(p => !p.GetCustomAttributes<NotMappedAttribute>().Any()).Select(p => new SqlPropertyMetadata(p)).ToArray();

            // Filter key properties
            KeySqlProperties = props.Where(p => p.GetCustomAttributes<KeyAttribute>().Any()).Select(p => new SqlPropertyMetadata(p)).ToArray();

            // Use identity as key pattern
            var identityProperty = props.FirstOrDefault(p => p.GetCustomAttributes<IdentityAttribute>().Any());
            IdentitySqlProperty = identityProperty != null ? new SqlPropertyMetadata(identityProperty) : null;

            var dateCreatedProperty = props.FirstOrDefault(p => p.GetCustomAttributes<CreatedAtAttribute>().Count() == 1);
            if (dateCreatedProperty != null && (dateCreatedProperty.PropertyType == typeof(DateTime) ||
                                                dateCreatedProperty.PropertyType == typeof(DateTime?)) && 
                !dateCreatedProperty.GetCustomAttributes<NotMappedAttribute>().Any())
            {
                CreatedAtProperty = props.FirstOrDefault(p => p.GetCustomAttributes<CreatedAtAttribute>().Any());
                CreatedAtPropertyMetadata = new SqlPropertyMetadata(CreatedAtProperty);
            }

            var dateChangedProperty = props.FirstOrDefault(p => p.GetCustomAttributes<UpdatedAtAttribute>().Count() == 1);
            if (dateChangedProperty != null && (dateChangedProperty.PropertyType == typeof(DateTime) ||
                                                dateChangedProperty.PropertyType == typeof(DateTime?)) &&
                !dateChangedProperty.GetCustomAttributes<NotMappedAttribute>().Any())
            {
                UpdatedAtProperty = props.FirstOrDefault(p => p.GetCustomAttributes<UpdatedAtAttribute>().Any());
                UpdatedAtPropertyMetadata = new SqlPropertyMetadata(UpdatedAtProperty);
            }

            var rowVersionProperty = props.FirstOrDefault(p => p.GetCustomAttributes<RowVersionAttribute>().Count() == 1);
            if (rowVersionProperty != null && (rowVersionProperty.PropertyType == typeof(byte[]) ||
                                               rowVersionProperty.PropertyType == typeof(byte)) &&
                !rowVersionProperty.GetCustomAttributes<NotMappedAttribute>().Any())
            {
                RowVersionProperty = props.FirstOrDefault(p => p.GetCustomAttributes<RowVersionAttribute>().Any());
                RowVersionPropertyMetadata = new SqlPropertyMetadata(RowVersionProperty);
            }
        }
    }
}