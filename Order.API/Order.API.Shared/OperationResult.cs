using Order.API.Shared.Constants;
using System.Collections.Generic;

namespace Order.API.Shared
{
    public class OperationResult
    {
        public bool Failure { get { return !Success; } }
        public bool Success { get; }
        public List<Error> ErrorList { get; }
        protected OperationResult(bool success)
        {
            Success = success;
        }
        protected OperationResult(Error error)
        {
            ErrorList = new List<Error> { error };
        }
        public static OperationResult<R> Create<R>(R value)
            => new OperationResult<R>(true, value);
        public static OperationResult<R> CreateError<R>(Error error)
         => new OperationResult<R>(error);      
        protected OperationResult(List<Error> errorList)
        {
            ErrorList = errorList ?? new List<Error>();
        }
        public static OperationResult CreateError(string errorCode, string errorMessage, ushort errorType = ErrorType.TECHNICAL)
         => new OperationResult(new Error(errorCode, errorMessage, errorType));
    }

    public class OperationResult<T> : OperationResult
    {
        public T Value { get; }
        internal OperationResult(T result) : this(true, result)
        {
          
        }
        internal OperationResult(Error error) : base(error) { }
        internal OperationResult(List<Error> errorList) : base(errorList) { }
        internal OperationResult(bool success) : base(success) { }
        internal OperationResult(bool success, T value) : base(success)
        {
            Value = value;
        }
    }
}
