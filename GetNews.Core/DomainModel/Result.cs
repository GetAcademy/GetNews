namespace GetNews.Core.DomainModel
{
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public T Value { get; }
        public string Error { get; }

        protected Result(T value, bool isSuccess, string error)
        {
            Value = value;
            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result<T> Ok(T value) => new Result<T>(value, true, null);
        public static Result<T> Fail(string error) => new Result<T>(default, false, error);
        public static Result<T> Fail<TENUM>(TENUM error) => new Result<T>(default, false, error.ToString());
    }

}
