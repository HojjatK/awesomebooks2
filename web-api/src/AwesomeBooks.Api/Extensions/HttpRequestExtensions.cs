using System.Linq;
using Microsoft.AspNetCore.Http;
using AwesomeBooks.Contracts.Envelope;

namespace AwesomeBooks.Api.Extensions
{
    public static class HttpRequestExtensions
    {
        public static string CorrelationId(this HttpRequest request)
        {
            request.Headers.TryGetValue(Meta.CorrelationIdKey, out var values);
            return values.FirstOrDefault();
        }

        public static string UserId(this HttpRequest request)
        {
            request.Headers.TryGetValue(Meta.UserIdKey, out var values);
            return values.FirstOrDefault();
        }
    }
}