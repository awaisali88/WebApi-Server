using FluentValidation;
using WebAPI_ViewModel.Identity;
using WebAPI_ViewModel.Validator.CustomValidators;

namespace WebAPI_ViewModel.Validator
{
    public class ChangePasswordViewModelValidator : AbstractValidator<ChangePasswordViewModel>
    {
        public ChangePasswordViewModelValidator()
        {
            RuleFor(x => x.OldPassword).NotEmpty().ValidatePassword();
            RuleFor(x => x.NewPassword).NotEmpty().ValidatePassword();
            RuleFor(x => x.ConfirmPassword).NotEmpty().Equal(x => x.NewPassword)
                .When(x => !string.IsNullOrEmpty(x.NewPassword));

        }
    }
}
