using System.Net.Http;
using System.Threading.Tasks;
using  Dream_Bridge.Services.Adapters;

namespace Dream_Bridge.Services.Adapters
{
    public class ApiAdapter : IApiAdapter
    {
        private readonly HttpClient _httpClient;

        public ApiAdapter(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetAsync(string url)
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> PostAsync(string url, HttpContent content)
        {
            var response = await _httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
