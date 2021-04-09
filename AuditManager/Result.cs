using System;

namespace AuditManager
{
    public class Result
    {
        public bool IsSuccess { get; }

        public bool IsFailure => !IsSuccess;

        public string Error { get; }

        protected Result(bool isSuccess, string error)
        {
            if (isSuccess && !string.IsNullOrEmpty(error))
                throw new InvalidOperationException();

            if (!isSuccess && string.IsNullOrEmpty(error))
                throw new InvalidOperationException();

            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Ok()
        {
            return new Result(isSuccess: true, error: null);
        }

        public static Result<T> Ok<T>(T value)
        {
            return new Result<T>(value, true, null);
        }

        public static Result Fail(string error)
        {
            return new Result(isSuccess: false, error: error);
        }
        public static Result<T> Fail<T>(string error)
        {
            return new Result<T>(default(T), false, error);
        }
    }

    public class Result<T> : Result
    {
        private readonly T _value;

        public T Value
        {
            get
            {
                if (!IsSuccess)
                    throw new InvalidOperationException();

                return _value;
            }
        }

        protected internal Result(T value, bool isSuccess, string error) : base(isSuccess, error)
        {
            _value = value;
        }
    }
}
