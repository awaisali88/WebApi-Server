using FluentValidation;
using WebAPI_ViewModel.Identity;

namespace WebAPI_ViewModel.Validator
{
    public class VerifyTokenViewModelValidator : AbstractValidator<VerifyTokenViewModel>
    {
        public VerifyTokenViewModelValidator()
        {
            RuleFor(x => x.Token).NotEmpty();
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}
