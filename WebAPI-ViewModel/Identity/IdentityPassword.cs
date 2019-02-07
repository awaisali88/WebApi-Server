namespace WebAPI_ViewModel.Identity
{
    public class ChangePasswordViewModel
    {
        public string OldPassword { get; set; }

        public string NewPassword { get; set; }

        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordWithTokenViewModel
    {
        public string Token { get; set; }

        public string UserId { get; set; }

        public string NewPassword { get; set; }

        public string ConfirmPassword { get; set; }
    }

    public class SetPasswordViewModel
    {
        public string NewPassword { get; set; }

        public string ConfirmPassword { get; set; }
    }

    public class PasswordResetTokenViewModel
    {
        public string Email { get; set; }
    }

    public class VerifyTokenViewModel
    {
        public string Token { get; set; }

        public string UserId { get; set; }

    }

    public class RefreshTokenViewModel
    {
        public string Token { get; set; }

        public string UserId { get; set; }

        public string TokenNumber { get; set; }
    }
}
