using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper.Identity.Stores;
using Microsoft.AspNetCore.Identity;

namespace Dapper.Identity.Tables
{
    internal class UserRolesTable
    {
        private readonly IDatabaseConnectionFactory _databaseConnectionFactory;

        public UserRolesTable(IDatabaseConnectionFactory databaseConnectionFactory) => _databaseConnectionFactory = databaseConnectionFactory;

        public async Task<IEnumerable<ApplicationUserRole>> GetRolesAsync(ApplicationUser user) {
            const string command = "SELECT r.Id AS RoleId, r.Name AS RoleName, ur.Status AS Status, ur.RecordStatus AS RecordStatus, ur.Trashed AS Trashed " +
                                   "FROM dbo.AppRoles AS r " +
                                   "INNER JOIN dbo.AppUserRoles AS ur ON ur.RoleId = r.Id " +
                                   "WHERE ur.UserId = @UserId;";

            using (var sqlConnection = await _databaseConnectionFactory.CreateConnectionAsync()) {
                return await sqlConnection.QueryAsync<ApplicationUserRole>(command, new {
                    UserId = user.Id
                });
            }
        }

        public virtual async Task<IdentityResult> UpdateAsync(ApplicationUserRole userRole, string userId)
        {
            // The implementation here might look a little strange, however there is a reason for this.
            // ASP.NET Core Identity stores follow a UOW (Unit of Work) pattern which practically means that when an operation is called it does not necessarily writes to the database.
            // It tracks the changes made and finally commits to the database. UserStore methods just manipulate the user and only CreateAsync, UpdateAsync and DeleteAsync of IUserStore<>
            // write to the database. This makes sense because this way we avoid connection to the database all the time and also we can commit all changes at once by using a transaction.
            const string updateUserRolesCommand =
                "UPDATE dbo.AppUserRoles " +
                "SET Status = @Status, Trashed = @Trashed, " +
                    "ModifiedDate = @ModifiedDate, ModifiedBy = @ModifiedBy, RecordStatus = @RecordStatus " +
                "WHERE UserId = @UserId AND RoleId = @RoleId;";

            using (var sqlConnection = await _databaseConnectionFactory.CreateConnectionAsync())
            {
                using (var transaction = sqlConnection.BeginTransaction())
                {
                    await sqlConnection.ExecuteAsync(updateUserRolesCommand, new
                    {
                        userRole.Status,
                        userRole.Trashed,
                        userRole.ModifiedDate,
                        userRole.ModifiedBy,
                        userRole.RecordStatusCode,
                        userId,
                        userRole.RoleId,
                    }, transaction);
                    
                    try
                    {
                        transaction.Commit();
                    }
                    catch
                    {
                        try
                        {
                            transaction.Rollback();
                        }
                        catch
                        {
                            return IdentityResult.Failed(new IdentityError
                            {
                                Code = nameof(UpdateAsync),
                                Description = $"User Roles could not be updated. Operation could not be rolled back."
                            });
                        }

                        return IdentityResult.Failed(new IdentityError
                        {
                            Code = nameof(UpdateAsync),
                            Description = $"User Roles could not be updated. Operation was rolled back."
                        });
                    }
                }
            }

            return IdentityResult.Success;
        }
    }
}
