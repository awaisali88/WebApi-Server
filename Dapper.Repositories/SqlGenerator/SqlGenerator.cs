﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Common;
using Dapper.Repositories.Attributes.Joins;
using Dapper.Repositories.Attributes.LogicalDelete;
using Dapper.Repositories.Extensions;
using Serilog;

namespace Dapper.Repositories.SqlGenerator
{
    /// <inheritdoc />
    public partial class SqlGenerator<TEntity> : ISqlGenerator<TEntity>
        where TEntity : class
    {
        private readonly bool _logQuery;
        /// <summary>
        ///     Constructor
        /// </summary>
        public SqlGenerator()
            : this(new SqlGeneratorConfig
            {
                SqlProvider = SqlProvider.MSSQL,
                UseQuotationMarks = false,
                LogQuery = false
            })
        {
            _logQuery = Config.LogQuery;
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        public SqlGenerator(SqlProvider sqlProvider, bool useQuotationMarks = false, bool logQuery = false)
            : this(new SqlGeneratorConfig
            {
                SqlProvider = sqlProvider,
                UseQuotationMarks = useQuotationMarks,
                LogQuery = logQuery
            })
        {
            _logQuery = Config.LogQuery;
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        public SqlGenerator(SqlGeneratorConfig sqlGeneratorConfig)
        {
            // Order is important
            InitProperties();
            InitConfig(sqlGeneratorConfig);
            InitLogicalDeleted();

            _logQuery = sqlGeneratorConfig.LogQuery;
        }

        /// <inheritdoc />
        public PropertyInfo[] AllProperties { get; protected set; }

        /// <inheritdoc />
        public bool HasUpdatedAt => UpdatedAtProperty != null;

        /// <inheritdoc />
        public bool HasCreatedAt => CreatedAtProperty != null;

        /// <inheritdoc />
        public bool HasRowVersion => RowVersionProperty != null;

        /// <inheritdoc />
        public PropertyInfo UpdatedAtProperty { get; protected set; }

        /// <inheritdoc />
        public PropertyInfo CreatedAtProperty { get; protected set; }

        /// <inheritdoc />
        public PropertyInfo RowVersionProperty { get; protected set; }

        /// <inheritdoc />
        public SqlPropertyMetadata UpdatedAtPropertyMetadata { get; protected set; }

        /// <inheritdoc />
        public SqlPropertyMetadata CreatedAtPropertyMetadata { get; protected set; }

        /// <inheritdoc />
        public SqlPropertyMetadata RowVersionPropertyMetadata { get; protected set; }

        /// <inheritdoc />
        public bool IsIdentity => IdentitySqlProperty != null;

        /// <inheritdoc />
        public string TableName { get; protected set; }

        /// <inheritdoc />
        public string TableSchema { get; protected set; }

        /// <inheritdoc />
        public SqlPropertyMetadata IdentitySqlProperty { get; protected set; }

        /// <inheritdoc />
        public SqlPropertyMetadata[] KeySqlProperties { get; protected set; }

        /// <inheritdoc />
        public SqlPropertyMetadata[] SqlProperties { get; protected set; }

        /// <inheritdoc />
        public SqlJoinPropertyMetadata[] SqlJoinProperties { get; protected set; }

        /// <inheritdoc />
        public SqlGeneratorConfig Config { get; protected set; }

        /// <inheritdoc />
        public bool LogicalDelete { get; protected set; }

        /// <inheritdoc />
        public string StatusPropertyName { get; protected set; }

        /// <inheritdoc />
        public object LogicalDeleteValue { get; protected set; }

        /// <inheritdoc />
        public virtual SqlQuery GetCount(Expression<Func<TEntity, bool>> predicate, bool includeLogicalDeleted)
        {
            var sqlQuery = new SqlQuery();

            sqlQuery.SqlBuilder
                .Append("SELECT COUNT(*) FROM ")
                .Append(TableName)
                .Append(" ");

            AppendWherePredicateQuery(sqlQuery, predicate, QueryType.Select, includeLogicalDeleted);

            LogSqlQuery(sqlQuery);
            return sqlQuery;
        }

        /// <inheritdoc />
        public virtual SqlQuery GetCount(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> distinctField, bool includeLogicalDeleted)
        {
            var propertyName = ExpressionHelper.GetPropertyName(distinctField);
            var property = SqlProperties.First(x => x.PropertyName == propertyName);
            var sqlQuery = InitBuilderCountWithDistinct(property);

            sqlQuery.SqlBuilder
                .Append(" FROM ")
                .Append(TableName)
                .Append(" ");

            AppendWherePredicateQuery(sqlQuery, predicate, QueryType.Select, includeLogicalDeleted);

            LogSqlQuery(sqlQuery);
            return sqlQuery;
        }

        /// <inheritdoc />
        public virtual SqlQuery GetSelectFirst(Expression<Func<TEntity, bool>> predicate, bool includeLogicalDeleted, params Expression<Func<TEntity, object>>[] includes)
        {
            return GetSelect(predicate, true, includeLogicalDeleted, includes);
        }

        /// <inheritdoc />
        public virtual SqlQuery GetSelectAll(Expression<Func<TEntity, bool>> predicate, bool includeLogicalDeleted, params Expression<Func<TEntity, object>>[] includes)
        {
            return GetSelect(predicate, false, includeLogicalDeleted, includes);
        }

        /// <inheritdoc />
        public SqlQuery GetSelectById(object id, bool includeLogicalDeleted, params Expression<Func<TEntity, object>>[] includes)
        {
            if (KeySqlProperties.Length != 1)
                throw new NotSupportedException("GetSelectById support only 1 key");

            var keyProperty = KeySqlProperties[0];

            var sqlQuery = InitBuilderSelect(true);

            if (includes.Any())
            {
                var joinsBuilder = AppendJoinToSelect(sqlQuery, includes);
                sqlQuery.SqlBuilder
                    .Append(" FROM ")
                    .Append(TableName)
                    .Append(" ");

                sqlQuery.SqlBuilder.Append(joinsBuilder);
            }
            else
            {
                sqlQuery.SqlBuilder
                    .Append(" FROM ")
                    .Append(TableName)
                    .Append(" ");
            }

            IDictionary<string, object> dictionary = new Dictionary<string, object>
            {
                { keyProperty.PropertyName, id }
            };

            sqlQuery.SqlBuilder
                .Append("WHERE ")
                .Append(TableName)
                .Append(".")
                .Append(keyProperty.ColumnName)
                .Append(" = @")
                .Append(keyProperty.PropertyName)
                .Append(" ");

            if (LogicalDelete && !includeLogicalDeleted)
                sqlQuery.SqlBuilder
                    .Append("AND ")
                    .Append(TableName)
                    .Append(".")
                    .Append(StatusPropertyName)
                    .Append(" != ")
                    .Append(LogicalDeleteValue)
                    .Append(" ");

            if (Config.SqlProvider == SqlProvider.MySQL || Config.SqlProvider == SqlProvider.PostgreSQL)
                sqlQuery.SqlBuilder.Append("LIMIT 1");

            sqlQuery.SetParam(dictionary);

            LogSqlQuery(sqlQuery);
            return sqlQuery;
        }

        /// <inheritdoc />
        public virtual SqlQuery GetSelectBetween(object from, object to, Expression<Func<TEntity, object>> btwField,
            bool includeLogicalDeleted, Expression<Func<TEntity, bool>> expression = null)
        {
            var fieldName = ExpressionHelper.GetPropertyName(btwField);
            var columnName = SqlProperties.First(x => x.PropertyName == fieldName).ColumnName;
            var query = GetSelectAll(expression, includeLogicalDeleted);

            query.SqlBuilder
                .Append(expression == null && !LogicalDelete ? "WHERE" : "AND")
                .Append(" ")
                .Append(TableName)
                .Append(".")
                .Append(columnName)
                .Append(" BETWEEN '")
                .Append(from)
                .Append("' AND '")
                .Append(to)
                .Append("'");

            LogSqlQuery(query);
            return query;
        }

        /// <inheritdoc />
        public virtual SqlQuery GetDelete(TEntity entity)
        {
            var sqlQuery = new SqlQuery();
            var whereSql = " WHERE " + string.Join(" AND ", KeySqlProperties.Select(p => TableName + "." + p.ColumnName + " = @" + p.PropertyName));
            if (HasRowVersion)
                whereSql += " AND " + RowVersionPropertyMetadata.ColumnName + " = @" + RowVersionPropertyMetadata.PropertyName;

            if (!LogicalDelete)
            {
                sqlQuery.SqlBuilder.Append("DELETE FROM " + TableName + whereSql);
            }
            else
            {
                return GetUpdate(entity);
                //sqlQuery.SqlBuilder.Append("UPDATE " + TableName + " SET " + StatusPropertyName + " = " + LogicalDeleteValue);
                //
                //if (HasUpdatedAt)
                //{
                //    UpdatedAtProperty.SetValue(entity, DateTime.UtcNow);
                //    sqlQuery.SqlBuilder.Append(", " + UpdatedAtPropertyMetadata.ColumnName + " = @" + UpdatedAtPropertyMetadata.PropertyName);
                //}
                //
                //sqlQuery.SqlBuilder.Append(whereSql);
            }

            sqlQuery.SetParam(entity);

            LogSqlQuery(sqlQuery);
            return sqlQuery;
        }

        /// <inheritdoc />
        public virtual SqlQuery GetDelete(Expression<Func<TEntity, bool>> predicate, TEntity entity)
        {
            var sqlQuery = new SqlQuery();

            if (!LogicalDelete)
            {
                sqlQuery.SqlBuilder.Append("DELETE FROM " + TableName + " ");
            }
            else
            {
                return GetUpdate(predicate, entity);

                //sqlQuery.SqlBuilder.Append("UPDATE " + TableName + " SET " + StatusPropertyName + " = " + LogicalDeleteValue);
                //sqlQuery.SqlBuilder.Append(HasUpdatedAt
                //    ? ", " + UpdatedAtPropertyMetadata.ColumnName + " = @" + UpdatedAtPropertyMetadata.PropertyName + " "
                //    : " ");
            }

            AppendWherePredicateQuery(sqlQuery, predicate, QueryType.Delete);
            if (HasRowVersion)
                sqlQuery.SqlBuilder.Append(" AND " + RowVersionPropertyMetadata.ColumnName + " = @" + RowVersionPropertyMetadata.PropertyName);

            LogSqlQuery(sqlQuery);
            return sqlQuery;
        }

        /// <inheritdoc />
        public virtual SqlQuery GetInsert(TEntity entity)
        {
            var properties =
                (IsIdentity
                    ? SqlProperties.Where(p => !p.PropertyName.Equals(IdentitySqlProperty.PropertyName, StringComparison.OrdinalIgnoreCase) && !p.IgnoreCreate && !p.RowVersionProp)
                    : SqlProperties).ToList();

            if (HasCreatedAt)
                CreatedAtProperty.SetValue(entity, DateTime.UtcNow);

            if (HasUpdatedAt)
                UpdatedAtProperty.SetValue(entity, DateTime.UtcNow);

            var query = new SqlQuery(entity);

            query.SqlBuilder.Append(
                "INSERT INTO " + TableName
                               + " (" + string.Join(", ", properties.Select(p => p.ColumnName)) + ")" // columNames
                               + " VALUES (" + string.Join(", ", properties.Select(p => "@" + p.PropertyName)) + ")"); // values

            if (IsIdentity)
                switch (Config.SqlProvider)
                {
                    case SqlProvider.MSSQL:
                        //query.SqlBuilder.Append(" SELECT SCOPE_IDENTITY() AS " + IdentitySqlProperty.ColumnName);
                        query.SqlBuilder.Append(" SELECT * FROM " + TableName + " WHERE " + IdentitySqlProperty.ColumnName + " = SCOPE_IDENTITY()");
                        break;

                    case SqlProvider.MySQL:
                        //query.SqlBuilder.Append("; SELECT CONVERT(LAST_INSERT_ID(), SIGNED INTEGER) AS " + IdentitySqlProperty.ColumnName);
                        query.SqlBuilder.Append("; SELECT * FROM " + TableName + " WHERE " + IdentitySqlProperty.ColumnName + " = CONVERT(LAST_INSERT_ID(), SIGNED INTEGER)");
                        break;

                    case SqlProvider.PostgreSQL:
                        query.SqlBuilder.Append(" RETURNING " + IdentitySqlProperty.ColumnName);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }

            LogSqlQuery(query);
            return query;
        }

        /// <inheritdoc />
        public virtual SqlQuery GetBulkInsert(IEnumerable<TEntity> entities)
        {
            var entitiesArray = entities as TEntity[] ?? entities.ToArray();
            if (!entitiesArray.Any())
                throw new ArgumentException("collection is empty");

            var entityType = entitiesArray[0].GetType();

            var properties =
                (IsIdentity
                    ? SqlProperties.Where(p => !p.PropertyName.Equals(IdentitySqlProperty.PropertyName, StringComparison.OrdinalIgnoreCase) && !p.IgnoreCreate && !p.RowVersionProp)
                    : SqlProperties).ToList();

            var query = new SqlQuery();

            var values = new List<string>();
            var parameters = new Dictionary<string, object>();

            for (var i = 0; i < entitiesArray.Length; i++)
            {
                if (HasCreatedAt)
                    CreatedAtProperty.SetValue(entitiesArray[i], DateTime.UtcNow);

                if (HasUpdatedAt)
                    UpdatedAtProperty.SetValue(entitiesArray[i], DateTime.UtcNow);

                foreach (var property in properties)
                    // ReSharper disable once PossibleNullReferenceException
                    parameters.Add(property.PropertyName + i, entityType.GetProperty(property.PropertyName).GetValue(entitiesArray[i], null));

                values.Add("(" + string.Join(", ", properties.Select(p => "@" + p.PropertyName + i)) + ")");
            }

            query.SqlBuilder.Append(
                "INSERT INTO " + TableName
                               + " (" + string.Join(", ", properties.Select(p => p.ColumnName)) + ")" // columNames
                               + " VALUES " + string.Join(",", values)); // values

            query.SetParam(parameters);

            LogSqlQuery(query);
            return query;
        }

        /// <inheritdoc />
        public virtual SqlQuery GetUpdate(TEntity entity)
        {
            var properties = SqlProperties.Where(p =>
                !KeySqlProperties.Any(k => k.PropertyName.Equals(p.PropertyName, StringComparison.OrdinalIgnoreCase)) &&
                !p.IgnoreUpdate && !p.RowVersionProp).ToArray();
            if (!properties.Any())
                throw new ArgumentException("Can't update without [Key]");

            if (HasUpdatedAt)
                UpdatedAtProperty.SetValue(entity, DateTime.UtcNow);

            var query = new SqlQuery(entity);
            query.SqlBuilder.Append("UPDATE " + TableName + " SET " +
                                    string.Join(", ", properties.Select(p => p.ColumnName + " = @" + p.PropertyName))
                                    + " WHERE " + string.Join(" AND ",
                                        KeySqlProperties.Where(p => !p.IgnoreUpdate && !p.RowVersionProp)
                                            .Select(p => p.ColumnName + " = @" + p.PropertyName)));

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
                    throw new ArgumentOutOfRangeException();

                default:
                    throw new ArgumentOutOfRangeException();
            }

            LogSqlQuery(query);
            return query;
        }


        /// <inheritdoc />
        public virtual SqlQuery GetUpdate(Expression<Func<TEntity, bool>> predicate, TEntity entity)
        {
            var properties = SqlProperties.Where(p =>
                !KeySqlProperties.Any(k => k.PropertyName.Equals(p.PropertyName, StringComparison.OrdinalIgnoreCase)) && !p.IgnoreUpdate && !p.RowVersionProp);

            if (HasUpdatedAt)
                UpdatedAtProperty.SetValue(entity, DateTime.UtcNow);

            var query = new SqlQuery(entity);
            query.SqlBuilder.Append("UPDATE " + TableName + " SET " + string.Join(", ", properties.Select(p => p.ColumnName + " = @" + p.PropertyName)) + " ");
            AppendWherePredicateQuery(query, predicate, QueryType.Update);

            if (HasRowVersion)
                query.SqlBuilder.Append(" AND " + RowVersionPropertyMetadata.ColumnName + " = @" + RowVersionPropertyMetadata.PropertyName);

            switch (Config.SqlProvider)
            {
                case SqlProvider.MSSQL:
                    //query.SqlBuilder.Append(" SELECT SCOPE_IDENTITY() AS " + IdentitySqlProperty.ColumnName);
                    query.SqlBuilder.Append(" SELECT * FROM " + TableName + " WHERE " + string.Join(" AND ",
                                                KeySqlProperties.Where(p => !p.IgnoreUpdate && !p.RowVersionProp).Select(p => p.ColumnName + " = @" + p.PropertyName)));
                    break;

                case SqlProvider.MySQL:
                    //query.SqlBuilder.Append("; SELECT CONVERT(LAST_INSERT_ID(), SIGNED INTEGER) AS " + IdentitySqlProperty.ColumnName);
                    query.SqlBuilder.Append("; SELECT * FROM " + TableName + " WHERE " + string.Join(" AND ",
                                                KeySqlProperties.Where(p => !p.IgnoreUpdate && !p.RowVersionProp).Select(p => p.ColumnName + " = @" + p.PropertyName)));
                    break;

                case SqlProvider.PostgreSQL:
                    throw new ArgumentOutOfRangeException();

                default:
                    throw new ArgumentOutOfRangeException();
            }

            LogSqlQuery(query);
            return query;
        }


        /// <inheritdoc />
        public virtual SqlQuery GetBulkUpdate(IEnumerable<TEntity> entities)
        {
            var entitiesArray = entities as TEntity[] ?? entities.ToArray();
            if (!entitiesArray.Any())
                throw new ArgumentException("collection is empty");

            var entityType = entitiesArray[0].GetType();

            var properties = SqlProperties.Where(p =>
                !KeySqlProperties.Any(k => k.PropertyName.Equals(p.PropertyName, StringComparison.OrdinalIgnoreCase)) && !p.IgnoreUpdate && !p.RowVersionProp).ToArray();

            var query = new SqlQuery();

            var parameters = new Dictionary<string, object>();

            for (var i = 0; i < entitiesArray.Length; i++)
            {
                if (HasUpdatedAt)
                    UpdatedAtProperty.SetValue(entitiesArray[i], DateTime.UtcNow);

                if (i > 0)
                    query.SqlBuilder.Append("; ");

                query.SqlBuilder.Append("UPDATE " + TableName + " SET " + string.Join(", ", properties.Select(p => p.ColumnName + " = @" + p.PropertyName + i))
                                        + " WHERE " + string.Join(" AND ",
                                            KeySqlProperties.Where(p => !p.IgnoreUpdate && !p.RowVersionProp).Select(p => p.ColumnName + " = @" + p.PropertyName + i)));

                if (HasRowVersion)
                    query.SqlBuilder.Append(" AND " + RowVersionPropertyMetadata.ColumnName + " = @" + RowVersionPropertyMetadata.PropertyName);

                // ReSharper disable PossibleNullReferenceException
                foreach (var property in properties)
                    parameters.Add(property.PropertyName + i, entityType.GetProperty(property.PropertyName).GetValue(entitiesArray[i], null));

                foreach (var property in KeySqlProperties.Where(p => !p.IgnoreUpdate && !p.RowVersionProp))
                    parameters.Add(property.PropertyName + i, entityType.GetProperty(property.PropertyName).GetValue(entitiesArray[i], null));

                foreach (var property in KeySqlProperties.Where(p => p.RowVersionProp))
                    parameters.Add(property.PropertyName + i, entityType.GetProperty(property.PropertyName).GetValue(entitiesArray[i], null));
                // ReSharper restore PossibleNullReferenceException
            }

            query.SetParam(parameters);

            LogSqlQuery(query);
            return query;
        }

        private void AppendWherePredicateQuery(SqlQuery sqlQuery, Expression<Func<TEntity, bool>> predicate, QueryType queryType, bool includeLogicalDeleted = false)
        {
            IDictionary<string, object> dictionaryParams = new Dictionary<string, object>();

            if (predicate != null)
            {
                // WHERE
                var queryProperties = new List<QueryParameter>();
                FillQueryProperties(predicate.Body, ExpressionType.Default, ref queryProperties);

                sqlQuery.SqlBuilder.Append("WHERE ");

                for (var i = 0; i < queryProperties.Count; i++)
                {
                    var item = queryProperties[i];
                    var tableName = TableName;
                    string columnName;
                    if (item.NestedProperty)
                    {
                        var joinProperty = SqlJoinProperties.First(x => x.PropertyName == item.PropertyName);
                        tableName = joinProperty.TableAlias;
                        columnName = joinProperty.ColumnName;
                    }
                    else
                    {
                        columnName = SqlProperties.First(x => x.PropertyName == item.PropertyName).ColumnName;
                    }

                    if (!string.IsNullOrEmpty(item.LinkingOperator) && i > 0)
                        sqlQuery.SqlBuilder.Append(item.LinkingOperator + " ");

                    if (item.PropertyValue == null)
                        sqlQuery.SqlBuilder.Append(tableName + "." + columnName + " " + (item.QueryOperator == "=" ? "IS" : "IS NOT") + " NULL ");
                    else
                        sqlQuery.SqlBuilder.Append(tableName + "." + columnName + " " + item.QueryOperator + " @" + item.PropertyName + " ");


                    dictionaryParams[item.PropertyName] = item.PropertyValue;
                }

                if (LogicalDelete && queryType == QueryType.Select && !includeLogicalDeleted)
                    sqlQuery.SqlBuilder.Append("AND " + TableName + "." + StatusPropertyName + " != " + LogicalDeleteValue + " ");
            }
            else
            {
                if (LogicalDelete && queryType == QueryType.Select && !includeLogicalDeleted)
                    sqlQuery.SqlBuilder.Append("WHERE " + TableName + "." + StatusPropertyName + " != " + LogicalDeleteValue + " ");
            }

            if (LogicalDelete && HasUpdatedAt && queryType == QueryType.Delete)
                dictionaryParams.Add(UpdatedAtPropertyMetadata.ColumnName, DateTime.UtcNow);

            sqlQuery.SetParam(dictionaryParams);
        }


        /// <summary>
        ///     Get join/nested properties
        /// </summary>
        /// <returns></returns>
        private static SqlJoinPropertyMetadata[] GetJoinPropertyMetadata(PropertyInfo[] joinPropertiesInfo)
        {
            // Filter and get only non collection nested properties
            var singleJoinTypes = joinPropertiesInfo.Where(p => !p.PropertyType.IsConstructedGenericType).ToArray();

            var joinPropertyMetadatas = new List<SqlJoinPropertyMetadata>();

            foreach (var propertyInfo in singleJoinTypes)
            {
                var joinInnerProperties = propertyInfo.PropertyType.GetProperties().Where(q => q.CanWrite)
                    .Where(ExpressionHelper.GetPrimitivePropertiesPredicate()).ToArray();
                joinPropertyMetadatas.AddRange(joinInnerProperties.Where(p => !p.GetCustomAttributes<NotMappedAttribute>().Any())
                    .Select(p => new SqlJoinPropertyMetadata(propertyInfo, p)).ToArray());
            }

            return joinPropertyMetadatas.ToArray();
        }

        private static string GetTableNameWithSchemaPrefix(string tableName, string tableSchema, string startQuotationMark = "", string endQuotationMark = "")
        {
            return !string.IsNullOrEmpty(tableSchema)
                ? startQuotationMark + tableSchema + endQuotationMark + "." + startQuotationMark + tableName + endQuotationMark
                : startQuotationMark + tableName + endQuotationMark;
        }

        private void InitLogicalDeleted()
        {
            var statusProperty =
                SqlProperties.FirstOrDefault(x => x.PropertyInfo.GetCustomAttributes<StatusAttribute>().Any());

            if (statusProperty == null)
                return;
            StatusPropertyName = statusProperty.ColumnName;

            if (statusProperty.PropertyInfo.PropertyType.IsBool())
            {
                var deleteProperty = AllProperties.FirstOrDefault(p => p.GetCustomAttributes<DeletedAttribute>().Any());
                if (deleteProperty == null)
                    return;

                LogicalDelete = true;
                LogicalDeleteValue = 1; // true
            }
            else if (statusProperty.PropertyInfo.PropertyType.IsEnum())
            {
                var deleteOption = statusProperty.PropertyInfo.PropertyType.GetFields().FirstOrDefault(f => f.GetCustomAttribute<DeletedAttribute>() != null);

                if (deleteOption == null)
                    return;

                var enumValue = Enum.Parse(statusProperty.PropertyInfo.PropertyType, deleteOption.Name);
                LogicalDeleteValue = Convert.ChangeType(enumValue, Enum.GetUnderlyingType(statusProperty.PropertyInfo.PropertyType));

                LogicalDelete = true;
            }
        }

        private SqlQuery InitBuilderSelect(bool firstOnly)
        {
            var query = new SqlQuery();
            query.SqlBuilder.Append("SELECT " + (firstOnly && Config.SqlProvider == SqlProvider.MSSQL ? "TOP 1 " : "") +
                                    GetFieldsSelect(TableName, SqlProperties));
            return query;
        }

        private SqlQuery InitBuilderCountWithDistinct(SqlPropertyMetadata sqlProperty)
        {
            var query = new SqlQuery();
            var partSqlCountQuery = !string.IsNullOrEmpty(sqlProperty.Alias)
                ? TableName + "." + sqlProperty.ColumnName + ") AS " + sqlProperty.PropertyName
                : TableName + "." + sqlProperty.ColumnName + ")";
            query.SqlBuilder.Append("SELECT COUNT(DISTINCT " + partSqlCountQuery);
            return query;
        }

        private string AppendJoinToSelect(SqlQuery originalBuilder, params Expression<Func<TEntity, object>>[] includes)
        {
            var joinBuilder = new StringBuilder();

            foreach (var include in includes)
            {
                var joinProperty = AllProperties.First(q => q.Name == ExpressionHelper.GetPropertyName(include));
                var declaringType = joinProperty.DeclaringType.GetTypeInfo();
                var tableAttribute = declaringType.GetCustomAttribute<TableAttribute>();
                var tableName = tableAttribute != null ? tableAttribute.Name : declaringType.Name;

                var attrJoin = joinProperty.GetCustomAttribute<JoinAttributeBase>();

                if (attrJoin == null)
                    continue;

                var joinString = "";
                if (attrJoin is LeftJoinAttribute)
                    joinString = "LEFT JOIN";
                else if (attrJoin is InnerJoinAttribute)
                    joinString = "INNER JOIN";
                else if (attrJoin is RightJoinAttribute)
                    joinString = "RIGHT JOIN";

                var joinType = joinProperty.PropertyType.IsGenericType() ? joinProperty.PropertyType.GenericTypeArguments[0] : joinProperty.PropertyType;
                var properties = joinType.FindClassProperties().Where(ExpressionHelper.GetPrimitivePropertiesPredicate());
                var props = properties.Where(p => !p.GetCustomAttributes<NotMappedAttribute>().Any()).Select(p => new SqlPropertyMetadata(p)).ToArray();

                if (Config.UseQuotationMarks)
                    switch (Config.SqlProvider)
                    {
                        case SqlProvider.MSSQL:
                            tableName = "[" + tableName + "]";
                            attrJoin.TableName = GetTableNameWithSchemaPrefix(attrJoin.TableName, attrJoin.TableSchema, "[", "]");
                            attrJoin.Key = "[" + attrJoin.Key + "]";
                            attrJoin.ExternalKey = "[" + attrJoin.ExternalKey + "]";
                            attrJoin.TableAlias = "[" + attrJoin.TableAlias + "]";
                            foreach (var prop in props)
                                prop.ColumnName = "[" + prop.ColumnName + "]";
                            break;

                        case SqlProvider.MySQL:
                            tableName = "`" + tableName + "`";
                            attrJoin.TableName = GetTableNameWithSchemaPrefix(attrJoin.TableName, attrJoin.TableSchema, "`", "`");
                            attrJoin.Key = "`" + attrJoin.Key + "`";
                            attrJoin.ExternalKey = "`" + attrJoin.ExternalKey + "`";
                            attrJoin.TableAlias = "`" + attrJoin.TableAlias + "`";
                            foreach (var prop in props)
                                prop.ColumnName = "`" + prop.ColumnName + "`";
                            break;

                        case SqlProvider.PostgreSQL:
                            tableName = "\"" + tableName + "\"";
                            attrJoin.TableName = GetTableNameWithSchemaPrefix(attrJoin.TableName, attrJoin.TableSchema, "\"", "\"");
                            attrJoin.Key = "\"" + attrJoin.Key + "\"";
                            attrJoin.ExternalKey = "\"" + attrJoin.ExternalKey + "\"";
                            attrJoin.TableAlias = "\"" + attrJoin.TableAlias + "\"";
                            foreach (var prop in props)
                                prop.ColumnName = "\"" + prop.ColumnName + "\"";
                            break;

                        default:
                            throw new ArgumentOutOfRangeException(nameof(Config.SqlProvider));
                    }
                else
                    attrJoin.TableName = GetTableNameWithSchemaPrefix(attrJoin.TableName, attrJoin.TableSchema);

                originalBuilder.SqlBuilder.Append($", {GetFieldsSelect(attrJoin.TableAlias, props)}");
                joinBuilder.Append(
                    $"{joinString} {attrJoin.TableName} AS {attrJoin.TableAlias} ON {tableName}.{attrJoin.Key} = {attrJoin.TableAlias}.{attrJoin.ExternalKey} ");
            }

            return joinBuilder.ToString();
        }


        private static string GetFieldsSelect(string tableName, IEnumerable<SqlPropertyMetadata> properties)
        {
            //Projection function
            string ProjectionFunction(SqlPropertyMetadata p)
            {
                return !string.IsNullOrEmpty(p.Alias)
                    ? tableName + "." + p.ColumnName + " AS " + p.PropertyName
                    : tableName + "." + p.ColumnName;
            }

            return string.Join(", ", properties.Select(ProjectionFunction));
        }

        private SqlQuery GetSelect(Expression<Func<TEntity, bool>> predicate, bool firstOnly, bool includeLogicalDeleted, params Expression<Func<TEntity, object>>[] includes)
        {
            var sqlQuery = InitBuilderSelect(firstOnly);

            if (includes.Any())
            {
                var joinsBuilder = AppendJoinToSelect(sqlQuery, includes);
                sqlQuery.SqlBuilder.Append(" FROM " + TableName + " ");
                sqlQuery.SqlBuilder.Append(joinsBuilder);
            }
            else
            {
                sqlQuery.SqlBuilder.Append(" FROM " + TableName + " ");
            }

            AppendWherePredicateQuery(sqlQuery, predicate, QueryType.Select, includeLogicalDeleted);

            if (firstOnly && (Config.SqlProvider == SqlProvider.MySQL || Config.SqlProvider == SqlProvider.PostgreSQL))
                sqlQuery.SqlBuilder.Append("LIMIT 1");

            LogSqlQuery(sqlQuery);
            return sqlQuery;
        }

        /// <summary>
        ///     Fill query properties
        /// </summary>
        /// <param name="expr">The expression.</param>
        /// <param name="linkingType">Type of the linking.</param>
        /// <param name="queryProperties">The query properties.</param>
        private void FillQueryProperties(Expression expr, ExpressionType linkingType, ref List<QueryParameter> queryProperties)
        {
            if (expr is MethodCallExpression body)
            {
                var innerBody = body;
                var methodName = innerBody.Method.Name;
                switch (methodName)
                {
                    case "Contains":
                        {
                            var propertyName = ExpressionHelper.GetPropertyNamePath(innerBody, out var isNested);

                            if (!SqlProperties.Select(x => x.PropertyName).Contains(propertyName) &&
                                !SqlJoinProperties.Select(x => x.PropertyName).Contains(propertyName))
                                throw new NotSupportedException("predicate can't parse");

                            var propertyValue = ExpressionHelper.GetValuesFromCollection(innerBody);
                            var opr = ExpressionHelper.GetMethodCallSqlOperator(methodName);
                            var link = ExpressionHelper.GetSqlOperator(linkingType);
                            queryProperties.Add(new QueryParameter(link, propertyName, propertyValue, opr, isNested));
                            break;
                        }
                    default:
                        throw new NotSupportedException($"'{methodName}' method is not supported");
                }
            }
            else if (expr is BinaryExpression)
            {
                var innerbody = (BinaryExpression) expr;
                if (innerbody.NodeType != ExpressionType.AndAlso && innerbody.NodeType != ExpressionType.OrElse)
                {
                    var propertyName = ExpressionHelper.GetPropertyNamePath(innerbody, out var isNested);

                    if (!SqlProperties.Select(x => x.PropertyName).Contains(propertyName) &&
                        !SqlJoinProperties.Select(x => x.PropertyName).Contains(propertyName))
                        throw new NotSupportedException("predicate can't parse");

                    var propertyValue = ExpressionHelper.GetValue(innerbody.Right);
                    var opr = ExpressionHelper.GetSqlOperator(innerbody.NodeType);
                    var link = ExpressionHelper.GetSqlOperator(linkingType);

                    queryProperties.Add(new QueryParameter(link, propertyName, propertyValue, opr, isNested));
                }
                else
                {
                    FillQueryProperties(innerbody.Left, innerbody.NodeType, ref queryProperties);
                    FillQueryProperties(innerbody.Right, innerbody.NodeType, ref queryProperties);
                }
            }
            else
            {
                FillQueryProperties(ExpressionHelper.GetBinaryExpression(expr), linkingType, ref queryProperties);
            }
        }

        private enum QueryType
        {
            Select,
            Delete,
            Update
        }

        private void LogSqlQuery(SqlQuery query)
        {
            if (_logQuery && query != null && !string.IsNullOrEmpty(query.GetSql()))
            {
                Log.Information("Query: {@SqlQuery} \nParam: {@Parameters}", query.GetSql(), query.Param);
                //Debug.WriteLine(query.GetSql());
            }
        }
    }
}
