namespace AuthorApp.DataAccessContracts.Models
{
    public class ResultData<T> : ResultData
    {
        public T Data { get; set; }

        public ResultData(T data)
        {
            IsSuccess = true;
            Data = data;
        }

        public ResultData(string message)
        {
            IsSuccess = false;
            ErrorMessage = message;
        }

        public ResultData(string message, int statusCode)
        {
            IsSuccess = false;
            ErrorMessage = message;
            StatusCode = statusCode;
        }
    }

    public class ResultData
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public int StatusCode { get; set; }

        public ResultData() {
            IsSuccess = true;
        }

        public ResultData(string message)
        {
            IsSuccess = false;
            ErrorMessage = message;
        }

        public ResultData(string message, int statusCode)
        {
            IsSuccess = false;
            ErrorMessage = message;
            StatusCode = statusCode;
        }
    }
}
