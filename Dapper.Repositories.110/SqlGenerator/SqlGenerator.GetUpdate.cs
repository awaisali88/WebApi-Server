using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Common;
using Dapper.Repositories.Extensions;

namespace Dapper.Repositories.SqlGenerator
{
    /// <inheritdoc />
    public partial class SqlGenerator<TEntity>
        where TEntity : class
    {
        /// <inheritdoc />
        public virtual SqlQuery GetUpdate(TEntity entity, Expression<Func<TEntity, object>> propertiesToUpdate)
        {
            var properties = SqlProperties.Where(p =>
                !KeySqlProperties.Any(k => k.PropertyName.Equals(p.PropertyName, StringComparison.OrdinalIgnoreCase)) &&
                !p.IgnoreUpdate && !p.RowVersionProp).ToArray();
            if (!properties.Any())
                throw new ArgumentException("Can't update without [Key]");

            if (propertiesToUpdate != null)
            {
                string[] updateProperties = ExpressionHelper.GetMemberName(propertiesToUpdate);
                if (HasMandatoryProp)
                    properties = properties.Where(x =>
                            updateProperties.Contains(x.PropertyName) ||
                            MandatoryUpdateProperty.Select(y => y.Name).Contains(x.PropertyName))
                        .ToArray();
                else
                    properties = properties.Where(x =>
                            updateProperties.Contains(x.PropertyName))
                        .ToArray();
            }

            if (HasUpdatedAt)
                UpdatedAtProperty.SetValue(entity, DateTime.UtcNow);

            var query = new SqlQuery(entity);

            query.SqlBuilder
                .Append("UPDATE ")
                .Append(TableName)
                .Append(" SET ");

            query.SqlBuilder.Append(string.Join(", ", properties
                .Select(p => string.Format("{0} = @{1}", p.ColumnName, p.PropertyName))));

            query.SqlBuilder.Append(" WHERE ");

            query.SqlBuilder.Append(string.Join(" AND ", KeySqlProperties.Where(p => !p.IgnoreUpdate && !p.RowVersionProp)
                .Select(p => string.Format("{0} = @{1}", p.ColumnName, p.PropertyName))));

            if (HasRowVersion)
                query.SqlBuilder.Append(" AND " + RowVersionPropertyMetadata.ColumnName + " = @" + RowVersionPropertyMetadata.PropertyName);

            switch (Config.SqlProvider)
            {
                case SqlProvider.MSSQL:
                    //query.SqlBuilder.Append(" SELECT SCOPE_IDENTITY() AS " + IdentitySqlProperty.ColumnName);
                    query.SqlBuilder.Append("; SELECT * FROM " + TableName + " WHERE " + string.Join(" AND ",
                                                KeySqlProperties.Where(p => !p.IgnoreUpdate && !p.RowVersionProp)
                                                    .Select(p => p.ColumnName + " = @" + p.PropertyName)));
                    break;

                case SqlProvider.MySQL:
                    //query.SqlBuilder.Append("; SELECT CONVERT(LAST_INSERT_ID(), SIGNED INTEGER) AS " + IdentitySqlProperty.ColumnName);
                    query.SqlBuilder.Append("; SELECT * FROM " + TableName + " WHERE " + string.Join(" AND ",
                                                KeySqlProperties.Where(p => !p.IgnoreUpdate && !p.RowVersionProp)
                                                    .Select(p => p.ColumnName + " = @" + p.PropertyName)));
                    break;

                case SqlProvider.PostgreSQL:
                    query.SqlBuilder.Append("; SELECT * FROM " + TableName + " WHERE " + string.Join(" AND ",
                                                KeySqlProperties.Where(p => !p.IgnoreUpdate && !p.RowVersionProp)
                                                    .Select(p => p.ColumnName + " = @" + p.PropertyName)));
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            LogSqlQuery(query);
            return query;
        }

        /// <inheritdoc />
        public virtual SqlQuery GetUpdate(Expression<Func<TEntity, bool>> predicate, TEntity entity, Expression<Func<TEntity, object>> propertiesToUpdate)
        {
            var properties = SqlProperties.Where(p =>
                !KeySqlProperties.Any(k => k.PropertyName.Equals(p.PropertyName, StringComparison.OrdinalIgnoreCase)) && !p.IgnoreUpdate && !p.RowVersionProp).ToArray();

            if (propertiesToUpdate != null)
            {
                string[] updateProperties = ExpressionHelper.GetMemberName(propertiesToUpdate);
                if (HasMandatoryProp)
                    properties = properties.Where(x =>
                            updateProperties.Contains(x.PropertyName) ||
                            MandatoryUpdateProperty.Select(y => y.Name).Contains(x.PropertyName))
                        .ToArray();
                else
                    properties = properties.Where(x =>
                            updateProperties.Contains(x.PropertyName))
                        .ToArray();
            }

            if (HasUpdatedAt)
                UpdatedAtProperty.SetValue(entity, DateTime.UtcNow);

            var query = new SqlQuery(entity);

            query.SqlBuilder
                .Append("UPDATE ")
                .Append(TableName)
                .Append(" SET ");

            query.SqlBuilder.Append(string.Join(", ", properties
                .Select(p => string.Format("{0} = @{1}", p.ColumnName, p.PropertyName))));

            query.SqlBuilder
                .Append(" ");
            
            AppendWherePredicateQuery(query, predicate, QueryType.Update);

            if (HasRowVersion)
                query.SqlBuilder.Append(" AND " + RowVersionPropertyMetadata.ColumnName + " = @" + RowVersionPropertyMetadata.PropertyName);

            switch (Config.SqlProvider)
            {
                case SqlProvider.MSSQL:
                    query.SqlBuilder.Append("; SELECT * FROM " + TableName + " ");
                    AppendWherePredicateQuery(query, predicate, QueryType.Update);
                    if (HasRowVersion)
                        query.SqlBuilder.Append(" AND " + RowVersionPropertyMetadata.ColumnName + " = @" + RowVersionPropertyMetadata.PropertyName);
                    break;

                case SqlProvider.MySQL:
                    query.SqlBuilder.Append("; SELECT * FROM " + TableName + " ");
                    AppendWherePredicateQuery(query, predicate, QueryType.Update);
                    if (HasRowVersion)
                        query.SqlBuilder.Append(" AND " + RowVersionPropertyMetadata.ColumnName + " = @" + RowVersionPropertyMetadata.PropertyName);
                    break;

                case SqlProvider.PostgreSQL:
                    query.SqlBuilder.Append("; SELECT * FROM " + TableName + " ");
                    AppendWherePredicateQuery(query, predicate, QueryType.Update);
                    if (HasRowVersion)
                        query.SqlBuilder.Append(" AND " + RowVersionPropertyMetadata.ColumnName + " = @" + RowVersionPropertyMetadata.PropertyName);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            var parameters = new Dictionary<string, object>();
            var entityType = entity.GetType();
            foreach (var property in properties)
                parameters.Add(property.PropertyName, entityType.GetProperty(property.PropertyName).GetValue(entity, null));

            if (query.Param is Dictionary<string, object> whereParam)
                parameters.AddRange(whereParam);

            query.SetParam(parameters);

            LogSqlQuery(query);
            return query;
        }
    }
}
