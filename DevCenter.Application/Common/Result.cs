namespace DevCenter.Application.Common
{
    public class Result
    {
        protected Result(bool isSuccess, string errorMessage = null)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
        }

        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public string ErrorMessage { get; }

        public static Result Success() => new(true);
        public static Result Failure(string errorMessage) => new(false, errorMessage);
    }

    public class Result<T> : Result
    {
        private readonly T _value;

        private Result(T value, bool isSuccess, string errorMessage) : base(isSuccess, errorMessage)
        {
            _value = value;
        }

        public T Value => IsSuccess ? _value : throw new InvalidOperationException("No value for failure result.");

        public static Result<T> Success(T value) => new(value, true, null);
        public static Result<T> Failure(string errorMessage) => new(default, false, errorMessage);
    }

    public static class ResultExtensions
    {
        public static TOut Match<TOut>(
            this Result result,
            Func<TOut> onSuccess,
            Func<string, TOut> onFailure)
        {
            return result.IsSuccess ? onSuccess() : onFailure(result.ErrorMessage);
        }
    }
}
