using System;
using Cohousing.Server.Model.Common;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Cohousing.Server.Api.ExceptionHandler
{
    public class ApiError
    {
        public int ErrorCode { get;  }
        public string Message { get; }
        public string Details { get; }
        
        public ApiError(Exception ex, bool showDetails)
        {
            ErrorCode = -1;
            Message = ex.Message;
            Details = showDetails ? ex.ToString() : "";
            
        }
        
        public ApiError(AppException ex, bool showDetails)
        {
            ErrorCode = (int) ex.ErrorCode;
            Message = ex.Message;
            Details = showDetails ? ex.ToString() : "";
            
        }
        
        public ApiError(string message, Exception ex, bool showDetails)
        {
            ErrorCode = -1;
            Message = message;
            Details = showDetails ? ex.ToString() : "";
        }
        
        public ApiError(ApiException ex, bool showDetails)
        {
            ErrorCode = ex.ErrorCode;
            Message = ex.Message;
            Details = showDetails ? ex.ToString() : "";
        }
    }
}