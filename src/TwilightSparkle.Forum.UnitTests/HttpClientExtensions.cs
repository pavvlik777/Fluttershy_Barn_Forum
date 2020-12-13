using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace TwilightSparkle.Forum.UnitTests
{
    public static class HttpClientExtensions
    {
        public static async Task<HttpResponseMessage> and_get(this HttpClient httpClient, string uri)
        {
            return await httpClient.GetAsync($"http://localhost/{uri}");
        }

        public static async Task<HttpResponseMessage> and_post(this HttpClient httpClient, string uri, object payload)
        {
            var content = JsonConvert.SerializeObject(payload);
            var buffer = System.Text.Encoding.UTF8.GetBytes(content);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var message = new HttpRequestMessage
            {
                RequestUri = new Uri($"http://localhost/{uri}"),
                Method = HttpMethod.Post,
                Content = byteContent,
            };

            return await httpClient.SendAsync(message);
        }
    }
}
