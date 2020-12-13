using System.Collections.Generic;
using System.Security.Claims;

using IdentityServer4.Models;

using TwilightSparkle.Forum.IdentityServer.Mappers;

namespace TwilightSparkle.Forum.IdentityServer
{
    public class Config
    {
        public static IEnumerable<DomainModel.IdentityServer4.IdentityResource> GetIdentityResources()
        {
            return new List<DomainModel.IdentityServer4.IdentityResource>
            {
                new IdentityResources.OpenId().ToEntity(),
                new IdentityResources.Profile().ToEntity(),
            };
        }

        public static IEnumerable<DomainModel.IdentityServer4.ApiResource> GetApiResources()
        {
            return new List<DomainModel.IdentityServer4.ApiResource>
            {
                new ApiResource("api", "Forum API")
                {
                    Scopes = new List<string>
                    {
                        "api"
                    },

                    UserClaims = new List<string>
                    {
                        "user_id",
                        ClaimsIdentity.DefaultNameClaimType
                    }
                }.ToEntity()
            };
        }

        public static IEnumerable<DomainModel.IdentityServer4.ApiScope> GetApiScopes()
        {
            return new List<DomainModel.IdentityServer4.ApiScope>
            {
                new ApiScope("api")
                {
                    UserClaims = new List<string>
                    {
                        "user_id",
                        ClaimsIdentity.DefaultNameClaimType
                    }
                }.ToEntity()
            };
        }

        public static IEnumerable<DomainModel.IdentityServer4.Client> GetClients()
        {
            return new List<DomainModel.IdentityServer4.Client>
            {
                new Client
                {
                    ClientId = "ro.client",
                    AllowOfflineAccess = true,
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    AlwaysSendClientClaims = true,
                    AlwaysIncludeUserClaimsInIdToken = true,

                    ClientSecrets =
                    {
                        new Secret("ro.client_secret_password_123".Sha256())
                    },
                    AllowedScopes = { "api" }
                }.ToEntity()
            };
        }
    }
}