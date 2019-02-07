using Swashbuckle.AspNetCore.Filters;
using WebAPI_ViewModel.Identity;

namespace WebAPI_ViewModel.ViewModelSwaggerExamples
{
    public class VerifyTokenViewModelExample : IExamplesProvider<VerifyTokenViewModel>
    {
        public VerifyTokenViewModel GetExamples()
        {
            return new VerifyTokenViewModel
            {
                Token = "Verify email token (Url Decoded string) (c = token)",
                UserId = "User id from verify email (f = userid)",
            };
        }
    }
}
