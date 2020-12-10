using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace TwilightSparkle.Firebase.Storage
{
    public static class HttpClientFactory
    {
        /// <summary>
        /// Creates a new <see cref="HttpClient"/> with authentication header when <see cref="FirebaseStorageOptions.AuthTokenAsyncFactory"/> is specified.
        /// </summary>
        /// <param name="options">Firebase storage options.</param>
        public static async Task<HttpClient> CreateHttpClientAsync(this FirebaseStorageOptions options)
        {
            var client = new HttpClient();

            if (options.HttpClientTimeout != default)
            {
                client.Timeout = options.HttpClientTimeout;
            }

            if (options.AuthTokenAsyncFactory == null)
            {
                return client;
            }

            var auth = await options.AuthTokenAsyncFactory().ConfigureAwait(false);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Firebase", auth);

            return client;
        }
    }
}
