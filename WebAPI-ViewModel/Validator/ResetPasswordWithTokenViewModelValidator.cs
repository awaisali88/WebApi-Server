using FluentValidation;
using WebAPI_ViewModel.Identity;
using WebAPI_ViewModel.Validator.CustomValidators;

namespace WebAPI_ViewModel.Validator
{
    public class ResetPasswordWithTokenViewModelValidator : AbstractValidator<ResetPasswordWithTokenViewModel>
    {
        public ResetPasswordWithTokenViewModelValidator()
        {
            RuleFor(x => x.Token).NotEmpty();
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.NewPassword).NotEmpty().ValidatePassword();
            RuleFor(x => x.ConfirmPassword).NotEmpty().Equal(x => x.NewPassword)
                .When(x => !string.IsNullOrEmpty(x.NewPassword));
        }
    }
}
