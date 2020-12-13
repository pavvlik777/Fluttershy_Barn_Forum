using IdentityServer4.Services;
using IdentityServer4.Validation;

using Microsoft.Extensions.DependencyInjection;

using TwilightSparkle.Common.Hasher;
using TwilightSparkle.Forum.DomainModel.Entities;
using TwilightSparkle.Forum.DomainModel.IdentityServer4;
using TwilightSparkle.Repository.Implementations;
using TwilightSparkle.Repository.Interfaces;

namespace TwilightSparkle.Forum.IdentityServer
{
    public static class IdentityServerBuilderExtensions
    {
        public static IIdentityServerBuilder AddRequiredServices(this IIdentityServerBuilder builder)
        {
            builder.Services.AddSingleton<IHasher, Sha256>();

            return builder;
        }

        public static IIdentityServerBuilder AddRepositories(this IIdentityServerBuilder builder)
        {
            builder.Services.AddScoped<IRepository<User>, Repository<User>>();
            builder.Services.AddScoped<IRepository<Client>, Repository<Client>>();
            builder.Services.AddScoped<IRepository<ApiResource>, Repository<ApiResource>>();
            builder.Services.AddScoped<IRepository<ApiScope>, Repository<ApiScope>>();
            builder.Services.AddScoped<IRepository<IdentityResource>, Repository<IdentityResource>>();

            return builder;
        }

        public static IIdentityServerBuilder AddClients(this IIdentityServerBuilder builder)
        {
            builder.AddClientStore<CustomClientStore>();
            builder.AddCorsPolicyService<InMemoryCorsPolicyService>();

            return builder;
        }

        public static IIdentityServerBuilder AddIdentityApiResources(this IIdentityServerBuilder builder)
        {
            builder.AddResourceStore<CustomResourceStore>();

            return builder;
        }

        public static IIdentityServerBuilder AddUsers(this IIdentityServerBuilder builder)
        {
            builder.Services.AddScoped<IResourceOwnerPasswordValidator, CustomResourceOwnerPasswordValidator>();
            builder.Services.AddScoped<IProfileService, CustomProfileService>();

            return builder;
        }
    }
}
