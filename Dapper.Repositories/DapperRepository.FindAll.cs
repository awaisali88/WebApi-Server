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
        public virtual IEnumerable<TEntity> FindAll(bool includeLogicalDeleted, IDbTransaction transaction = null)
        {
            return FindAll(null, includeLogicalDeleted, transaction);
        }

        /// <inheritdoc />
        public virtual IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate, bool includeLogicalDeleted, IDbTransaction transaction = null)
        {
            var queryResult = SqlGenerator.GetSelectAll(predicate, includeLogicalDeleted);
            return Connection.Query<TEntity>(queryResult.GetSql(), queryResult.Param, transaction);
        }
        
        /// <inheritdoc />
        public virtual Task<IEnumerable<TEntity>> FindAllAsync(bool includeLogicalDeleted, IDbTransaction transaction = null)
        {
            return FindAllAsync(null, includeLogicalDeleted, transaction);
        }

        /// <inheritdoc />
        public virtual Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate, bool includeLogicalDeleted, IDbTransaction transaction = null)
        {
            var queryResult = SqlGenerator.GetSelectAll(predicate, includeLogicalDeleted);
            return Connection.QueryAsync<TEntity>(queryResult.GetSql(), queryResult.Param, transaction);
        }
    }
}
