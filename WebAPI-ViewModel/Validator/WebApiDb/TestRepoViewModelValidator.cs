using FluentValidation;
using WebAPI_ViewModel.DTO;

namespace WebAPI_ViewModel.Validator
{
    public class TestRepoViewModelValidator : AbstractValidator<TestRepoViewModel>
    {
        public TestRepoViewModelValidator()
        {
        }
    }
}
