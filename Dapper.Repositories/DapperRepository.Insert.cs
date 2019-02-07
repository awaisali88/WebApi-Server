using System;
using System.Data;
using System.Linq;
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
        public virtual (bool, TEntity) Insert(TEntity instance)
        {
            return Insert(instance, null);
        }

        /// <inheritdoc />
        public virtual (bool, TEntity) Insert(TEntity instance, IDbTransaction transaction)
        {
            var queryResult = SqlGenerator.GetInsert(instance);
            if (SqlGenerator.IsIdentity)
            {
                if (SqlGenerator.Config.SqlProvider == SqlProvider.PostgreSQL)
                {
                    var newId = Connection.Query<long>(queryResult.GetSql(), queryResult.Param, transaction).FirstOrDefault();
                    return (SetValue(newId, instance), null);
                }
                TEntity newEntity = Connection.Query<TEntity>(queryResult.GetSql(), queryResult.Param, transaction).FirstOrDefault();
                return (true, newEntity);
            }
            return (Connection.Execute(queryResult.GetSql(), instance, transaction) > 0, null);
        }

        /// <inheritdoc />
        public virtual Task<(bool, TEntity)> InsertAsync(TEntity instance)
        {
            return InsertAsync(instance, null);
        }

        /// <inheritdoc />
        public virtual async Task<(bool, TEntity)> InsertAsync(TEntity instance, IDbTransaction transaction)
        {
            var queryResult = SqlGenerator.GetInsert(instance);
            if (SqlGenerator.IsIdentity)
            {
                if (SqlGenerator.Config.SqlProvider == SqlProvider.PostgreSQL)
                {
                    var newId = (await Connection.QueryAsync<long>(queryResult.GetSql(), queryResult.Param, transaction)).FirstOrDefault();
                    return (SetValue(newId, instance), null);
                }
                TEntity data =
                    (await Connection.QueryAsync<TEntity>(queryResult.GetSql(), queryResult.Param, transaction))
                    .FirstOrDefault();
                return (true, data);
            }

            return (await Connection.ExecuteAsync(queryResult.GetSql(), instance, transaction) > 0, null);
        }

        private bool SetValue(long newId, TEntity instance)
        {
            var added = newId > 0;
            if (added)
            {
                var newParsedId = Convert.ChangeType(newId, SqlGenerator.IdentitySqlProperty.PropertyInfo.PropertyType);
                SqlGenerator.IdentitySqlProperty.PropertyInfo.SetValue(instance, newParsedId);
            }
            return added;
        }
    }
}