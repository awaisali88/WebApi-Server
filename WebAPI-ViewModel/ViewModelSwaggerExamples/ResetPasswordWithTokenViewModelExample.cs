using Swashbuckle.AspNetCore.Filters;
using WebAPI_ViewModel.Identity;

namespace WebAPI_ViewModel.ViewModelSwaggerExamples
{
    public class ResetPasswordWithTokenViewModelExample : IExamplesProvider<ResetPasswordWithTokenViewModel>
    {
        public ResetPasswordWithTokenViewModel GetExamples()
        {
            return new ResetPasswordWithTokenViewModel
            {
                Token = "Reset password token from email (Url Decoded string) (c = token)",
                UserId = "User id from reset password email (f = userid)",
                NewPassword = "JfD5N@V8!w",
                ConfirmPassword = "JfD5N@V8!w"
            };
        }
    }
}
