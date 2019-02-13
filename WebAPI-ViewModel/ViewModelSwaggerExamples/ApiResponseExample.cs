using Common.Messages;
using Swashbuckle.AspNetCore.Filters;
using WebAPI_ViewModel.Response;

namespace WebAPI_ViewModel.ViewModelSwaggerExamples
{
    public class ApiResponseExample : IExamplesProvider<ApiResponse>
    {
        public ApiResponse GetExamples()
        {
            return new ApiResponse(true, InfoMessages.CommonInfoMessage);
        }
    }

    //public class ApiResponseExample<T> : IExamplesProvider<ApiResponse<T>> where T : class
    //{
    //    public ApiResponse<T> GetExamples()
    //    {
    //        return new ApiResponse<T>(true, InfoMessages.CommonInfoMessage, default(T));
    //    }
    //}

    public class ApiResponse401Example : IExamplesProvider
    {
        public object GetExamples()
        {
            return new ApiResponse(false, ErrorMessages.UnAuthorized);
        }
    }

}
