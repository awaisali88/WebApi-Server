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
        public IEnumerable<TEntity> FindAllBetween(object from, object to, Expression<Func<TEntity, object>> btwField, bool includeLogicalDeleted)
        {
            return FindAllBetween(from, to, btwField, includeLogicalDeleted, transaction: null);
        }
        
        /// <inheritdoc />
        public IEnumerable<TEntity> FindAllBetween(object from, object to, Expression<Func<TEntity, object>> btwField, bool includeLogicalDeleted, IDbTransaction transaction)
        {
            return FindAllBetween(from, to, btwField, null, includeLogicalDeleted, transaction);
        }

        /// <inheritdoc />
        public IEnumerable<TEntity> FindAllBetween(DateTime from, DateTime to, Expression<Func<TEntity, object>> btwField, bool includeLogicalDeleted)
        {
            return FindAllBetween(from, to, btwField, includeLogicalDeleted, transaction: null);
        }
        
        /// <inheritdoc />
        public IEnumerable<TEntity> FindAllBetween(DateTime from, DateTime to, Expression<Func<TEntity, object>> btwField, bool includeLogicalDeleted, IDbTransaction transaction)
        {
            return FindAllBetween(from, to, btwField, null, includeLogicalDeleted, transaction);
        }

        /// <inheritdoc />
        public IEnumerable<TEntity> FindAllBetween(DateTime from, DateTime to, Expression<Func<TEntity, object>> btwField, Expression<Func<TEntity, bool>> predicate, bool includeLogicalDeleted)
        {
            return FindAllBetween(from, to, btwField, predicate, includeLogicalDeleted, null);
        }

        /// <inheritdoc />
        public IEnumerable<TEntity> FindAllBetween(
            DateTime from, 
            DateTime to, 
            Expression<Func<TEntity, object>> btwField, 
            Expression<Func<TEntity, bool>> predicate,
            bool includeLogicalDeleted, IDbTransaction transaction)
        {
            var fromString = from.ToString(_dateTimeFormat);
            var toString = to.ToString(_dateTimeFormat);
            return FindAllBetween(fromString, toString, btwField, predicate, includeLogicalDeleted, transaction);
        }

        /// <inheritdoc />
        public IEnumerable<TEntity> FindAllBetween(object from, object to, Expression<Func<TEntity, object>> btwField, Expression<Func<TEntity, bool>> predicate, bool includeLogicalDeleted)
        {
            return FindAllBetween(from, to, btwField, predicate, includeLogicalDeleted, null);
        }

        /// <inheritdoc />
        public IEnumerable<TEntity> FindAllBetween(
            object from, 
            object to, 
            Expression<Func<TEntity, object>> btwField, 
            Expression<Func<TEntity, bool>> predicate,
            bool includeLogicalDeleted, IDbTransaction transaction)
        {
            var queryResult = SqlGenerator.GetSelectBetween(from, to, btwField, includeLogicalDeleted, predicate);
            return Connection.Query<TEntity>(queryResult.GetSql(), queryResult.Param, transaction);
        }

        /// <inheritdoc />
        public Task<IEnumerable<TEntity>> FindAllBetweenAsync(object from, object to, Expression<Func<TEntity, object>> btwField, bool includeLogicalDeleted)
        {
            return FindAllBetweenAsync(from, to, btwField, includeLogicalDeleted, transaction: null);
        }
        
        /// <inheritdoc />
        public Task<IEnumerable<TEntity>> FindAllBetweenAsync(object from, object to, Expression<Func<TEntity, object>> btwField, bool includeLogicalDeleted, IDbTransaction transaction)
        {
            return FindAllBetweenAsync(from, to, btwField, null, includeLogicalDeleted, transaction);
        }
        
        /// <inheritdoc />
        public Task<IEnumerable<TEntity>> FindAllBetweenAsync(DateTime from, DateTime to, Expression<Func<TEntity, object>> btwField, bool includeLogicalDeleted)
        {
            return FindAllBetweenAsync(from, to, btwField, includeLogicalDeleted, transaction: null);
        }
        
        /// <inheritdoc />
        public Task<IEnumerable<TEntity>> FindAllBetweenAsync(DateTime from, DateTime to, Expression<Func<TEntity, object>> btwField, bool includeLogicalDeleted, IDbTransaction transaction)
        {
            return FindAllBetweenAsync(from, to, btwField, null, includeLogicalDeleted, transaction);
        }

        /// <inheritdoc />
        public Task<IEnumerable<TEntity>> FindAllBetweenAsync(DateTime from, DateTime to, Expression<Func<TEntity, object>> btwField, Expression<Func<TEntity, bool>> predicate, bool includeLogicalDeleted)
        {
            return FindAllBetweenAsync(from, to, btwField, predicate, includeLogicalDeleted, null);
        }

        /// <inheritdoc />
        public Task<IEnumerable<TEntity>> FindAllBetweenAsync(object from, object to, Expression<Func<TEntity, object>> btwField, Expression<Func<TEntity, bool>> predicate, bool includeLogicalDeleted)
        {
            return FindAllBetweenAsync(from, to, btwField, predicate, includeLogicalDeleted, null);
        }
        
        /// <inheritdoc />
        public Task<IEnumerable<TEntity>> FindAllBetweenAsync(
            DateTime from,
            DateTime to, 
            Expression<Func<TEntity, object>> btwField, 
            Expression<Func<TEntity, bool>> predicate,
            bool includeLogicalDeleted, IDbTransaction transaction)
        {
            return FindAllBetweenAsync(from.ToString(_dateTimeFormat), to.ToString(_dateTimeFormat), btwField, predicate, includeLogicalDeleted, transaction);
        }

        /// <inheritdoc />
        public Task<IEnumerable<TEntity>> FindAllBetweenAsync(
            object from, 
            object to,
            Expression<Func<TEntity, object>> btwField, 
            Expression<Func<TEntity, bool>> predicate,
            bool includeLogicalDeleted, IDbTransaction transaction)
        {
            var queryResult = SqlGenerator.GetSelectBetween(from, to, btwField, includeLogicalDeleted, predicate);
            return Connection.QueryAsync<TEntity>(queryResult.GetSql(), queryResult.Param, transaction);
        }
    }
}
