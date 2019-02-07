using FluentValidation;
using WebAPI_ViewModel.Identity;
using WebAPI_ViewModel.Validator.CustomValidators;

namespace WebAPI_ViewModel.Validator
{
    public class LoginUserViewModelValidator : AbstractValidator<LoginUserViewModel>
    {
        public LoginUserViewModelValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();

            RuleFor(x => x.Password).NotEmpty().ValidatePassword();

            //RuleSet(ValidatorRuleSetNames.All, () =>
            //{
            //    RuleFor(x => x.Email).EmailAddress().WithMessage("Email address is not valid");
            //    RuleFor(x => x.Password).MinimumLength(8).WithMessage("Password length should be 8");
            //});
        }
    }
}
