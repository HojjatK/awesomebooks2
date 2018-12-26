using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using AwesomeBooks.Model.EF;
using Microsoft.EntityFrameworkCore;
using AwesomeBooks.Model.DomainEntities.Identity;
using Microsoft.AspNetCore.Identity;
using AwesomeBooks.Api.Extensions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Serialization;

namespace AwesomeBooks.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddEntityContext(Configuration);
            services.AddAspNetIdentity();
            services.ConfigureAppCookies();
            services.AddDomainServices();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSwashbuckle();

            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //        .AddJwtBearer(options =>
            //        {
            //            options.Authority = Configuration.GetValue<string>(ApplicationSettings.IdentityAuthorityKey);
            //            options.Audience = Configuration.GetValue<string>(ApplicationSettings.IdentityAudienceKey);
            //            options.RequireHttpsMetadata = false;
            //        });
            services.AddLogging();
            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                };
            });
            services.AddCors(options =>
            {
                options.AddPolicy("All", builder =>
                    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().AllowCredentials());

                //var corsOrigins = Configuration.GetValue<string>("CorsOrigins");
                //if (!string.IsNullOrWhiteSpace(corsOrigins))
                //{
                //    options.AddPolicy("CORS", builder =>
                //    builder.WithOrigins(corsOrigins.Split(',')).AllowAnyHeader().AllowAnyMethod().AllowCredentials());
                //}
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //app.UseHsts();
            }
            app.UseCors("All");
            //var corsOrigins = Configuration.GetValue<string>("CorsOrigins");
            //if (!string.IsNullOrWhiteSpace(corsOrigins))
            //{
            //    app.UseCors("CORS");
            //}
            app.UseRequestResponseLogging();
            app.UseCustomExceptionHandler();
            app.UseSwagger()
                .UseSwaggerUI(setupAction =>
                {
                    setupAction.SwaggerEndpoint("/swagger/v1/swagger.json", /*ApiDocInfo.Title*/ "Awesome Books API");                    
                });
            //app.UseAuthentication();
            app.UseMvc();

            app.InitializeDatabase(env);

        }
    }
}