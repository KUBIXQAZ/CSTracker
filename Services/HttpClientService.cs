using System.Net.Http;
using System.Net.Http.Headers;

namespace CSTracker.Services
{
    internal class HttpClientService
    {
        public HttpClient CreateHttpClient(string baseUrl, string apiKey = null, string bearerToken = null)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);
            if (apiKey != null) client.DefaultRequestHeaders.Add("X-Api-Key", apiKey);
            if (bearerToken != null) client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

            return client;
        }
    }
}