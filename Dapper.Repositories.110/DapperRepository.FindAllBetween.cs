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
        private const string _dateTimeFormat = "yyyy-MM-dd HH:mm:ss";

        /// <inheritdoc />
        public (IEnumerable<TEntity>, int) FindAllBetween(object from, object to, Expression<Func<TEntity, object>> btwField, int pageNo, int pageSize, bool includeLogicalDeleted)
        {
            return FindAllBetween(from, to, btwField, pageNo, pageSize, includeLogicalDeleted, transaction: null);
        }

        /// <inheritdoc />
        public (IEnumerable<TEntity>, int) FindAllBetween(object from, object to, Expression<Func<TEntity, object>> btwField, int pageNo, int pageSize, bool includeLogicalDeleted, IDbTransaction transaction)
        {
            return FindAllBetween(from, to, btwField, null, pageNo, pageSize, includeLogicalDeleted, transaction);
        }

        /// <inheritdoc />
        public (IEnumerable<TEntity>, int) FindAllBetween(DateTime from, DateTime to, Expression<Func<TEntity, object>> btwField, int pageNo, int pageSize, bool includeLogicalDeleted)
        {
            return FindAllBetween(from, to, btwField, pageNo, pageSize, includeLogicalDeleted, transaction: null);
        }

        /// <inheritdoc />
        public (IEnumerable<TEntity>, int) FindAllBetween(DateTime from, DateTime to, Expression<Func<TEntity, object>> btwField, int pageNo, int pageSize, bool includeLogicalDeleted, IDbTransaction transaction)
        {
            return FindAllBetween(from, to, btwField, null, pageNo, pageSize, includeLogicalDeleted, transaction);
        }

        /// <inheritdoc />
        public (IEnumerable<TEntity>, int) FindAllBetween(DateTime from, DateTime to, Expression<Func<TEntity, object>> btwField, Expression<Func<TEntity, bool>> predicate, int pageNo, int pageSize, bool includeLogicalDeleted)
        {
            return FindAllBetween(from, to, btwField, predicate, pageNo, pageSize, includeLogicalDeleted, null);
        }

        /// <inheritdoc />
        public (IEnumerable<TEntity>, int) FindAllBetween(
            DateTime from,
            DateTime to,
            Expression<Func<TEntity, object>> btwField,
            Expression<Func<TEntity, bool>> predicate,
            int pageNo, int pageSize, bool includeLogicalDeleted, IDbTransaction transaction)
        {
            var fromString = from.ToString(_dateTimeFormat);
            var toString = to.ToString(_dateTimeFormat);
            return FindAllBetween(fromString, toString, btwField, predicate, pageNo, pageSize, includeLogicalDeleted, transaction);
        }

        /// <inheritdoc />
        public (IEnumerable<TEntity>, int) FindAllBetween(object from, object to, Expression<Func<TEntity, object>> btwField, Expression<Func<TEntity, bool>> predicate, int pageNo, int pageSize, bool includeLogicalDeleted)
        {
            return FindAllBetween(from, to, btwField, predicate, pageNo, pageSize, includeLogicalDeleted, null);
        }

        /// <inheritdoc />
        public (IEnumerable<TEntity>, int) FindAllBetween(
            object from,
            object to,
            Expression<Func<TEntity, object>> btwField,
            Expression<Func<TEntity, bool>> predicate,
            int pageNo, int pageSize, bool includeLogicalDeleted, IDbTransaction transaction)
        {
            //Query
            var queryResult = SqlGenerator.GetSelectBetween(from, to, btwField, pageNo, pageSize, includeLogicalDeleted, predicate);

            if (!queryResult.GetSql().Contains(";"))
            {
                return (Connection.Query<TEntity>(queryResult.GetSql(), queryResult.Param, transaction), 0);
            }

            var totalPages = Connection.QueryFirstOrDefault<int>(queryResult.GetSql().Split(";")[1], queryResult.Param, transaction);
            return (Connection.Query<TEntity>(queryResult.GetSql().Split(";")[0], queryResult.Param, transaction), totalPages);
        }

        /// <inheritdoc />
        public Task<(IEnumerable<TEntity>, int)> FindAllBetweenAsync(object from, object to, Expression<Func<TEntity, object>> btwField, int pageNo, int pageSize, bool includeLogicalDeleted)
        {
            return FindAllBetweenAsync(from, to, btwField, pageNo, pageSize, includeLogicalDeleted, transaction: null);
        }

        /// <inheritdoc />
        public Task<(IEnumerable<TEntity>, int)> FindAllBetweenAsync(object from, object to, Expression<Func<TEntity, object>> btwField, int pageNo, int pageSize, bool includeLogicalDeleted, IDbTransaction transaction)
        {
            return FindAllBetweenAsync(from, to, btwField, null, pageNo, pageSize, includeLogicalDeleted, transaction);
        }

        /// <inheritdoc />
        public Task<(IEnumerable<TEntity>, int)> FindAllBetweenAsync(DateTime from, DateTime to, Expression<Func<TEntity, object>> btwField, int pageNo, int pageSize, bool includeLogicalDeleted)
        {
            return FindAllBetweenAsync(from, to, btwField, pageNo, pageSize, includeLogicalDeleted, transaction: null);
        }

        /// <inheritdoc />
        public Task<(IEnumerable<TEntity>, int)> FindAllBetweenAsync(DateTime from, DateTime to, Expression<Func<TEntity, object>> btwField, int pageNo, int pageSize, bool includeLogicalDeleted, IDbTransaction transaction)
        {
            return FindAllBetweenAsync(from, to, btwField, null, pageNo, pageSize, includeLogicalDeleted, transaction);
        }

        /// <inheritdoc />
        public Task<(IEnumerable<TEntity>, int)> FindAllBetweenAsync(DateTime from, DateTime to, Expression<Func<TEntity, object>> btwField, Expression<Func<TEntity, bool>> predicate, int pageNo, int pageSize, bool includeLogicalDeleted)
        {
            return FindAllBetweenAsync(from, to, btwField, predicate, pageNo, pageSize, includeLogicalDeleted, null);
        }

        /// <inheritdoc />
        public Task<(IEnumerable<TEntity>, int)> FindAllBetweenAsync(object from, object to, Expression<Func<TEntity, object>> btwField, Expression<Func<TEntity, bool>> predicate, int pageNo, int pageSize, bool includeLogicalDeleted)
        {
            return FindAllBetweenAsync(from, to, btwField, predicate, pageNo, pageSize, includeLogicalDeleted, null);
        }

        /// <inheritdoc />
        public Task<(IEnumerable<TEntity>, int)> FindAllBetweenAsync(
            DateTime from,
            DateTime to,
            Expression<Func<TEntity, object>> btwField,
            Expression<Func<TEntity, bool>> predicate,
            int pageNo, int pageSize, bool includeLogicalDeleted, IDbTransaction transaction)
        {
            return FindAllBetweenAsync(from.ToString(_dateTimeFormat), to.ToString(_dateTimeFormat), btwField, predicate, pageNo, pageSize, includeLogicalDeleted, transaction);
        }

        /// <inheritdoc />
        public async Task<(IEnumerable<TEntity>, int)> FindAllBetweenAsync(
            object from,
            object to,
            Expression<Func<TEntity, object>> btwField,
            Expression<Func<TEntity, bool>> predicate,
            int pageNo, int pageSize, bool includeLogicalDeleted, IDbTransaction transaction)
        {
            //Query
            var queryResult = SqlGenerator.GetSelectBetween(from, to, btwField, pageNo, pageSize, includeLogicalDeleted, predicate);

            if (!queryResult.GetSql().Contains(";"))
                return (await Connection.QueryAsync<TEntity>(queryResult.GetSql(), queryResult.Param, transaction), 0);

            var data = await Connection.QueryAsync<TEntity>(queryResult.GetSql().Split(";")[0], queryResult.Param, transaction);
            var tPages = await Connection.QueryFirstOrDefaultAsync<int>(queryResult.GetSql().Split(";")[1], queryResult.Param, transaction);
            return (data, tPages);
        }
    }
}
