using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Dapper.Identity.Stores;

namespace Dapper.Identity.Tables
{
    internal class UserClaimsTable
    {
        private readonly IDatabaseConnectionFactory _databaseConnectionFactory;

        public UserClaimsTable(IDatabaseConnectionFactory databaseConnectionFactory) => _databaseConnectionFactory = databaseConnectionFactory;

        public async Task<IList<Claim>> GetClaimsAsync(ApplicationUser user) {
            const string command = "SELECT * " +
                                   "FROM dbo.AppUserClaims " +
                                   "WHERE UserId = @UserId;";

            using (var sqlConnection = await _databaseConnectionFactory.CreateConnectionAsync()) {
                return (
                    await sqlConnection.QueryAsync<ApplicationUserClaim>(command, new { UserId = user.Id })
                )
                .Select(e => new Claim(e.ClaimType, e.ClaimValue))
                .ToList(); ;
            }
        }
    }
}
