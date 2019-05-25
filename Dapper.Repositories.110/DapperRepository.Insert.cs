using System;
using System.Data;
using System.Linq;
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
                object entityData = instance.ToDictionary();

                //var insertData = Connection.Query<TEntity>(queryResult.GetSql(), queryResult.Param, transaction).FirstOrDefault();
                var insertData = Connection.Query<TEntity>(queryResult.GetSql(), entityData, transaction).FirstOrDefault();
                return (true, insertData);
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
                object entityData = instance.ToDictionary();
                //var insertData = (await Connection.QueryAsync<TEntity>(queryResult.GetSql(), queryResult.Param, transaction)).FirstOrDefault();
                var insertData = (await Connection.QueryAsync<TEntity>(queryResult.GetSql(), entityData, transaction)).FirstOrDefault();
                return (true, insertData);
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