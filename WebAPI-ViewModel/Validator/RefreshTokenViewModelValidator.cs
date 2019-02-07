using FluentValidation;
using WebAPI_ViewModel.Identity;

namespace WebAPI_ViewModel.Validator
{
    public class RefreshTokenViewModelValidator : AbstractValidator<RefreshTokenViewModel>
    {
        public RefreshTokenViewModelValidator()
        {
            RuleFor(x => x.Token).NotEmpty();
            RuleFor(x => x.TokenNumber).NotEmpty();
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}
