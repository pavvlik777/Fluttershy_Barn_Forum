using System.Net.Http;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace TwilightSparkle.Forum.UnitTests
{
    public static class HttpResponseMessageExtensions
    {
        public static async Task<T> deserialize_content_as_<T>(this HttpResponseMessage resultMessage)
        {
            var content = await resultMessage.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(content);
        }
    }
}
