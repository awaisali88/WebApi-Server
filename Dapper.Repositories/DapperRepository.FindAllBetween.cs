using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Dapper.Repositories
{
    /// <summary>
    ///     Base Repository
    /// </summary>
    public partial class DapperRepository<TEntity>
        where TEntity : class
    {
        private const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

        /// <inheritdoc />
        public IEnumerable<TEntity> FindAllBetween(object from, object to, Expression<Func<TEntity, object>> btwField, bool includeLogicalDeleted, IDbTransaction transaction = null)
        {
            return FindAllBetween(from, to, btwField, null, includeLogicalDeleted, transaction);
        }

        /// <inheritdoc />
        public IEnumerable<TEntity> FindAllBetween(object from, object to, Expression<Func<TEntity, object>> btwField, Expression<Func<TEntity, bool>> predicate = null,
            bool includeLogicalDeleted = false, IDbTransaction transaction = null)
        {
            var queryResult = SqlGenerator.GetSelectBetween(from, to, btwField, includeLogicalDeleted, predicate);
            return Connection.Query<TEntity>(queryResult.GetSql(), queryResult.Param, transaction);
        }

        /// <inheritdoc />
        public IEnumerable<TEntity> FindAllBetween(DateTime from, DateTime to, Expression<Func<TEntity, object>> btwField, bool includeLogicalDeleted, IDbTransaction transaction = null)
        {
            return FindAllBetween(from, to, btwField, null, includeLogicalDeleted, transaction);
        }

        /// <inheritdoc />
        public IEnumerable<TEntity> FindAllBetween(DateTime from, DateTime to, Expression<Func<TEntity, object>> btwField, Expression<Func<TEntity, bool>> predicate,
            bool includeLogicalDeleted, IDbTransaction transaction = null)
        {
            var fromString = from.ToString(DateTimeFormat);
            var toString = to.ToString(DateTimeFormat);
            return FindAllBetween(fromString, toString, btwField, predicate, includeLogicalDeleted);
        }

        /// <inheritdoc />
        public Task<IEnumerable<TEntity>> FindAllBetweenAsync(object from, object to, Expression<Func<TEntity, object>> btwField, bool includeLogicalDeleted, IDbTransaction transaction = null)
        {
            return FindAllBetweenAsync(from, to, btwField, null, includeLogicalDeleted, transaction);
        }

        /// <inheritdoc />
        public Task<IEnumerable<TEntity>> FindAllBetweenAsync(object from, object to, Expression<Func<TEntity, object>> btwField, Expression<Func<TEntity, bool>> predicate,
            bool includeLogicalDeleted, IDbTransaction transaction = null)
        {
            var queryResult = SqlGenerator.GetSelectBetween(from, to, btwField, includeLogicalDeleted, predicate);
            return Connection.QueryAsync<TEntity>(queryResult.GetSql(), queryResult.Param, transaction);
        }

        /// <inheritdoc />
        public Task<IEnumerable<TEntity>> FindAllBetweenAsync(DateTime from, DateTime to, Expression<Func<TEntity, object>> btwField, bool includeLogicalDeleted, IDbTransaction transaction = null)
        {
            return FindAllBetweenAsync(from, to, btwField, null, includeLogicalDeleted, transaction);
        }

        /// <inheritdoc />
        public Task<IEnumerable<TEntity>> FindAllBetweenAsync(DateTime from, DateTime to, Expression<Func<TEntity, object>> btwField, Expression<Func<TEntity, bool>> predicate,
            bool includeLogicalDeleted, IDbTransaction transaction = null)
        {
            return FindAllBetweenAsync(from.ToString(DateTimeFormat), to.ToString(DateTimeFormat), btwField, predicate, includeLogicalDeleted, transaction);
        }
    }
}