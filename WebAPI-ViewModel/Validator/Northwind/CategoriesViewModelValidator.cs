using FluentValidation;
using WebAPI_ViewModel.DTO;

namespace WebAPI_ViewModel.Validator
{
    public class CategoriesViewModelValidator : AbstractValidator<CategoriesViewModel>
    {
        public CategoriesViewModelValidator()
        {
        }
    }
}
