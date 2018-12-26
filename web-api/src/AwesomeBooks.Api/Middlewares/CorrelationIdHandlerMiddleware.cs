using System;
using System.Threading.Tasks;
using AwesomeBooks.Contracts.Envelope;
using Microsoft.AspNetCore.Http;

namespace AwesomeBooks.Api.Middlewares
{
    public class CorrelationIdHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public CorrelationIdHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            if (!httpContext.Request.Headers.TryGetValue(Meta.CorrelationIdKey, out var value))
            {
                httpContext.Request.Headers.Add(Meta.CorrelationIdKey, Guid.NewGuid().ToString());
            }
            else if (string.IsNullOrWhiteSpace(value.ToString()))
            {
                httpContext.Request.Headers[Meta.CorrelationIdKey] = Guid.NewGuid().ToString();
            }

            return _next(httpContext);
        }
    }
}
