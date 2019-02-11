using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Common;

namespace Dapper.Repositories
{
    /// <summary>
    ///     Base Repository
    /// </summary>
    public partial class DapperRepository<TEntity>
        where TEntity : class
    {
        /// <inheritdoc />
        public virtual (bool, TEntity) Update(TEntity instance)
        {
            return Update(instance, null);
        }

        /// <inheritdoc />
        public virtual (bool,TEntity) Update(TEntity instance, IDbTransaction transaction)
        {
            var sqlQuery = SqlGenerator.GetUpdate(instance);
            if (SqlGenerator.Config.SqlProvider == SqlProvider.PostgreSQL)
            {
                var updatedPostgreSQL = Connection.Execute(sqlQuery.GetSql(), instance, transaction) > 0;
                return (updatedPostgreSQL, null);
            }
            var updated = Connection.Execute(sqlQuery.GetSql().Split(";")[0], instance, transaction) > 0;
            TEntity newEntity = Connection.Query<TEntity>(sqlQuery.GetSql().Split(";")[1], sqlQuery.Param, transaction).FirstOrDefault();
            return (updated, newEntity);
            //var sqlQuery = SqlGenerator.GetUpdate(instance);
            //var updated = Connection.Execute(sqlQuery.GetSql(), instance, transaction) > 0;
            //return updated;
        }

        /// <inheritdoc />
        public virtual Task<(bool, TEntity)> UpdateAsync(TEntity instance)
        {
            return UpdateAsync(instance, null);
        }

        /// <inheritdoc />
        public virtual async Task<(bool, TEntity)> UpdateAsync(TEntity instance, IDbTransaction transaction)
        {
            var queryResult = SqlGenerator.GetUpdate(instance);
            if (SqlGenerator.Config.SqlProvider == SqlProvider.PostgreSQL)
            {
                var updatedP = await Connection.ExecuteAsync(queryResult.GetSql(), instance, transaction) > 0;
                return (updatedP, null);
            }

            var updated = await Connection.ExecuteAsync(queryResult.GetSql().Split(";")[0], instance, transaction) > 0;
            TEntity data =
                (await Connection.QueryAsync<TEntity>(queryResult.GetSql(), queryResult.Param, transaction))
                .FirstOrDefault();
            return (updated, data);

            //var sqlQuery = SqlGenerator.GetUpdate(instance);
            //var updated = await Connection.ExecuteAsync(sqlQuery.GetSql(), instance, transaction) > 0;
            //return updated;
        }

        /// <inheritdoc />
        public virtual (bool, TEntity) Update(Expression<Func<TEntity, bool>> predicate, TEntity instance)
        {
            return Update(predicate, instance, null);
        }

        /// <inheritdoc />
        public virtual (bool, TEntity) Update(Expression<Func<TEntity, bool>> predicate, TEntity instance, IDbTransaction transaction)
        {
            var sqlQuery = SqlGenerator.GetUpdate(predicate, instance);
            if (SqlGenerator.Config.SqlProvider == SqlProvider.PostgreSQL)
            {
                var updatedP = Connection.Execute(sqlQuery.GetSql(), instance, transaction) > 0;
                return (updatedP, null);
            }
            var updated = Connection.Execute(sqlQuery.GetSql().Split(";")[0], instance, transaction) > 0;
            TEntity newEntity = Connection.Query<TEntity>(sqlQuery.GetSql().Split(";")[1], sqlQuery.Param, transaction).FirstOrDefault();
            return (updated, newEntity);


            //var updated = Connection.Execute(sqlQuery.GetSql(), instance, transaction) > 0;
            //return updated;
        }

        /// <inheritdoc />
        public virtual Task<(bool, TEntity)> UpdateAsync(Expression<Func<TEntity, bool>> predicate, TEntity instance)
        {
            return UpdateAsync(predicate, instance, null);
        }

        /// <inheritdoc />
        public virtual async Task<(bool, TEntity)> UpdateAsync(Expression<Func<TEntity, bool>> predicate, TEntity instance, IDbTransaction transaction)
        {
            var sqlQuery = SqlGenerator.GetUpdate(predicate, instance);
            var queryResult = SqlGenerator.GetUpdate(instance);
            if (SqlGenerator.Config.SqlProvider == SqlProvider.PostgreSQL)
            {
                var updatedP = await Connection.ExecuteAsync(queryResult.GetSql(), instance, transaction) > 0;
                return (updatedP, null);
            }

            var updated = await Connection.ExecuteAsync(queryResult.GetSql().Split(";")[0], instance, transaction) > 0;
            TEntity data =
                (await Connection.QueryAsync<TEntity>(queryResult.GetSql(), queryResult.Param, transaction))
                .FirstOrDefault();
            return (updated, data);

            //var sqlQuery = SqlGenerator.GetUpdate(predicate, instance);
            //var updated = await Connection.ExecuteAsync(sqlQuery.GetSql(), instance, transaction) > 0;
            //return updated;
        }
    }
}