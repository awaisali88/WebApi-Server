using Swashbuckle.AspNetCore.Filters;
using WebAPI_ViewModel.Identity;

namespace WebAPI_ViewModel.ViewModelSwaggerExamples
{
    public class ChangePasswordViewModelExample : IExamplesProvider<ChangePasswordViewModel>
    {
        public ChangePasswordViewModel GetExamples()
        {
            return new ChangePasswordViewModel
            {
                OldPassword = "v@TFG<B69E",
                NewPassword = "JfD5N@V8!w",
                ConfirmPassword = "JfD5N@V8!w"
            };
        }
    }
}
