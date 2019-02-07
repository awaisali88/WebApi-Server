using System.Data;
using Common;
using Dapper.Repositories.SqlGenerator;

namespace Dapper.Repositories
{
    /// <summary>
    ///     Base Repository
    /// </summary>
    public partial class DapperRepository<TEntity> : IDapperRepository<TEntity>
        where TEntity : class
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        public DapperRepository(IDbConnection connection, bool logQuery = false)
        {
            Connection = connection;
            SqlGenerator = new SqlGenerator<TEntity>(SqlProvider.MSSQL, logQuery: logQuery);
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        public DapperRepository(IDbConnection connection, SqlProvider sqlProvider, bool logQuery = false)
        {
            Connection = connection;
            SqlGenerator = new SqlGenerator<TEntity>(sqlProvider, logQuery:logQuery);
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        public DapperRepository(IDbConnection connection, ISqlGenerator<TEntity> sqlGenerator)
        {
            Connection = connection;
            SqlGenerator = sqlGenerator;
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        public DapperRepository(IDbConnection connection, SqlGeneratorConfig config)
        {
            Connection = connection;
            SqlGenerator = new SqlGenerator<TEntity>(config);
        }

        /// <inheritdoc />
        public IDbConnection Connection { get; }

        /// <inheritdoc />
        public ISqlGenerator<TEntity> SqlGenerator { get; }
        
    }
}