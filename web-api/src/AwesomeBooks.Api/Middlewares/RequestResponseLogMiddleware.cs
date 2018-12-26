using AwesomeBooks.Api.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeBooks.Api.Middlewares
{
    public class RequestResponseLogMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseLogMiddleware> _logger;

        public RequestResponseLogMiddleware(RequestDelegate next, ILogger<RequestResponseLogMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            await LogRequestAsync(httpContext.Request);
            var sw = Stopwatch.StartNew();
            await _next(httpContext);
            sw.Stop();
            await LogResponseAsync(httpContext.Response, sw);
        }

        private async Task LogRequestAsync(HttpRequest httpRequest)
        {
            httpRequest.EnableRewind();
            using (var reader = new StreamReader(httpRequest.Body, Encoding.UTF8, true, 1024, true))
            {
                var headers = httpRequest.Headers.AsString();
                var body = await reader.ReadToEndAsync();
                var data = new { Headers = headers, Content = body };

                _logger.LogInformation("HTTP request {Duration} {data}", null, data);
            }

            httpRequest.Body.Position = 0;
        }

        private async Task LogResponseAsync(HttpResponse httpResponse, Stopwatch stopwatch)
        {
            var body = string.Empty;

            if (httpResponse.Body.CanRead)
            {
                using (var reader = new StreamReader(httpResponse.Body, Encoding.UTF8, true, 1024, true))
                {
                    body = await reader.ReadToEndAsync();
                }
            }

            var headers = httpResponse.Headers.AsString();
            var data = new { Headers = headers, Content = body };
            _logger.LogInformation("HTTP response {Duration} {data}", stopwatch.ElapsedMilliseconds, data);
        }
    }
}
