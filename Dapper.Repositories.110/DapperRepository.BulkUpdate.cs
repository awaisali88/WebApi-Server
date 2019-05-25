using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Common;
using Dapper.Repositories.SqlGenerator;

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
            if (SqlGenerator.Config.SqlProvider == SqlProvider.MSSQL)
            {
                int count = 0;
                int totalInstances = instances.Count();

                var properties = SqlGenerator.SqlProperties.ToList();

                int exceededTimes = (int)Math.Ceiling(totalInstances * properties.Count / 2100d);
                if (exceededTimes > 1)
                {
                    int maxAllowedInstancesPerBatch = totalInstances / exceededTimes;

                    for (int i = 0; i <= exceededTimes; i++)
                    {
                        var items = instances.Skip(i * maxAllowedInstancesPerBatch).Take(maxAllowedInstancesPerBatch);
                        var msSqlQueryResult = SqlGenerator.GetBulkUpdate(items, propertiesToUpdate);
                        count += Connection.Execute(msSqlQueryResult.GetSql(), msSqlQueryResult.Param, transaction);
                    }
                    return count > 0;
                }
            }
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
            if (SqlGenerator.Config.SqlProvider == SqlProvider.MSSQL)
            {
                int count = 0;
                int totalInstances = instances.Count();

                var properties = SqlGenerator.SqlProperties.ToList();

                int exceededTimes = (int)Math.Ceiling(totalInstances * properties.Count / 2100d);
                if (exceededTimes > 1)
                {
                    int maxAllowedInstancesPerBatch = totalInstances / exceededTimes;

                    for (int i = 0; i <= exceededTimes; i++)
                    {
                        var items = instances.Skip(i * maxAllowedInstancesPerBatch).Take(maxAllowedInstancesPerBatch);
                        var msSqlQueryResult = SqlGenerator.GetBulkUpdate(items, propertiesToUpdate);
                        count += await Connection.ExecuteAsync(msSqlQueryResult.GetSql(), msSqlQueryResult.Param, transaction);
                    }
                    return count > 0;
                }
            }
            var queryResult = SqlGenerator.GetBulkUpdate(instances, propertiesToUpdate);
            var result = await Connection.ExecuteAsync(queryResult.GetSql(), queryResult.Param, transaction) > 0;
            return result;
        }
    }
}