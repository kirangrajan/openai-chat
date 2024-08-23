using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text.Json;

namespace OpenApi.Services
{
    public class BingSearchPlugin
    {
        private readonly string _subscriptionKey;
        private readonly HttpClient _httpClient;

        public BingSearchPlugin(string subscriptionKey)
        {
            _subscriptionKey = subscriptionKey;
            _httpClient = new HttpClient();
        }

        public async Task<string> SearchAsync(string query)
        {
            var uri = $"https://api.cognitive.microsoft.com/bing/v7.0/search?q={Uri.EscapeDataString(query)}";

            _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _subscriptionKey);
            _httpClient.DefaultRequestHeaders.Add("Content-Type", "application/json; charset=utf-8");
            var response = await _httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<BingSearchResult>(json);
                return string.Join("\n", result.WebPages.Value.Select(page => page.Name + ": " + page.Snippet));
            }
            else
            {
                return $"Error: {response.StatusCode}";
            }
        }
    }

    public class BingSearchResult
    {
        public WebPages WebPages { get; set; }
    }

    public class WebPages
    {
        public WebPage[] Value { get; set; }
    }

    public class WebPage
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Snippet { get; set; }
    }
}
