using Swashbuckle.AspNetCore.Filters;
using WebAPI_ViewModel.Identity;

namespace WebAPI_ViewModel.ViewModelSwaggerExamples
{
    public class AddNewUserViewModelExample : IExamplesProvider<AddNewUserViewModel>
    {
        public AddNewUserViewModel GetExamples()
        {
            return new AddNewUserViewModel
            {
                FirstName = "David",
                LastName = "Fox",
                Email = "david.fox79@example.com"
            };
        }
    }
}
