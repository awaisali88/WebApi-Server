using FluentValidation;
using WebAPI_ViewModel.DTO;

namespace WebAPI_ViewModel.Validator
{
    public class EmployeesViewModelValidator : AbstractValidator<EmployeesViewModel>
    {
        public EmployeesViewModelValidator()
        {
        }
    }
}
