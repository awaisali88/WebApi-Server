using FluentValidation;
using WebAPI_ViewModel.DTO;

namespace WebAPI_ViewModel.Validator
{
    public class OrderDetailsViewModelValidator : AbstractValidator<OrderDetailsViewModel>
    {
        public OrderDetailsViewModelValidator()
        {
        }
    }
}
