using System;

namespace Standard.Contracts.Exceptions
{
    public class InternalException : Exception
    {
        public int ErrorCode;
        public string ErrorMessage;

        public InternalException(int code)
        {
            ErrorCode = code;
        }

        public InternalException(int code, string message) : base(message)
        {
            ErrorCode = code;
            ErrorMessage = message;
        }
    }
}
