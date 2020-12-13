using System;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

using TwilightSparkle.Forum.Configurations;
using TwilightSparkle.Forum.DatabaseSeed;
using TwilightSparkle.Forum.IdentityServer;
using TwilightSparkle.Forum.Middlewares;
using TwilightSparkle.Forum.Repository.DbContexts;

using Microsoft.Extensions.Hosting;
using VueCliMiddleware;
using Microsoft.IdentityModel.Logging;

namespace TwilightSparkle.Forum
{
    public class Startup
    {
        public IConfiguration Configuration { get; }


        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddSqlDatabase(connectionString);

            services.AddCommon();

            services.AddAuthenticationServices();

            services.AddUsersInfoServices();

            services.AddThreadsSectionsManagementServices();

            var imageStorageConfigurationSection = Configuration.GetSection("ImageStorage");
            var firebaseImageStorageConfigurationSection = Configuration.GetSection("FirebaseImageStorage");
            services.AddFirebaseImageServices(imageStorageConfigurationSection, firebaseImageStorageConfigurationSection);

            services.AddSingleton(Configuration);

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

            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            });

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DatabaseContext appContext)
        {
            DatabaseMigrationSeed.SeedMigrateDatabase(appContext);
            app.UseDeveloperExceptionPage();

            app.Use((context, next) => { context.Request.Scheme = "https"; return next(); });
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();
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

            if (env.IsStaging())
            {
                IdentityModelEventSource.ShowPII = true;
            }

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "API",
                    pattern: "api/{controller=Home}/{action=Index}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsStaging())
                {
                    spa.UseVueCli(npmScript: "serve", port: 8080);
                }
            });
        }
    }
}
