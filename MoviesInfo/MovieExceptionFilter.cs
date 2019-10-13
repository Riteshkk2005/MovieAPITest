using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

namespace MoviesInfo
{
    public class MovieExceptionFilter: ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            HttpStatusCode status = HttpStatusCode.InternalServerError;
            String message = String.Empty;
            var exceptionType = actionExecutedContext.Exception.GetType();
            if (exceptionType == typeof(UnauthorizedAccessException))
            {
                message = "Access to the Web API is not authorized.";
                status = HttpStatusCode.Unauthorized;
            }
            else if (exceptionType == typeof(DivideByZeroException))
            {
                message = "Internal Server Error.";
                status = HttpStatusCode.InternalServerError;
            }
            else if (exceptionType == typeof(InvalidOperationException))
            {
                message = "Invalid Operation.";
                status = HttpStatusCode.InternalServerError;
            }
            else if (exceptionType == typeof(System.Data.Entity.Infrastructure.DbUpdateConcurrencyException))
            {
                message = "Duplicate data not allow";
                status = HttpStatusCode.BadRequest;
            }
            else if (exceptionType == typeof(ArgumentNullException))
            {
                message = actionExecutedContext.Exception.Message;
                status = HttpStatusCode.NotFound;
            }
            else if (exceptionType == typeof(NullReferenceException))
            {
                message = "Null Reference";
                status = HttpStatusCode.BadRequest;
            }
            else
            {
                message = "Not found.";
                status = HttpStatusCode.NotFound;
            }

            actionExecutedContext.Response = new HttpResponseMessage()
            {
                Content = new StringContent(message, System.Text.Encoding.UTF8, "text/plain"),
                StatusCode = status,
                ReasonPhrase = message + ", RequestUri: "+ actionExecutedContext.Request.RequestUri,                
            };
            //for Logging purpose
            base.OnException(actionExecutedContext);
        }
    }
}