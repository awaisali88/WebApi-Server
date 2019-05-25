using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
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
            var updated = Connection.Execute(sqlQuery.GetSql().Split(";")[0], instance, transaction) > 0;
            TEntity newEntity = Connection.Query<TEntity>(sqlQuery.GetSql().Split(";")[1], sqlQuery.Param, transaction).FirstOrDefault();
            return (updated, newEntity);
        }

        /// <inheritdoc />
        public virtual Task<(bool, TEntity)> UpdateAsync(TEntity instance, Expression<Func<TEntity, object>> propertiesToUpdate)
        {
            return UpdateAsync(instance, propertiesToUpdate, null);
        }

        /// <inheritdoc />
        public virtual async Task<(bool, TEntity)> UpdateAsync(TEntity instance, Expression<Func<TEntity, object>> propertiesToUpdate, IDbTransaction transaction)
        {
            var sqlQuery = SqlGenerator.GetUpdate(instance, propertiesToUpdate);
            var updated = await Connection.ExecuteAsync(sqlQuery.GetSql().Split(";")[0], instance, transaction) > 0;
            TEntity newEntity = Connection.Query<TEntity>(sqlQuery.GetSql().Split(";")[1], sqlQuery.Param, transaction).FirstOrDefault();
            return (updated, newEntity);
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

            object sqlGeneratedParam;
            if (sqlQuery.Param is Dictionary<string, object> qParams)
            {
                var entityData = instance.ToDictionary();
                foreach (var qParam in qParams)
                    if (!entityData.ContainsKey(qParam.Key))
                        entityData.Add(qParam.Key, qParam.Value);
                sqlGeneratedParam = entityData;
            }
            else
                sqlGeneratedParam = instance;

            var updated = Connection.Execute(sqlQuery.GetSql().Split(";")[0], sqlGeneratedParam, transaction) > 0;
            IEnumerable<TEntity> newEntity = Connection.Query<TEntity>(sqlQuery.GetSql().Split(";")[1], sqlGeneratedParam, transaction);
            return (updated, newEntity);
        }

        /// <inheritdoc />
        public virtual Task<(bool, IEnumerable<TEntity>)> UpdateAsync(Expression<Func<TEntity, bool>> predicate, TEntity instance, Expression<Func<TEntity, object>> propertiesToUpdate)
        {
            return UpdateAsync(predicate, instance, propertiesToUpdate, null);
        }

        /// <inheritdoc />
        public virtual async Task<(bool, IEnumerable<TEntity>)> UpdateAsync(Expression<Func<TEntity, bool>> predicate, TEntity instance, Expression<Func<TEntity, object>> propertiesToUpdate, IDbTransaction transaction)
        {
            var sqlQuery = SqlGenerator.GetUpdate(predicate, instance, propertiesToUpdate);

            object sqlGeneratedParam;
            if (sqlQuery.Param is Dictionary<string, object> qParams)
            {
                var entityData = instance.ToDictionary();
                foreach (var qParam in qParams)
                    if (!entityData.ContainsKey(qParam.Key))
                        entityData.Add(qParam.Key, qParam.Value);
                sqlGeneratedParam = entityData;
            }
            else
                sqlGeneratedParam = instance;

            var updated = await Connection.ExecuteAsync(sqlQuery.GetSql().Split(";")[0], sqlGeneratedParam, transaction).ConfigureAwait(false) > 0;
            IEnumerable<TEntity> data =
                (await Connection.QueryAsync<TEntity>(sqlQuery.GetSql().Split(";")[1], sqlGeneratedParam, transaction).ConfigureAwait(false));
            return (updated, data);
        }
    }
}
