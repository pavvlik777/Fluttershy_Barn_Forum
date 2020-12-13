using System;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Moq;

using TwilightSparkle.Common.Hasher;
using TwilightSparkle.Forum.Repository.DbContexts;
using TwilightSparkle.Forum.Repository.Interfaces;

namespace TwilightSparkle.Forum.UnitTests
{
    public static class connect_to
    {
        public class ConnectionConfig
        {
            public IForumUnitOfWork UnitOfWork { get; set; } = Mock.Of<IForumUnitOfWork>();

            public IHasher Hasher { get; set; } = new Sha256();
        }


        public static ConnectionConfig easy_money_domain_api_()
        {
            var config = new ConnectionConfig();

            return config;
        }

        public static ConnectionConfig with_hasher(this ConnectionConfig config, IHasher hasher)
        {
            config.Hasher = hasher;

            return config;
        }

        public static ConnectionConfig with_unit_of_work(this ConnectionConfig config, IForumUnitOfWork unitOfWork)
        {
            config.UnitOfWork = unitOfWork;

            return config;
        }


        public static async Task<HttpResponseMessage> execute(this ConnectionConfig connectionConfig, Func<HttpClient, Task<HttpResponseMessage>> action)
        {
            var hostBuilder = new HostBuilder()
                .ConfigureWebHostDefaults(c =>
                {
                    c.UseEnvironment("Testing");
                    c.ConfigureAppConfiguration(x => x.AddJsonFile("appsettings.json"));
                    c.UseTestServer();
                    c.UseStartup<TestStartup>();

                    c.ConfigureTestServices(sc =>
                    {
                        sc.RemoveAll(typeof(IForumUnitOfWork));
                        sc.RemoveAll(typeof(IHasher));

                        sc.RemoveAll(typeof(ILogger));
                        sc.AddSingleton(Mock.Of<ILogger>());

                        var options = new DbContextOptionsBuilder<DatabaseContext>()
                            .UseInMemoryDatabase(databaseName: "TMP")
                            .Options;

                        var ctx = new DatabaseContext(options);

                        sc.RemoveAll(typeof(DbContext));
                        sc.AddSingleton<DbContext>(ctx);
                        sc.AddSingleton<DatabaseContext>(ctx);

                        sc.AddSingleton(connectionConfig.Hasher);
                        sc.AddSingleton(connectionConfig.UnitOfWork);
                    });
                });

            using var host = await hostBuilder.StartAsync();
            using var client = host.GetTestClient();
            var result = await action(client);

            return result;
        }
    }
}
