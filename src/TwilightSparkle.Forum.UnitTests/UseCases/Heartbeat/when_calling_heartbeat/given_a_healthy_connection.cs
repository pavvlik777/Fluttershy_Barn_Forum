using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using FluentAssertions;

using TwilightSparkle.Forum.UnitTests;

using Xunit;

namespace Heartbeat.when_calling_heartbeat
{
    public class given_a_healthy_connection : BddAsyncTest
    {
        private HttpResponseMessage _result;

        protected override async Task Setup()
        {
            _result = await connect_to.easy_money_domain_api_()
                .execute(client => client.and_get("/api/heartbeat/date"));
        }

        [Fact]
        public void then_returns_ok() => _result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
