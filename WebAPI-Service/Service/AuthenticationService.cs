using System.Collections.Generic;
using System.Linq;
using Common.Exception;
using Common.Messages;
using FluentValidation;
using FluentValidation.Results;
using WebAPI_ViewModel.Identity;
using WebAPI_ViewModel.Validator.CustomValidators;

namespace WebAPI_Service.Service
{
    public interface IAuthenticationService
    {
        bool ValidateLogin(LoginUserViewModel data);
        List<ErrorsModelException> ValidateLoginForWeb(LoginUserViewModel data);

    }

    public class AuthenticationService : IAuthenticationService
    {
        private readonly IValidator<LoginUserViewModel> _validator;  

        public AuthenticationService(IValidator<LoginUserViewModel> validator)  
        {
            _validator = validator;
        }

        public bool ValidateLogin(LoginUserViewModel data)
        {
            ValidationResult res = _validator.Validate(data, ruleSet: ValidatorRuleSetNames.All);

            if (!res.IsValid)
            {
                List<ErrorsModelException> errors = res.Errors.Select(x => new ErrorsModelException
                    {Description = x.ErrorMessage, Code = x.PropertyName}).ToList();
                throw new ModelValidationException(ErrorMessages.ModelValidationFailed, errors);
            }

            return true;
        }
        public List<ErrorsModelException> ValidateLoginForWeb(LoginUserViewModel data)
        {
            ValidationResult res = _validator.Validate(data);

            if (!res.IsValid)
            {
                return res.Errors.Select(x => new ErrorsModelException
                    { Description = x.ErrorMessage, Code = x.PropertyName }).ToList();
            }

            return new List<ErrorsModelException>();
        }

    }
}
