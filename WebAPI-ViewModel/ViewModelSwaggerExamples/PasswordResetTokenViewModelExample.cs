using Swashbuckle.AspNetCore.Filters;
using WebAPI_ViewModel.Identity;

namespace WebAPI_ViewModel.ViewModelSwaggerExamples
{
    public class PasswordResetTokenViewModelExample : IExamplesProvider<PasswordResetTokenViewModel>
    {
        public PasswordResetTokenViewModel GetExamples()
        {
            return new PasswordResetTokenViewModel
            {
                Email = "david.fox79@example.com"
            };
        }
    }
}
