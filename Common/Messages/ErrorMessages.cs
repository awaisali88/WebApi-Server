namespace Common.Messages
{
    public static class ErrorMessages
    {
        public const string CommonErrorMessage = "Something went wrong. Please try again.";

        public const string InActiveRecord = "The provided record is in-active";
        public const string TrashedRecord = "The provided record is deleted";
        public const string RecordNotFound = "The provided record does not exist.";
        public const string RecordNotFoundUpdate = "The record is updated by another user or record does not exist.";

        public const string ModelValidationFailed = "Provided data is invalid.";

        public const string UnAuthorized = "You are not authorized to access this resource";
        public const string Forbidden = "Method not allowed";
        public const string InternalServerError = "Internal Server Error. Please Contact your Administrator.";
        public const string Page404 = "Page not found.";

        public const string InvalidApiKey = "Invalid access token.";
        public const string InvalidRequestUrl = "Invalid Request Url.";
        public const string InvalidUser = "Invalid username or password.";
        public const string EmailNotVerified = "Please verify you email first.";
        public const string ErrorCreatingFbUser = "Error in connecting facebook account. Please try again later.";
        public const string ErrorCreatingLocalUser = "Failed to create user account. Please try again later.";
        public const string InvalidFbToken = "Invalid facebook token.";
        public const string ErrorSetPassword = "Error in setting new password.";
        public const string ErrorEmailConfirmationToken = "Error in generating email token.";
        public const string ErrorUserNotFound = "User does not exist.";
        public const string RoleAlreadyExist = "The provided role already exists.";
        public const string UserNotAllowed = "You are now allowed to login. Please verify your email.";
        public const string UserLockedOut = "You are locked out from the system. Please contact administrator.";
        public const string RequireTwoFactor = "Require 2 factor authentication.";


        public const string TokenExpired = "The provided token is expired";

        //User Already Exist
        public const string UserExistCode = "DuplicateUserName";
        public const string UserExistDescription = "User name '[USERNAME]' is already taken.";

        //Identity Resources
        public const string InvalidPasswordHasherCompatibilityMode = "The provided PasswordHasherCompatibilityMode is invalid.";
        public const string InvalidPasswordHasherIterationCount = "The iteration count must be a positive integer.";
        public const string ValueCannotBeNullOrEmpty = "Value cannot be null or empty.";

        public const string EmptyData = "The provided cannot be null or empty.";
    }
}
