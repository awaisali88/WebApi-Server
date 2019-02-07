using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Dapper.Identity.Stores;

namespace Dapper.Identity.Tables
{
    internal class RoleClaimsTable
    {
        private readonly IDatabaseConnectionFactory _databaseConnectionFactory;

        public RoleClaimsTable(IDatabaseConnectionFactory databaseConnectionFactory) => _databaseConnectionFactory = databaseConnectionFactory;

        public async Task<IList<Claim>> GetClaimsAsync(string roleId) {
            const string command = "SELECT * " +
                                   "FROM dbo.AppRoleClaims " +
                                   "WHERE RoleId = @RoleId;";

            IEnumerable<ApplicationRoleClaim> roleClaims = new List<ApplicationRoleClaim>();

            using (var sqlConnection = await _databaseConnectionFactory.CreateConnectionAsync()) {
                return (
                    await sqlConnection.QueryAsync<ApplicationRoleClaim>(command, new { RoleId = roleId })
                )
                .Select(x => new Claim(x.ClaimType, x.ClaimValue))
                .ToList();
            }
        }
    }
}
