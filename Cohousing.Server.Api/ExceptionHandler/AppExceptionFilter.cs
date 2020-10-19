using System;
using System.Net;
using Cohousing.Server.Model.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;

namespace Cohousing.Server.Api.ExceptionHandler
{
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
        private readonly IConfiguration _configuration;

        public ApiExceptionFilter(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public override void OnException(ExceptionContext context)
        {
            var showErrorDetails = bool.Parse(_configuration.GetSection("AppSettings:ShowErrorDetails").Value);
            ApiError apiError = null;
            switch (context.Exception)
            {
                case ApiException apiException:
                {
                    // handle explicit 'known' API errors
                    context.Exception = null;
                    apiError = new ApiError(apiException, showErrorDetails);
                    context.HttpContext.Response.StatusCode = apiException.HttpStatusCode;
                    break;
                }
                case AppException appException:
                {
                    // handle explicit 'known' API errors
                    context.Exception = null;
                    apiError = new ApiError(appException, showErrorDetails);
                    context.HttpContext.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                    break;
                }
                
                case UnauthorizedAccessException ex:
                    apiError = new ApiError("Unauthorized Access", ex, showErrorDetails);
                    context.HttpContext.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
                    break;
                case Exception ex:
                {
                    // Unhandled errors
                    apiError = new ApiError(ex, showErrorDetails);
                    context.HttpContext.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                    break;
                }
            }

            // always return a JSON result
            context.Result = new JsonResult(apiError);
            context.ExceptionHandled = true;
            base.OnException(context);
        }
    }
}