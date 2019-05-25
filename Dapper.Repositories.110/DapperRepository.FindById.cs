using System.Data;
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
        public virtual TEntity FindById(object id, bool includeLogicalDeleted)
        {
            return FindById(id, includeLogicalDeleted, null);
        }

        /// <inheritdoc />
        public virtual TEntity FindById(object id, bool includeLogicalDeleted, IDbTransaction transaction)
        {
            var queryResult = SqlGenerator.GetSelectById(id, includeLogicalDeleted);
            return Connection.QuerySingleOrDefault<TEntity>(queryResult.GetSql(), queryResult.Param, transaction);
        }

        /// <inheritdoc />
        public virtual Task<TEntity> FindByIdAsync(object id, bool includeLogicalDeleted)
        {
            return FindByIdAsync(id, includeLogicalDeleted, null);
        }

        /// <inheritdoc />
        public virtual Task<TEntity> FindByIdAsync(object id, bool includeLogicalDeleted, IDbTransaction transaction)
        {
            var queryResult = SqlGenerator.GetSelectById(id, includeLogicalDeleted);
            return Connection.QuerySingleOrDefaultAsync<TEntity>(queryResult.GetSql(), queryResult.Param, transaction);
        }
    }
}
