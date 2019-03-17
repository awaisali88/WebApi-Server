using FluentValidation;
using WebAPI_ViewModel.DTO;

namespace WebAPI_ViewModel.Validator
{
    public class ProductsViewModelValidator : AbstractValidator<ProductsViewModel>
    {
        public ProductsViewModelValidator()
        {
        }
    }
}
