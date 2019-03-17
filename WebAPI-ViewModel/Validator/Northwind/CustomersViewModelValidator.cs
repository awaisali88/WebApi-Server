using FluentValidation;
using WebAPI_ViewModel.DTO;

namespace WebAPI_ViewModel.Validator
{
    public class CustomersViewModelValidator : AbstractValidator<CustomersViewModel>
    {
        public CustomersViewModelValidator()
        {
        }
    }
}
