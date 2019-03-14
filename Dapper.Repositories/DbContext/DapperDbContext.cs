using System;
using System.ComponentModel.Design;
using System.Data;
using System.Data.SqlClient;
using Serilog;

namespace Dapper.Repositories.DbContext
{
    /// <inheritdoc />
    public class DapperDbContext : IDapperDbContext
    {
        private bool _closeConnection = true;
        /// <summary>
        ///     DB Connection for internal use
        /// </summary>
        protected readonly IDbConnection InnerConnection;

        /// <summary>
        ///     DB Connection for internal use
        /// </summary>
        protected readonly IDbConnection InnerConnectionWithoutMultipleActiveResultSets;

        /// <summary>
        ///     Constructor
        /// </summary>
        protected DapperDbContext(IDbConnection connection)
        {
            InnerConnection = connection;

            string connStringWithoutMultipleActiveResultSets = connection.ConnectionString.Replace("MultipleActiveResultSets=true",
                "MultipleActiveResultSets=false");
            IDbConnection connectionWithoutMultipleActiveResultSets = new SqlConnection(connStringWithoutMultipleActiveResultSets);
            InnerConnectionWithoutMultipleActiveResultSets = connectionWithoutMultipleActiveResultSets;
        }

        /// <inheritdoc />
        public virtual IDbConnection Connection
        {
            get
            {
                OpenConnection();
                return InnerConnection;
            }
        }

        /// <inheritdoc />
        public virtual IDbConnection ConnectionWithoutMultipleActiveResultSets
        {
            get
            {
                OpenConnectionWithoutMultipleActiveResultSets();
                return InnerConnectionWithoutMultipleActiveResultSets;
            }
        }

        /// <inheritdoc />
        public void OpenConnection()
        {
            try
            {
                if (InnerConnection.State != ConnectionState.Open && InnerConnection.State != ConnectionState.Connecting)
                    InnerConnection.Open();
            }
            catch (Exception e)
            {
                Log.Fatal(e, e.Message);
                throw;
            }
        }

        /// <inheritdoc />
        public void OpenConnectionWithoutMultipleActiveResultSets()
        {
            try
            {
                if (InnerConnectionWithoutMultipleActiveResultSets.State != ConnectionState.Open && InnerConnectionWithoutMultipleActiveResultSets.State != ConnectionState.Connecting)
                    InnerConnectionWithoutMultipleActiveResultSets.Open();
            }
            catch (Exception e)
            {
                Log.Fatal(e, e.Message);
                throw;
            }
        }

        /// <inheritdoc />
        public virtual IDbTransaction BeginTransaction(bool closeConnection = true)
        {
            _closeConnection = closeConnection;
            return Connection.BeginTransaction();
        }



        /// <inheritdoc />
        /// <summary>
        ///     Close DB connection
        /// </summary>
        public void Dispose()
        {
            if (_closeConnection)
            {
                CloseConnection();
            }
        }

        public void CloseConnection()
        {
            if (InnerConnection != null && InnerConnection.State != ConnectionState.Closed)
                InnerConnection.Close();

            if (InnerConnectionWithoutMultipleActiveResultSets != null &&
                InnerConnectionWithoutMultipleActiveResultSets.State != ConnectionState.Closed)
                InnerConnectionWithoutMultipleActiveResultSets.Close();
        }
    }
}