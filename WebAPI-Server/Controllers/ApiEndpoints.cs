using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI_Server.Controllers
{
#pragma warning disable 1591
    public class ApiEndpoints
    {
        #region Account Controller
        public const string AccountControllerPrefix = "api/auth";
        public const string Register = "register";
        public const string AddUser = "add-user";
        public const string RemoveUser = "remove-user";
        public const string Facebook = "facebook";
        public const string Authenticate = "authenticate";
        public const string SetPassword = "set-password";
        public const string ChangePassword = "change-password";
        public const string ResetPasswordToken = "reset-password-token";
        public const string ResetPassword = "reset-password";
        public const string VerifyEmail = "verify-email";
        public const string VerifyResetPassword = "verify-reset-password";
        public const string RefreshToken = "refresh-token";
        public const string LogOff = "logoff";
        public const string AddRole = "add-role";
        public const string UpdateRole = "update-role";
        public const string DeleteRole = "delete-role";
        public const string HasRole = "has-role";
        public const string AssignRole = "assign-role";
        public const string RemoveRole = "remove-role";
        public const string UserRole = "user-role";
        #endregion
    }
#pragma warning restore 1591
}
