using Swashbuckle.AspNetCore.Filters;
using WebAPI_ViewModel.Identity;

namespace WebAPI_ViewModel.ViewModelSwaggerExamples
{
    public class LoginUserViewModelExample : IExamplesProvider<LoginUserViewModel>
    {
        public LoginUserViewModel GetExamples()
        {
            return new LoginUserViewModel
            {
                Email = "david.fox79@example.com",
                Password = "JfD5N@V8!w"
            };
        }
    }
}
