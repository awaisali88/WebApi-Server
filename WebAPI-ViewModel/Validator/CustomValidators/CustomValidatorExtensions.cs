using FluentValidation;

namespace WebAPI_ViewModel.Validator.CustomValidators
{
    public static class CustomValidatorExtensions
    {
        public static IRuleBuilderOptions<T, string> ValidatePassword<T>(
            this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new PasswordPropertyValidator());
        }
    }
}
