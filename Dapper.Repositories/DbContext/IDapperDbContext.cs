using System;
using System.Data;

namespace Dapper.Repositories.DbContext
{
    /// <summary>
    ///     Class is helper for use and close IDbConnection
    /// </summary>
    public interface IDapperDbContext : IDisposable
    {
        IDapperRepository<TModel> GetRepository<TModel>(bool defaultConnection = true) where TModel : class;
        IDapperSProcRepository GetSpRepository(bool defaultConnection = true);

        /// <summary>
        ///     Get opened DB Connection
        /// </summary>
        IDbConnection Connection { get; }

        /// <summary>
        ///     Open DB connection
        /// </summary>
        void OpenConnection();

        /// <summary>
        ///     Get opened DB Connection
        /// </summary>
        IDbConnection ConnectionWithoutMultipleActiveResultSets { get; }

        /// <summary>
        ///     Open DB connection
        /// </summary>
        void OpenConnectionWithoutMultipleActiveResultSets();

        /// <summary>
        ///     Close DB connection
        /// </summary>
        void CloseConnection();

        /// <summary>
        ///     Open DB connection and Begin transaction
        /// </summary>
        IDbTransaction BeginTransaction(bool closeConnection = true);
    }
}