using System;

namespace Cohousing.Server.Model.Common
{
    public class AppException : Exception
    {
        public AppException(AppErrorCodes errorCode, string message) : base(message)
        {
            ErrorCode = errorCode;
        }
        
        public AppException(AppErrorCodes errorCode, string message, Exception ex) : base(message, ex)
        {
            ErrorCode = errorCode;
        }
        
        public AppException(AppErrorCodes errorCode, Exception exception) : base(exception.Message, exception)
        {
            ErrorCode = errorCode;
        }

        public AppErrorCodes ErrorCode { get; } 
    }
}