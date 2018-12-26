using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using AwesomeBooks.Model.Exceptions;
using AwesomeBooks.Contracts.Envelope;
using AwesomeBooks.Api.Extensions;

namespace AwesomeBooks.Api.Middlewares
{
    public class CustomExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception exception)
            {
                if (httpContext.Response.HasStarted)
                {
                    throw;
                }

                var headers = new HeaderDictionary();
                foreach(var h in httpContext.Response.Headers)
                {
                    headers.Add(h.Key, h.Value);
                }

                var statusCode = ConvertExceptionToHttpStatusCode(exception);
                httpContext.Response.Clear();
                foreach(var h in headers)
                {
                    httpContext.Response.Headers.Add(h.Key, h.Value);
                }
                httpContext.Response.StatusCode = (int)statusCode;
                httpContext.Response.ContentType = "application/json";
                var statusCodeStr = Regex.Replace(statusCode.ToString(), "([a-z])([A-Z])", "$1 $2");
                var metaError = new MetaError(statusCode, httpContext.Request.CorrelationId(), exception.Message, statusCodeStr);
                await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(new Error(metaError)));

                //Log.Error(exception, "Application unhandled exception happened.");
            }
        }

        private HttpStatusCode ConvertExceptionToHttpStatusCode(Exception exception)
        {
            switch (exception)
            {
                case FormatException _:
                case InvalidOperationException _:
                case EntityAlreadyExistsException _:
                case ValidationException _:
                    return HttpStatusCode.BadRequest;
                case EntityNotFoundException _:
                    return HttpStatusCode.NotFound;
                default:
                    return HttpStatusCode.InternalServerError;
            }
        }
    }
}
