namespace WebAPI_ViewModel.Response
{
    public class ApiResponse
    {
        public ApiResponse(bool status, string msg, object data)
        {
            Status = status;
            Msg = msg;
            Data = data;
        }
        public bool Status { get; set; }

        public string Msg { get; set; }

        public object Data { get; set; }
    }
    public class ApiResponse<T> : ApiResponse
    {
        public ApiResponse(bool status, string msg, T data) : base(status, msg, data)
        {
        }
        public new T Data { get => (T)base.Data; set => base.Data = value; }
    }

}
