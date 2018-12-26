using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AwesomeBooks.Model.DomainEntities.Identity;
using AwesomeBooks.Model.EF;
using Swashbuckle.AspNetCore.Swagger;
using System.Reflection;
using System.IO;
using AwesomeBooks.Model.DomainServices;
using AwesomeBooks.Model.Repositories;
using AwesomeBooks.Api.Utilities;

namespace AwesomeBooks.Api.Extensions
{

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSwashbuckle(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSwaggerGen(setupAction =>
            {
                // TODO: make documentation configurable
                setupAction.SwaggerDoc("v1",
                    new Info
                    {
                        Title = "Awesome Books API",
                        Version = "v1",
                        Description = @"A service for books CRUD",
                        Contact = new Contact
                        {
                            Name = "Hojjat Khodabakhsh",
                            Email = "hodjatkh@gmail.com",
                            Url = "https://hojjatk.com"
                        }
                    });
                var xmlFile = $"{Assembly.GetEntryAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                setupAction.IncludeXmlComments(xmlPath);

                //setupAction.OperationFilter<CorrelationIdOperationFilter>();
            });
            return serviceCollection;
        }

        public static void AddEntityContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<EntityContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
        }

        public static void AddAspNetIdentity(this IServiceCollection services)
        {
            services.AddIdentity<UserEntity, RoleEntity>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 8;
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
            })
            .AddEntityFrameworkStores<EntityContext>()
            .AddDefaultTokenProviders();
        }

        public static void ConfigureAppCookies(this IServiceCollection services)
        {
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.Expiration = TimeSpan.FromDays(150);
                options.LoginPath = "/Login"; // If the LoginPath is not set here, ASP.NET Core will default to /Account/Login
                options.LogoutPath = "/Logout"; // If the LogoutPath is not set here, ASP.NET Core will default to /Account/Logout
                options.AccessDeniedPath = "/AccessDenied"; // If the AccessDeniedPath is not set here, ASP.NET Core will default to /Account/AccessDenied
                options.SlidingExpiration = true;
            });
        }

        public static void AddDomainServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
            services.AddScoped<ICategoryGroupService, CategoryGroupService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IEntityImporter, EntityImporter>();

            services.AddSingleton<IFileUploadUtility, FileUploadUtility>();
            services.AddSingleton<ICsvRecordReader, CsvRecordReader>();
        }
    }
}
