using FluentValidation;
using WebAPI_ViewModel.Identity;
using WebAPI_ViewModel.Validator.CustomValidators;

namespace WebAPI_ViewModel.Validator
{
    public class RegisterUserViewModelValidator : AbstractValidator<RegisterUserViewModel>
    {
        public RegisterUserViewModelValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();

            RuleFor(x => x.Password).NotEmpty().ValidatePassword();

            RuleFor(x => x.ConfirmPassword).NotEmpty().Equal(x => x.Password)
                .When(x => !string.IsNullOrEmpty(x.Password)).ValidatePassword();

        }
    }
}
