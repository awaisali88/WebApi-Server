using FluentValidation;
using WebAPI_ViewModel.DTO;

namespace WebAPI_ViewModel.Validator
{
    public class OrdersViewModelValidator : AbstractValidator<OrdersViewModel>
    {
        public OrdersViewModelValidator()
        {
        }
    }
}
