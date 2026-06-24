namespace webApp.GenericResponse
{
    public class ResponseResult<T>
    {

        public T? Data { get; set; }
        public string? Message { get; set; }

        public bool Status { get; set; } = false;

        public static ResponseResult<T> Success(T? data,string message)
        {
            return new ResponseResult<T>
            {
                Data = data,
                Message = message,
                Status = true
            };
          
        }

        public static ResponseResult<T> Failure(T? data ,string message)
        {
            return new ResponseResult<T>
            {
                Data = data,
                Message = message,
                Status = false
            };
        }

    }
}
