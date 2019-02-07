using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Common.Exception;
using Common.Messages;
using Dapper.Identity.Stores;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using WebAPI_BAL.IdentityManager;
using WebAPI_ViewModel.Identity;

namespace WebAPI_BAL.AuthLogic
{
    public interface IAuthorization
    {
        Task<bool> CreateRole(ClaimsPrincipal claim, ApplicationRoleViewModel data);
        Task<bool> UpdateRole(ClaimsPrincipal claim, ApplicationRoleViewModel data);
        Task<bool> RemoveRole(ClaimsPrincipal claim, ApplicationRoleViewModel data);
        Task<bool> AssignRole(ClaimsPrincipal claim, string userId, List<string> selectedRoles);
        Task<bool> UserIsInRole(string userId, string roleId);
        Task<bool> UserIsInRole(ClaimsPrincipal claim, string roleName);
        Task<bool> RemoveFromRole(ClaimsPrincipal claim, string userId, List<string> selectedRoles);
    }

    public class Authorization: IAuthorization
    {
        private readonly ApplicationRoleManager _roleManager;
        private readonly ApplicationUserManager _userManager;
        private readonly RoleStore _roleStore;
        private readonly IMapper _mapper;
        private readonly ILogger<Authorization> _logger;

        public Authorization(ApplicationRoleManager roleManager, ApplicationUserManager userManager, RoleStore roleStore, IMapper mapper, ILogger<Authorization> logger)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _roleStore = roleStore;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<bool> CreateRole(ClaimsPrincipal claim, ApplicationRoleViewModel data)
        {
            data.CreatedBy = ExtBusinessLogic.UserValue(claim);
            data.ModifiedBy = ExtBusinessLogic.UserValue(claim);
            ApplicationRole roleData = _mapper.Map<ApplicationRole>(data);
            bool roleExist = await _roleManager.RoleExistsAsync(roleData.Name);
            if (roleExist)
                throw new WebApiApplicationException(StatusCodes.Status409Conflict, ErrorMessages.RoleAlreadyExist);

            roleData.GenerateNewId();
            var result = await _roleManager.CreateAsync(roleData);
            if (!result.Succeeded)
                throw new WebApiApplicationException(StatusCodes.Status400BadRequest, ErrorMessages.CommonErrorMessage, result.Errors.ToList());

            return true;
        }

        public async Task<bool> UpdateRole(ClaimsPrincipal claim, ApplicationRoleViewModel data)
        {
            data.ModifiedBy = ExtBusinessLogic.UserValue(claim);
            ApplicationRole roleData = _mapper.Map<ApplicationRole>(data);

            var result = await _roleManager.UpdateAsync(roleData);
            if (!result.Succeeded)
                throw new WebApiApplicationException(StatusCodes.Status400BadRequest, ErrorMessages.CommonErrorMessage, result.Errors.ToList());

            return true;
        }

        public async Task<bool> RemoveRole(ClaimsPrincipal claim, ApplicationRoleViewModel data)
        {
            ApplicationRole roleData = _roleManager.Roles.FirstOrDefault(x => x.Id == data.Id);
            ExtBusinessLogic.CheckRecord(roleData);

            // ReSharper disable once PossibleNullReferenceException
            roleData.ModifiedBy = ExtBusinessLogic.UserValue(claim);
            var result = await _roleManager.DeleteAsync(roleData);
            if (!result.Succeeded)
                throw new WebApiApplicationException(StatusCodes.Status400BadRequest, ErrorMessages.CommonErrorMessage, result.Errors.ToList());

            return true;
        }

        public async Task<bool> AssignRole(ClaimsPrincipal claim, string userId, List<string> selectedRoles)
        {
            if (!selectedRoles.Any())
                throw new WebApiApplicationException(StatusCodes.Status400BadRequest, ErrorMessages.EmptyData);

            List<string> roleNames = _roleManager.Roles.Where(x => selectedRoles.Contains(x.Id) && x.Status).Select(x => x.Name).ToList();
            if (!roleNames.Any())
                throw new WebApiApplicationException(StatusCodes.Status404NotFound, ErrorMessages.RecordNotFound);

            ApplicationUser user =
                await Task.Run(() => _userManager.Users.FirstOrDefault(x => x.Id == userId));
            ExtBusinessLogic.CheckRecord(user);
            user.ModifiedBy = ExtBusinessLogic.UserValue(claim);
            var result = await _userManager.AddToRolesAsync(user, roleNames);
            if (!result.Succeeded)
                throw new WebApiApplicationException(StatusCodes.Status400BadRequest, ErrorMessages.CommonErrorMessage,
                    result.Errors.ToList());

            return true;
        }

        public async Task<bool> UserIsInRole(string userId, string roleId)
        {
            ApplicationRole role = await Task.Run(() => _roleManager.Roles.FirstOrDefault(x => x.Id == roleId));
            ExtBusinessLogic.CheckRecord(role);
            ApplicationUser user =
                await Task.Run(() => _userManager.Users.FirstOrDefault(x => x.Id == userId));
            ExtBusinessLogic.CheckRecord(user);

            return await _userManager.IsInRoleAsync(user, role.Name);
        }

        public async Task<bool> UserIsInRole(ClaimsPrincipal claim, string roleName)
        {
            ApplicationRole role = await Task.Run(() => _roleManager.Roles.FirstOrDefault(x => x.Name == roleName));
            ExtBusinessLogic.CheckRecord(role);
            ApplicationUser user =
                await Task.Run(() => _userManager.Users.FirstOrDefault(x => x.Id == ExtBusinessLogic.UserValue(claim, nameof(ApplicationUser.Id))));
            ExtBusinessLogic.CheckRecord(user);

            return await _userManager.IsInRoleAsync(user, role.Name);
        }

        public async Task<bool> RemoveFromRole(ClaimsPrincipal claim, string userId, List<string> selectedRoles)
        {
            if (!selectedRoles.Any())
                throw new WebApiApplicationException(StatusCodes.Status400BadRequest, ErrorMessages.EmptyData);

            List<string> roleNames = _roleManager.Roles.Where(x => selectedRoles.Contains(x.Id) && x.Status).Select(x => x.Name).ToList();
            if (!roleNames.Any())
                throw new WebApiApplicationException(StatusCodes.Status404NotFound, ErrorMessages.RecordNotFound);

            ApplicationUser user =
                await Task.Run(() => _userManager.Users.FirstOrDefault(x => x.Id == userId));
            ExtBusinessLogic.CheckRecord(user);
            user.ModifiedBy = ExtBusinessLogic.UserValue(claim);

            var result = await _userManager.RemoveFromRolesAsync(user, roleNames);
            if (!result.Succeeded)
                throw new WebApiApplicationException(StatusCodes.Status400BadRequest, ErrorMessages.CommonErrorMessage, result.Errors.ToList());

            return true;
        }
    }
}
