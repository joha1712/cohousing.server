using System;
using Cohousing.Server.Model.Common;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Cohousing.Server.Api.ExceptionHandler
{
    public class ApiException : Exception
    {
        public int ErrorCode { get; }
        public int HttpStatusCode { get; }

        public ApiException(string message, int errorCode, int httpStatusCode) : base(message)
        {
            HttpStatusCode = httpStatusCode;
            ErrorCode = errorCode;
        }
        
        public ApiException(string message, int errorCode, int httpStatusCode, Exception ex) : base(message, ex)
        {
            HttpStatusCode = httpStatusCode;
            ErrorCode = errorCode;
        }
        
        public ApiException(string message, int httpStatusCode, AppException ex) : base(message, ex)
        {
            HttpStatusCode = httpStatusCode;
            ErrorCode = (int) ex.ErrorCode;
        }
    }
}