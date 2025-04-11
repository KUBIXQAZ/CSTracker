using System.Net.Http;

namespace SteamItemsStatsViewer.Services
{
    internal class HttpClientService
    {
        public HttpClient CreateHttpClient(string baseUrl, string apiKey)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Add("X-Api-Key", apiKey);

            return client;
        }
    }
}