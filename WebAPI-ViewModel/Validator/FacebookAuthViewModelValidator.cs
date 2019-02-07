using FluentValidation;
using WebAPI_ViewModel.Identity;

namespace WebAPI_ViewModel.Validator
{
    public class FacebookAuthViewModelValidator : AbstractValidator<FacebookAuthViewModel>
    {
        public FacebookAuthViewModelValidator()
        {
            RuleFor(x => x.AccessToken).NotEmpty();
        }
    }
}
