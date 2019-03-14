using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Common;
using Dapper.Repositories.Extensions;

namespace Dapper.Repositories
{
    /// <summary>
    ///     Base Repository
    /// </summary>
    public partial class DapperRepository<TEntity>
        where TEntity : class
    {
        /// <inheritdoc />
        public virtual (bool, TEntity) Update(TEntity instance, Expression<Func<TEntity, object>> propertiesToUpdate)
        {
            return Update(instance, propertiesToUpdate, null);
        }

        /// <inheritdoc />
        public virtual (bool, TEntity) Update(TEntity instance, Expression<Func<TEntity, object>> propertiesToUpdate, IDbTransaction transaction)
        {
            var sqlQuery = SqlGenerator.GetUpdate(instance, propertiesToUpdate);
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
        public virtual Task<(bool, TEntity)> UpdateAsync(TEntity instance, Expression<Func<TEntity, object>> propertiesToUpdate)
        {
            return UpdateAsync(instance, propertiesToUpdate, null);
        }

        /// <inheritdoc />
        public virtual async Task<(bool, TEntity)> UpdateAsync(TEntity instance, Expression<Func<TEntity, object>> propertiesToUpdate, IDbTransaction transaction)
        {
            var queryResult = SqlGenerator.GetUpdate(instance, propertiesToUpdate);
            if (SqlGenerator.Config.SqlProvider == SqlProvider.PostgreSQL)
            {
                var updatedP = await Connection.ExecuteAsync(queryResult.GetSql(), instance, transaction).ConfigureAwait(false) > 0;
                return (updatedP, null);
            }

            var updated = await Connection.ExecuteAsync(queryResult.GetSql().Split(";")[0], instance, transaction).ConfigureAwait(false) > 0;
            TEntity data =
                (await Connection.QueryAsync<TEntity>(queryResult.GetSql().Split(";")[1], queryResult.Param, transaction).ConfigureAwait(false))
                .FirstOrDefault();
            return (updated, data);

            //var sqlQuery = SqlGenerator.GetUpdate(instance);
            //var updated = await Connection.ExecuteAsync(sqlQuery.GetSql(), instance, transaction) > 0;
            //return updated;
        }

        /// <inheritdoc />
        public virtual (bool, IEnumerable<TEntity>) Update(Expression<Func<TEntity, bool>> predicate, TEntity instance, Expression<Func<TEntity, object>> propertiesToUpdate)
        {
            return Update(predicate, instance, propertiesToUpdate, null);
        }

        /// <inheritdoc />
        public virtual (bool, IEnumerable<TEntity>) Update(Expression<Func<TEntity, bool>> predicate, TEntity instance, Expression<Func<TEntity, object>> propertiesToUpdate, IDbTransaction transaction)
        {
            var sqlQuery = SqlGenerator.GetUpdate(predicate, instance, propertiesToUpdate);
            if (SqlGenerator.Config.SqlProvider == SqlProvider.PostgreSQL)
            {
                var updatedP = Connection.Execute(sqlQuery.GetSql(), instance, transaction) > 0;
                return (updatedP, null);
            }

            object sqlGeneratedParam;
            if (sqlQuery.Param is Dictionary<string, object> qParams)
            {
                var entityData = instance.ToDictionary();
                foreach (var qParam in qParams)
                    entityData.Add(qParam.Key, qParam.Value);
                sqlGeneratedParam = entityData;
            }
            else
                sqlGeneratedParam = instance;
            //
            //object paramResult = TypeMerger.Merge(instance, sqlGeneratedParam);
            //object paramResult = _mapper.Map(instance, sqlGeneratedParam);

            var updated = Connection.Execute(sqlQuery.GetSql().Split(";")[0], sqlGeneratedParam, transaction) > 0;
            IEnumerable<TEntity> newEntity = Connection.Query<TEntity>(sqlQuery.GetSql().Split(";")[1], sqlGeneratedParam, transaction);
            return (updated, newEntity);


            //var updated = Connection.Execute(sqlQuery.GetSql(), instance, transaction) > 0;
            //return updated;
        }

        /// <inheritdoc />
        public virtual Task<(bool, IEnumerable<TEntity>)> UpdateAsync(Expression<Func<TEntity, bool>> predicate, TEntity instance, Expression<Func<TEntity, object>> propertiesToUpdate)
        {
            return UpdateAsync(predicate, instance, propertiesToUpdate, null);
        }

        /// <inheritdoc />
        public virtual async Task<(bool, IEnumerable<TEntity>)> UpdateAsync(Expression<Func<TEntity, bool>> predicate, TEntity instance, Expression<Func<TEntity, object>> propertiesToUpdate, IDbTransaction transaction)
        {
            var queryResult = SqlGenerator.GetUpdate(predicate, instance, propertiesToUpdate);
            if (SqlGenerator.Config.SqlProvider == SqlProvider.PostgreSQL)
            {
                var updatedP = await Connection.ExecuteAsync(queryResult.GetSql(), instance, transaction).ConfigureAwait(false) > 0;
                return (updatedP, null);
            }

            object sqlGeneratedParam;
            if (queryResult.Param is Dictionary<string, object> qParams)
            {
                var entityData = instance.ToDictionary();
                foreach (var qParam in qParams)
                    entityData.Add(qParam.Key, qParam.Value);
                sqlGeneratedParam = entityData;
            }
            else
                sqlGeneratedParam = instance;

            var updated = await Connection.ExecuteAsync(queryResult.GetSql().Split(";")[0], sqlGeneratedParam, transaction).ConfigureAwait(false) > 0;
            IEnumerable<TEntity> data =
                (await Connection.QueryAsync<TEntity>(queryResult.GetSql().Split(";")[1], sqlGeneratedParam, transaction).ConfigureAwait(false));
            return (updated, data);

            //var sqlQuery = SqlGenerator.GetUpdate(predicate, instance);
            //var updated = await Connection.ExecuteAsync(sqlQuery.GetSql(), instance, transaction) > 0;
            //return updated;
        }
    }
}