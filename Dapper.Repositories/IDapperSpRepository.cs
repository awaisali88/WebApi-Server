using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper.Repositories.Extensions;

namespace Dapper.Repositories
{
    public interface IDapperSProcRepository
    {
        /// <summary>
        ///     DB Connection
        /// </summary>
        IDbConnection Connection { get; }

        IEnumerable<TSpModel> Execute<TSpModel, TSpParam>(TSpParam spParameters, IDbTransaction transaction = null)
            where TSpParam : class, ISProcParam;

        Task<IEnumerable<TSpModel>> ExecuteAsync<TSpModel, TSpParam>(TSpParam spParameters,
            IDbTransaction transaction = null) where TSpParam : class, ISProcParam;

    }
}
