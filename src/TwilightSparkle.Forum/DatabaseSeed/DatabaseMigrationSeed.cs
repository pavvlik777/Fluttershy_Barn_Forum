using System.Collections.Generic;
using System.Linq;

using Microsoft.EntityFrameworkCore;

using TwilightSparkle.Forum.DomainModel.Entities;
using TwilightSparkle.Forum.IdentityServer;
using TwilightSparkle.Forum.Repository.DbContexts;

namespace TwilightSparkle.Forum.DatabaseSeed
{
    public static class DatabaseMigrationSeed
    {
        private static IReadOnlyCollection<string> _sectionNames;


        static DatabaseMigrationSeed()
        {
            var sectionNames = new List<string>
            {
                "News",
                "Arts",
                "Politics",
                "Friendship"
            };
            _sectionNames = sectionNames;
        }


        public static void SeedMigrateDatabase(DatabaseContext appContext)
        {
            appContext.Database.Migrate();
            var defaultProfileImage = appContext.Images.FirstOrDefault(i => i.ExternalId == "default-profile-image");
            if(defaultProfileImage == null)
            {
                defaultProfileImage = new UploadedImage
                {
                    ExternalId = "default-profile-image",
                    MediaType = "image/png",
                    FilePath = "default-profile-image.png"
                };
                appContext.Images.Add(defaultProfileImage);
                appContext.SaveChanges();
            }

            foreach(var sectionName in _sectionNames)
            {
                var section = appContext.Sections.FirstOrDefault(s => s.Name == sectionName);
                if(section != null)
                {
                    continue;
                }

                section = new Section
                {
                    Name = sectionName
                };
                appContext.Sections.Add(section);
                appContext.SaveChanges();
            }

            var clients = appContext.Clients;
            foreach (var client in Config.GetClients())
            {
                clients.Add(client);
                appContext.SaveChanges();
            }

            var identityResources = appContext.IdentityResources;
            foreach (var identityResource in Config.GetIdentityResources())
            {
                identityResources.Add(identityResource);
                appContext.SaveChanges();
            }

            var apiResources = appContext.ApiResources;
            foreach (var apiResource in Config.GetApiResources())
            {
                apiResources.Add(apiResource);
                appContext.SaveChanges();
            }

            var apiScopes = appContext.ApiScopes;
            foreach (var apiScope in Config.GetApiScopes())
            {
                apiScopes.Add(apiScope);
                appContext.SaveChanges();
            }
        }
    }
}