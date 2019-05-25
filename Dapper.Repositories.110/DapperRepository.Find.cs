using System;
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
        public virtual TEntity Find(bool includeLogicalDeleted)
        {
            return Find(null, includeLogicalDeleted, null);
        }

        /// <inheritdoc />
        public virtual TEntity Find(bool includeLogicalDeleted, IDbTransaction transaction)
        {
            return Find(null, includeLogicalDeleted, transaction);
        }

        /// <inheritdoc />
        public virtual TEntity Find(Expression<Func<TEntity, bool>> predicate, bool includeLogicalDeleted)
        {
            return Find(predicate, includeLogicalDeleted, null);
        }

        /// <inheritdoc />
        public virtual TEntity Find(Expression<Func<TEntity, bool>> predicate, bool includeLogicalDeleted, IDbTransaction transaction)
        {
            var queryResult = SqlGenerator.GetSelectFirst(predicate, includeLogicalDeleted);
            return Connection.QueryFirstOrDefault<TEntity>(queryResult.GetSql(), queryResult.Param, transaction);
        }

        /// <inheritdoc />
        public virtual Task<TEntity> FindAsync(bool includeLogicalDeleted)
        {
            return FindAsync(null, includeLogicalDeleted, null);
        }

        /// <inheritdoc />
        public virtual Task<TEntity> FindAsync(bool includeLogicalDeleted, IDbTransaction transaction)
        {
            return FindAsync(null, includeLogicalDeleted, transaction);
        }

        /// <inheritdoc />
        public virtual Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate, bool includeLogicalDeleted)
        {
            return FindAsync(predicate, includeLogicalDeleted, null);
        }

        /// <inheritdoc />
        public virtual Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate, bool includeLogicalDeleted, IDbTransaction transaction)
        {
            var queryResult = SqlGenerator.GetSelectFirst(predicate, includeLogicalDeleted);
            return Connection.QueryFirstOrDefaultAsync<TEntity>(queryResult.GetSql(), queryResult.Param, transaction);
        }
    }
}
