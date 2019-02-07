using Swashbuckle.AspNetCore.Filters;
using WebAPI_ViewModel.Identity;

namespace WebAPI_ViewModel.ViewModelSwaggerExamples
{
    public class RegisterUserViewModelExample : IExamplesProvider<RegisterUserViewModel>
    {
        public RegisterUserViewModel GetExamples()
        {
            return new RegisterUserViewModel
            {
                FirstName = "David",
                LastName = "Fox",
                Email = "david.fox79@example.com",
                Password = "JfD5N@V8!w",
                ConfirmPassword = "JfD5N@V8!w",
            };
        }
    }
}
