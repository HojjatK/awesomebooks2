using Newtonsoft.Json;
using System;
using System.Net;

namespace AwesomeBooks.Contracts.Envelope
{
    public class Meta
    {
        public const string CorrelationIdKey = "correlation-id";
        public const string UserIdKey = "user-id";

        public Meta(HttpStatusCode httpStatusCode, string correlationId)
        {
            DateUtc = DateTime.UtcNow;
            Code = (int)httpStatusCode;
            CorrelationId = correlationId;
        }

        public int Code { get; }

        [JsonProperty(PropertyName = CorrelationIdKey)]
        public string CorrelationId { get; }

        [JsonProperty(PropertyName = UserIdKey)]
        public string UserId { get; }

        public DateTime DateUtc { get; }
    }

    public class MetaError : Meta
    {
        public string ErrorType { get; }

        public string ErrorMessage { get; }

        public MetaError(HttpStatusCode httpStatusCode, string correlationId, string errorMessage, string errorType)
            : base(httpStatusCode, correlationId)
        {
            ErrorMessage = errorMessage;
            ErrorType = errorType;
        }
    }
}
