using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

using TwilightSparkle.Forum.DatabaseSeed;
using TwilightSparkle.Forum.Middlewares;
using TwilightSparkle.Forum.Repository.DbContexts;

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




            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "API",
                    Description = "Easy Money Domain API"
                });
            });

            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            });


            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => { options.LoginPath = new PathString("/Home/Login"); });
            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DatabaseContext appContext)
        {
            DatabaseMigrationSeed.SeedMigrateDatabase(appContext);
            app.UseDeveloperExceptionPage();

            app.Use((context, next) => { context.Request.Scheme = "https"; return next(); });
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMiddleware<ErrorLoggerMiddleware>();

            app.UseRouting();

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
