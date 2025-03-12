namespace GoodsAndOrders.Common
{
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public T? Value { get; }
        public string? ErrorMessage { get; }
        public int StatusCode { get; }

        private Result(T value)
        {
            IsSuccess = true;
            Value = value;
            StatusCode = 200;
        }

        private Result(string errorMessage, int statusCode)
        {
            IsSuccess = false;
            ErrorMessage = errorMessage;
            StatusCode = statusCode;
        }

        public static Result<T> Success(T value)
        {
            return new(value);
        }

        public static Result<T> Fail(string errorMessage, int statusCode = 400)
        {
            return new(errorMessage, statusCode);
        }
    }
}
