using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AwesomeBooks.Model.EF;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using AwesomeBooks.Api.Middlewares;

namespace AwesomeBooks.Api.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseRequestResponseLogging(this IApplicationBuilder builder) =>
            builder.UseMiddleware<RequestResponseLogMiddleware>();

        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder) =>
            builder.UseMiddleware<CustomExceptionHandlerMiddleware>();

        public static void InitializeDatabase(this IApplicationBuilder app, IHostingEnvironment env)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var entityContext = scope.ServiceProvider.GetService(typeof(EntityContext)) as EntityContext;
                entityContext.Database.EnsureCreated();
            }
        }
    }
}
