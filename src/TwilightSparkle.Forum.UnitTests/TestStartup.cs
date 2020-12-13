using System;
using System.Reflection;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

using TwilightSparkle.Forum.Configurations;
using TwilightSparkle.Forum.IdentityServer;
using TwilightSparkle.Forum.Middlewares;
using TwilightSparkle.Forum.Repository.DbContexts;

namespace TwilightSparkle.Forum.UnitTests
{
    public class TestStartup
    {
        public IConfiguration Configuration { get; }


        public TestStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public void ConfigureServices(IServiceCollection services)
        {
            var authOptions = new AuthOptions
            {
                Authority = Configuration[$"AuthOptions:{nameof(AuthOptions.Authority)}"],
                Audience = Configuration[$"AuthOptions:{nameof(AuthOptions.Audience)}"]
            };
            services.AddIdentityServer(options =>
            {
                options.IssuerUri = authOptions.Authority;
            })
                .AddDeveloperSigningCredential()
                .AddRequiredServices()
                .AddRepositories()
                .AddClients()
                .AddIdentityApiResources()
                .AddUsers();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = authOptions.Authority;

                    options.Audience = authOptions.Audience;

                    options.TokenValidationParameters.ClockSkew = TimeSpan.Zero;
                    options.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };
                });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "API",
                    Description = "Forum API"
                });
            });

            var assembly = typeof(Startup).GetTypeInfo().Assembly;
            services.AddControllers().AddApplicationPart(assembly).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DatabaseContext appContext)
        {
            app.UseDeveloperExceptionPage();

            app.Use((context, next) => { context.Request.Scheme = "https"; return next(); });
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMiddleware<ErrorLoggerMiddleware>();

            app.UseRouting();

            app.UseIdentityServer();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseMiddleware<SwaggerAuthorizedMiddleware>();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "API",
                    pattern: "api/{controller=Home}/{action=Index}");
            });
        }
    }
}
