using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using TwilightSparkle.Common.Hasher;
using TwilightSparkle.Forum.Configurations;
using TwilightSparkle.Forum.Foundation.Authentication;
using TwilightSparkle.Forum.Foundation.ImageStorage;
using TwilightSparkle.Forum.Foundation.SectionsService;
using TwilightSparkle.Forum.Foundation.ThreadsService;
using TwilightSparkle.Forum.Foundation.UsersInfo;
using TwilightSparkle.Forum.Repository.DbContexts;
using TwilightSparkle.Forum.Repository.Interfaces;
using TwilightSparkle.Forum.Repository.UnitOfWork;

namespace TwilightSparkle.Forum
{
    public static class AppServiceCollectionExtensions
    {
        public static IServiceCollection AddSqlDatabase(this IServiceCollection services,
            string connectionString)
        {
            services.AddDbContext<DatabaseContext>(options => options
                .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()))
                .UseSqlServer(connectionString));
            services.AddScoped<DbContext, DatabaseContext>();

            services.AddScoped<IForumUnitOfWork, ForumUnitOfWork>();

            return services;
        }

        public static IServiceCollection AddCommon(this IServiceCollection services)
        {
            services.AddSingleton<IHasher, Sha256>();

            return services;
        }

        public static IServiceCollection AddFirebaseImageServices(this IServiceCollection services,
            IConfigurationSection imageStorageConfigurationSection,
            IConfigurationSection firebaseConfiguration)
        {
            services.Configure<ImageStorageConfiguration>(imageStorageConfigurationSection);
            services.AddSingleton<IImageStorageConfiguration>(provider => provider.GetService<IOptions<ImageStorageConfiguration>>().Value);

            services.Configure<FirebaseConfiguration>(firebaseConfiguration);
            services.AddSingleton<IFirebaseImageStorageConfiguration>(provider => provider.GetService<IOptions<FirebaseConfiguration>>().Value);

            services.AddScoped<IImageStorageService, FirebaseImageStorageService>();

            return services;
        }

        public static IServiceCollection AddAuthenticationServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthenticationService, AuthenticationService>();

            return services;
        }

        public static IServiceCollection AddUsersInfoServices(this IServiceCollection services)
        {
            services.AddScoped<IUserInfoService, UserInfoService>();

            return services;
        }

        public static IServiceCollection AddThreadsSectionsManagementServices(this IServiceCollection services)
        {
            services.AddScoped<IThreadsManagementService, ThreadsManagementService>();
            services.AddScoped<ISectionsManagementService, SectionsManagementService>();

            return services;
        }
    }
}
