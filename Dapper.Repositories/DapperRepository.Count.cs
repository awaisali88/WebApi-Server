using System;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Dapper.Repositories
{
    /// <inheritdoc />
    /// <summary>
    ///     Base Repository
    /// </summary>
    public partial class DapperRepository<TEntity>
        where TEntity : class
    {
        /// <inheritdoc />
        public virtual int Count(bool includeLogicalDeleted)
        {
            return Count(includeLogicalDeleted, transaction: null);
        }

        /// <inheritdoc />
        public virtual int Count(bool includeLogicalDeleted, IDbTransaction transaction)
        {
            return Count(null, includeLogicalDeleted, transaction);
        }

        /// <inheritdoc />
        public virtual int Count(Expression<Func<TEntity, bool>> predicate, bool includeLogicalDeleted)
        {
            return Count(predicate, includeLogicalDeleted, transaction: null);
        }

        /// <inheritdoc />
        public virtual int Count(Expression<Func<TEntity, bool>> predicate, bool includeLogicalDeleted, IDbTransaction transaction)
        {
            var queryResult = SqlGenerator.GetCount(predicate, includeLogicalDeleted);
            return Connection.QueryFirstOrDefault<int>(queryResult.GetSql(), queryResult.Param, transaction);
        }

        /// <inheritdoc />
        public virtual int Count(Expression<Func<TEntity, object>> distinctField, bool includeLogicalDeleted)
        {
            return Count(distinctField, includeLogicalDeleted, null);
        }

        /// <inheritdoc />
        public virtual int Count(Expression<Func<TEntity, object>> distinctField, bool includeLogicalDeleted, IDbTransaction transaction)
        {
            return Count(null, distinctField, includeLogicalDeleted, transaction);
        }

        /// <inheritdoc />
        public virtual int Count(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> distinctField, bool includeLogicalDeleted)
        {
            return Count(predicate, distinctField, includeLogicalDeleted, null);
        }

        /// <inheritdoc />
        public virtual int Count(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> distinctField, bool includeLogicalDeleted, IDbTransaction transaction)
        {
            var queryResult = SqlGenerator.GetCount(predicate, distinctField, includeLogicalDeleted);
            return Connection.QueryFirstOrDefault<int>(queryResult.GetSql(), queryResult.Param, transaction);
        }

        /// <inheritdoc />
        public virtual Task<int> CountAsync(bool includeLogicalDeleted)
        {
            return CountAsync(includeLogicalDeleted, transaction: null);
        }
        
        /// <inheritdoc />
        public virtual Task<int> CountAsync(bool includeLogicalDeleted, IDbTransaction transaction)
        {
            return CountAsync(null, includeLogicalDeleted, transaction);
        }

        /// <inheritdoc />
        public virtual Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, bool includeLogicalDeleted)
        {
            return CountAsync(predicate, includeLogicalDeleted, transaction: null);
        }

        /// <inheritdoc />
        public virtual Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, bool includeLogicalDeleted, IDbTransaction transaction)
        {
            var queryResult = SqlGenerator.GetCount(predicate, includeLogicalDeleted);
            return Connection.QueryFirstOrDefaultAsync<int>(queryResult.GetSql(), queryResult.Param, transaction);
        }

        /// <inheritdoc />
        public virtual Task<int> CountAsync(Expression<Func<TEntity, object>> distinctField, bool includeLogicalDeleted)
        {
            return CountAsync(distinctField, includeLogicalDeleted, null);
        }

        /// <inheritdoc />
        public virtual Task<int> CountAsync(Expression<Func<TEntity, object>> distinctField, bool includeLogicalDeleted, IDbTransaction transaction)
        {
            return CountAsync(null, distinctField, includeLogicalDeleted, transaction);
        }

        /// <inheritdoc />
        public virtual Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> distinctField, bool includeLogicalDeleted)
        {
            return CountAsync(predicate, distinctField, includeLogicalDeleted, null);
        }

        /// <inheritdoc />
        public virtual Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> distinctField, bool includeLogicalDeleted, IDbTransaction transaction)
        {
            var queryResult = SqlGenerator.GetCount(predicate, distinctField, includeLogicalDeleted);
            return Connection.QueryFirstOrDefaultAsync<int>(queryResult.GetSql(), queryResult.Param, transaction);
        }
    }
}