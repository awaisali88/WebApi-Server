using System.Text.RegularExpressions;
using FluentValidation.Validators;

namespace WebAPI_ViewModel.Validator.CustomValidators
{
    public class PasswordPropertyValidator : PropertyValidator
    {
        public PasswordPropertyValidator()
            : base("'{PropertyName}' must be 8-20 characters long and must contain 1 small-case letter, 1 Capital letter, 1 digit and 1 special character.")
        {
        }
        protected override bool IsValid(PropertyValidatorContext context)
        {
            if (context.PropertyValue == null && context.PropertyValue is string) return false;

            var passwordValue = context.PropertyValue as string ?? "";
            Regex regex = new Regex(@"(?=^.{8,20}$)(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&amp;*()_+}{&quot;:;'?/&gt;.&lt;,])(?!.*\s).*$");
            if (regex.Match(passwordValue).Success) return true;
            return false;
        }
    }
}
