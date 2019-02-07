using Swashbuckle.AspNetCore.Filters;
using WebAPI_ViewModel.Identity;

namespace WebAPI_ViewModel.ViewModelSwaggerExamples
{
    public class SetPasswordViewModelExample : IExamplesProvider<SetPasswordViewModel>
    {
        public SetPasswordViewModel GetExamples()
        {
            return new SetPasswordViewModel
            {
                NewPassword = "JfD5N@V8!w",
                ConfirmPassword = "JfD5N@V8!w"
            };
        }
    }
}
