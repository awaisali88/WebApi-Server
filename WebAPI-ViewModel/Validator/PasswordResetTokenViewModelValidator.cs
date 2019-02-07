using FluentValidation;
using WebAPI_ViewModel.Identity;

namespace WebAPI_ViewModel.Validator
{
    public class PasswordResetTokenViewModelValidator : AbstractValidator<PasswordResetTokenViewModel>
    {
        public PasswordResetTokenViewModelValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
        }
    }
}
