namespace Shared.Models
{
    public class Result
    {
        public bool IsSuccess { get; }
        public string Error { get; }

        public Result(bool isSuccess, string errorMessage)
        {
            IsSuccess = isSuccess;
            Error = errorMessage;
        }

        public static Result Success() 
        {
            return new Result (true, string.Empty);
        }

        public static Result Failure(string errorMessage) 
        {
            return new Result (false, errorMessage);
        }
    }
}
