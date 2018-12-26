using System.Net;
using Microsoft.AspNetCore.Mvc;
using AwesomeBooks.Contracts.Envelope;

namespace AwesomeBooks.Api.Extensions
{
    public static class ControllerExtensions
    {
        public static IActionResult ActionResult<TData>(this ControllerBase controller, HttpStatusCode statusCode, TData data)
        {
            var correlationId = controller.Request.CorrelationId();
            var content = new Content<Meta, TData>(new Meta(statusCode, correlationId), data);

            return new ObjectResult(content) { StatusCode = (int)statusCode };
        }

        public static IActionResult ActionResult(this ControllerBase controller, HttpStatusCode statusCode)
        {
            var correlationId = controller.Request.CorrelationId();
            var content = new Content<Meta>(new Meta(statusCode, correlationId));

            return new ObjectResult(content) { StatusCode = (int)statusCode };
        }

        public static IActionResult OkActionResult(this ControllerBase controller)
        {
            var correlationId = controller.Request.CorrelationId();
            var content = new Content<Meta>(new Meta(HttpStatusCode.OK, correlationId));

            return new OkObjectResult(content);
        }

        public static IActionResult OkActionResult<TData>(this ControllerBase controller, TData data)
        {
            var correlationId = controller.Request.CorrelationId();
            var content = new Content<Meta, TData>(new Meta(HttpStatusCode.OK, correlationId), data);

            return new OkObjectResult(content);
        }

        public static IActionResult CreatedActionResult(this ControllerBase controller, string uri)
        {
            var correlationId = controller.Request.CorrelationId();
            var content = new Content<Meta>(new Meta(HttpStatusCode.Created, correlationId));

            return new CreatedResult(uri, content);
        }

        public static IActionResult CreatedActionResult<TData>(this ControllerBase controller, string uri, TData data)
        {
            var correlationId = controller.Request.CorrelationId();
            var content = new Content<Meta, TData>(new Meta(HttpStatusCode.Created, correlationId), data);

            return new CreatedResult(uri, content);
        }
    }
}
