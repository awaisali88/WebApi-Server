using FluentValidation;
using WebAPI_ViewModel.Identity;

namespace WebAPI_ViewModel.Validator
{
    public class AddNewUserViewModelValidator : AbstractValidator<AddNewUserViewModel>
    {
        public AddNewUserViewModelValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
        }
    }
}
