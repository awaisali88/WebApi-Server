using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper.Identity.Stores;
using Microsoft.AspNetCore.Identity;

namespace Dapper.Identity.Tables
{
    internal class RolesTable
    {
        private readonly IDatabaseConnectionFactory _databaseConnectionFactory;

        public RolesTable(IDatabaseConnectionFactory databaseConnectionFactory) => _databaseConnectionFactory = databaseConnectionFactory;

        public async Task<IdentityResult> CreateAsync(ApplicationRole role, CancellationToken cancellationToken) {
            const string command = "INSERT INTO dbo.AppRoles (Id, Name, NormalizedName, ConcurrencyStamp, Description, Status, Trashed, CreatedDate, ModifiedDate, CreatedBy, ModifiedBy, RecordStatus)" +
                                   "VALUES (@Id, @Name, @NormalizedName, @ConcurrencyStamp, @Description, @Status, @Trashed, @CreatedDate, @ModifiedDate, @CreatedBy, @ModifiedBy, @RecordStatusCode);";

            int rowsInserted;

            using (var sqlConnection = await _databaseConnectionFactory.CreateConnectionAsync()) {
                rowsInserted = await sqlConnection.ExecuteAsync(command, new {
                    role.Id,
                    role.Name,
                    role.NormalizedName,
                    role.ConcurrencyStamp,
                    role.Description,
                    role.Status,
                    role.Trashed,
                    role.CreatedDate,
                    role.ModifiedDate,
                    role.CreatedBy,
                    role.ModifiedBy,
                    role.RecordStatusCode,
                });
            }

            return rowsInserted == 1 ? IdentityResult.Success : IdentityResult.Failed(new IdentityError {
                Code = string.Empty,
                Description = $"The role with name {role.Name} could not be inserted."
            });
        }

        public async Task<IdentityResult> UpdateAsync(ApplicationRole role) {
            const string command = "UPDATE dbo.AppRoles " +
                                   "SET Name = @Name, NormalizedName = @NormalizedName, ConcurrencyStamp = @ConcurrencyStamp, Description = @Description, " +
                                   "ModifiedBy = @ModifiedBy, ModifiedDate = @ModifiedDate, RecordStatus = @RecordStatusCode, " +
                                   "Status = @Status, Trashed = @Trashed " +
                                   "WHERE Id = @Id;";

            using (var sqlConnection = await _databaseConnectionFactory.CreateConnectionAsync()) {
                using (var transaction = sqlConnection.BeginTransaction()) {
                    await sqlConnection.ExecuteAsync(command, new {
                        role.Name,
                        role.NormalizedName,
                        role.ConcurrencyStamp,
                        role.Description,
                        role.ModifiedBy,
                        role.ModifiedDate,
                        role.RecordStatusCode,
                        role.Status,
                        role.Trashed,
                        role.Id,
                    }, transaction);

                    if (role.Claims.Any()) {
                        const string deleteClaimsCommand = "DELETE " +
                                                           "FROM dbo.AppRoleClaims " +
                                                           "WHERE RoleId = @RoleId;";

                        await sqlConnection.ExecuteAsync(deleteClaimsCommand, new {
                            RoleId = role.Id
                        }, transaction);

                        const string insertClaimsCommand = "INSERT INTO dbo.AppRoleClaims (RoleId, ClaimType, ClaimValue) " +
                                                           "VALUES (RoleId, ClaimType, ClaimValue);";

                        await sqlConnection.ExecuteAsync(insertClaimsCommand, role.Claims.Select(x => new {
                            RoleId = role.Id,
                            ClaimType = x.Type,
                            ClaimValue = x.Value
                        }), transaction);
                    }

                    try {
                        transaction.Commit();
                    } catch {
                        try {
                            transaction.Rollback();
                        } catch {
                            return IdentityResult.Failed(new IdentityError {
                                Code = nameof(UpdateAsync),
                                Description = $"Role with name {role.Name} could not be updated. Operation could not be rolled back."
                            });
                        }

                        return IdentityResult.Failed(new IdentityError {
                            Code = nameof(UpdateAsync),
                            Description = $"Role with name {role.Name} could not be updated.. Operation was rolled back."
                        });
                    }
                }
            }

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(ApplicationRole role) {
            const string command = "DELETE " +
                                   "FROM dbo.AppRoles " +
                                   "WHERE Id = @Id;";

            int rowsDeleted;

            using (var sqlConnection = await _databaseConnectionFactory.CreateConnectionAsync()) {
                rowsDeleted = await sqlConnection.ExecuteAsync(command, new { role.Id });
            }

            return rowsDeleted == 1 ? IdentityResult.Success : IdentityResult.Failed(new IdentityError {
                Code = string.Empty,
                Description = $"The role with name {role.Name} could not be deleted."
            });
        }

        public async Task<ApplicationRole> FindByIdAsync(string roleId) {
            const string command = "SELECT * " +
                                   "FROM dbo.AppRoles " +
                                   "WHERE Id = @Id;";

            using (var sqlConnection = await _databaseConnectionFactory.CreateConnectionAsync()) {
                return await sqlConnection.QuerySingleOrDefaultAsync<ApplicationRole>(command, new {
                    Id = roleId
                });
            }
        }

        public async Task<ApplicationRole> FindByNameAsync(string normalizedRoleName) {
            const string command = "SELECT * " +
                                   "FROM dbo.AppRoles " +
                                   "WHERE NormalizedName = @NormalizedName;";

            using (var sqlConnection = await _databaseConnectionFactory.CreateConnectionAsync()) {
                return await sqlConnection.QuerySingleOrDefaultAsync<ApplicationRole>(command, new {
                    NormalizedName = normalizedRoleName
                });
            }
        }

        public async Task<IEnumerable<ApplicationRole>> GetAllRolesAsync() {
            const string command = "SELECT * " +
                                   "FROM dbo.AppRoles;";

            using (var sqlConnection = await _databaseConnectionFactory.CreateConnectionAsync()) {
                return await sqlConnection.QueryAsync<ApplicationRole>(command);
            }
        }
    }
}
