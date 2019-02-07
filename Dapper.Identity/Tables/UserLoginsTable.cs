using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper.Identity.Stores;
using Microsoft.AspNetCore.Identity;

namespace Dapper.Identity.Tables
{
    internal class UserLoginsTable
    {
        private readonly IDatabaseConnectionFactory _databaseConnectionFactory;

        public UserLoginsTable(IDatabaseConnectionFactory databaseConnectionFactory) => _databaseConnectionFactory = databaseConnectionFactory;

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(ApplicationUser user) {
            const string command = "SELECT * " +
                                   "FROM dbo.AppUserLogins " +
                                   "WHERE UserId = @UserId;";

            using (var sqlConnection = await _databaseConnectionFactory.CreateConnectionAsync()) {
                return (
                    await sqlConnection.QueryAsync<ApplicationUserLogin>(command, new { UserId = user.Id })
                )
                .Select(x => new UserLoginInfo(x.LoginProvider, x.ProviderKey, x.ProviderDisplayName))
                .ToList(); ;
            }
        }

        public async Task<ApplicationUser> FindByLoginAsync(string loginProvider, string providerKey) {
            string[] command =
            {
                "SELECT UserId " +
                "FROM dbo.AppUserLogins " +
                "WHERE LoginProvider = @LoginProvider AND ProviderKey = @ProviderKey;"
            };

            using (var sqlConnection = await _databaseConnectionFactory.CreateConnectionAsync()) {
                var userId = await sqlConnection.QuerySingleOrDefaultAsync<Guid?>(command[0], new {
                    LoginProvider = loginProvider,
                    ProviderKey = providerKey
                });

                if (userId == null) {
                    return null;
                }

                command[0] = "SELECT * " +
                             "FROM dbo.AppUsers " +
                             "WHERE Id = @Id;";

                return await sqlConnection.QuerySingleAsync<ApplicationUser>(command[0], new { Id = userId });
            }
        }
    }
}
