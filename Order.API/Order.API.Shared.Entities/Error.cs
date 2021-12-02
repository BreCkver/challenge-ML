using Newtonsoft.Json;
using Order.API.Shared.Entities.Constants;
using System;
using System.Collections.Generic;

namespace Order.API.Shared.Entities
{
    public class Error
    {
        public string Code { get; }
        public string Message { get; }
        public ushort Type { get; }
        public ErrorTrace Trace { get; }

        [JsonConstructor]
        public Error(string code, string message, ushort type = ErrorType.TECHNICAL)
        {
            Code = code;
            Message = message;
            Type = type;
        }

        public Error(string code, Exception exception) : this(code, exception.Message, ErrorType.TECHNICAL)
        {
            Trace = new ErrorTrace(exception);
        }

        public Error(string code, string message, Exception exception) : this(code, message, ErrorType.TECHNICAL)
        {
            Trace = new ErrorTrace(exception);
        }

        public override string ToString()
        {
            return $"{{ Code: \"{Code}\", Type: \"{Type}\", Message: \"{Message}\", Trace: \"{Trace}\" }}";
        }
     
    }

    public class ErrorTrace
    {
        public string Message { get; }
        public string StackTrace { get; }
        public string Type { get; }

        public ErrorTrace(Exception exception)
        {
            if (exception == null)
            {
                return;
            }
            Message = exception.Message;
            var stackTrace = exception.StackTrace;
            if (!string.IsNullOrEmpty(stackTrace))
            {
                var stackTraceLenght = stackTrace.Length;
                const int maxCapacity = 1000;
                StackTrace = stackTraceLenght > maxCapacity ? stackTrace.Substring(stackTraceLenght - maxCapacity) : stackTrace;
            }
            Type = exception.GetType().ToString();
        }

        public override string ToString()
        {
            return $"{{ Type: \"{Type}\", Message: \"{Message}\", StackTrace: \"{StackTrace}\" }}";
        }
    }
}
