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
        /// <inheritdoc />
        public bool BulkUpdate(IEnumerable<TEntity> instances, Expression<Func<TEntity, object>> propertiesToUpdate)
        {
            return BulkUpdate(instances, propertiesToUpdate, null);
        }

        /// <inheritdoc />
        public bool BulkUpdate(IEnumerable<TEntity> instances, Expression<Func<TEntity, object>> propertiesToUpdate, IDbTransaction transaction)
        {
            var queryResult = SqlGenerator.GetBulkUpdate(instances, propertiesToUpdate);
            var result = Connection.Execute(queryResult.GetSql(), queryResult.Param, transaction) > 0;
            return result;
        }

        /// <inheritdoc />
        public Task<bool> BulkUpdateAsync(IEnumerable<TEntity> instances, Expression<Func<TEntity, object>> propertiesToUpdate)
        {
            return BulkUpdateAsync(instances, propertiesToUpdate, null);
        }

        /// <inheritdoc />
        public async Task<bool> BulkUpdateAsync(IEnumerable<TEntity> instances, Expression<Func<TEntity, object>> propertiesToUpdate, IDbTransaction transaction)
        {
            var queryResult = SqlGenerator.GetBulkUpdate(instances, propertiesToUpdate);
            var result = await Connection.ExecuteAsync(queryResult.GetSql(), queryResult.Param, transaction).ConfigureAwait(false) > 0;
            return result;
        }
    }
}