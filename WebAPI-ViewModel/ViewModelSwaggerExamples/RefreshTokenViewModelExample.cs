using Swashbuckle.AspNetCore.Filters;
using WebAPI_ViewModel.Identity;

namespace WebAPI_ViewModel.ViewModelSwaggerExamples
{
    public class RefreshTokenViewModelExample : IExamplesProvider<RefreshTokenViewModel>
    {
        public RefreshTokenViewModel GetExamples()
        {
            return new RefreshTokenViewModel
            {
                TokenNumber = "Refresh Token Number from JWT",
                Token = "Access token from JWT",
                UserId = "User Id form JWT"
            };
        }
    }
}
