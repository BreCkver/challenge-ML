using System.Collections.Generic;

namespace Order.API.Shared.Entities
{
    public class ResponseGeneric
    {
        public bool Failure { get { return !Success; } }
        public bool Success { get; }
        public List<Error> ErrorList { get; }
        protected ResponseGeneric(bool success)
            => Success = success;
        protected ResponseGeneric(Error error)
            => ErrorList = new List<Error> { error };
        public static ResponseGeneric<R> Create<R>(R value)
            => new ResponseGeneric<R>(true, value);
        public static ResponseGeneric<R> CreateError<R>(Error error)
            => new ResponseGeneric<R>(error);      
        protected ResponseGeneric(List<Error> errorList)
            => ErrorList = errorList ?? new List<Error>();
        public static ResponseGeneric<R> CreateError<R>(List<Error> errorList)
            => new ResponseGeneric<R>(errorList);

      
    }
    public class ResponseGeneric<T> : ResponseGeneric
    {
        public T Value { get; }
        internal ResponseGeneric(T result) : this(true, result) { }
        internal ResponseGeneric(Error error) : base(error) { }
        internal ResponseGeneric(List<Error> errorList) : base(errorList) { }
        internal ResponseGeneric(bool success) : base(success) { }
        internal ResponseGeneric(bool success, T value) : base(success)
            => Value = value;
    }
    public static class OperationResultExtensions
    {
        public static ResponseGeneric<R> AsError<R>(this ResponseGeneric result)
            => ResponseGeneric.CreateError<R>(result.ErrorList);
    }
}

