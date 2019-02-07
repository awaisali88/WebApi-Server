using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper.Identity.Stores;

namespace Dapper.Identity.Tables
{
    internal class UserTokensTable
    {
        private readonly IDatabaseConnectionFactory _databaseConnectionFactory;

        public UserTokensTable(IDatabaseConnectionFactory databaseConnectionFactory) => _databaseConnectionFactory = databaseConnectionFactory;

        public async Task<IEnumerable<ApplicationUserToken>> GetTokensAsync(string userId) {
            const string command = "SELECT * " +
                                   "FROM dbo.AppUserTokens " +
                                   "WHERE UserId = @UserId;";

            using (var sqlConnection = await _databaseConnectionFactory.CreateConnectionAsync()) {
                return await sqlConnection.QueryAsync<ApplicationUserToken>(command, new {
                    UserId = userId
                });
            }
        }
    }
}
