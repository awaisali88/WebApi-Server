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
        public virtual (IEnumerable<TEntity>, int) FindAll(int pageNo, int pageSize, bool includeLogicalDeleted, IDbTransaction transaction = null)
        {
            return FindAll(null, pageNo, pageSize, includeLogicalDeleted, transaction);
        }

        /// <inheritdoc />
        public virtual (IEnumerable<TEntity>, int) FindAll(Expression<Func<TEntity, bool>> predicate, int pageNo, int pageSize, bool includeLogicalDeleted, IDbTransaction transaction = null)
        {
            var queryResult = SqlGenerator.GetSelectAll(predicate, pageNo, pageSize, includeLogicalDeleted);
            if (!queryResult.GetSql().Contains(";"))
            {
                return (Connection.Query<TEntity>(queryResult.GetSql(), queryResult.Param, transaction), 0);
            }

            var totalPages = Connection.QueryFirstOrDefault<int>(queryResult.GetSql().Split(";")[1], queryResult.Param, transaction);
            return (Connection.Query<TEntity>(queryResult.GetSql().Split(";")[0], queryResult.Param, transaction), totalPages);
        }

        /// <inheritdoc />
        public virtual Task<(IEnumerable<TEntity>, int)> FindAllAsync(int pageNo, int pageSize, bool includeLogicalDeleted, IDbTransaction transaction = null)
        {
            return FindAllAsync(null, pageNo, pageSize, includeLogicalDeleted, transaction);
        }

        /// <inheritdoc />
        public virtual async Task<(IEnumerable<TEntity>, int)> FindAllAsync(Expression<Func<TEntity, bool>> predicate, int pageNo, int pageSize, bool includeLogicalDeleted, IDbTransaction transaction = null)
        {
            var queryResult = SqlGenerator.GetSelectAll(predicate, pageNo, pageSize, includeLogicalDeleted);
            if (!queryResult.GetSql().Contains(";"))
                return (await Connection.QueryAsync<TEntity>(queryResult.GetSql(), queryResult.Param, transaction), 0);

            var data = await Connection.QueryAsync<TEntity>(queryResult.GetSql().Split(";")[0], queryResult.Param, transaction);
            var tPages = await Connection.QueryFirstOrDefaultAsync<int>(queryResult.GetSql().Split(";")[1], queryResult.Param, transaction);
            return (data, tPages);
        }
    }
}
