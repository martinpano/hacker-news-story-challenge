using HackerNews.Stories.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace HackerNews.Stories.Services.Services
{
    public class HttpService : IHttpService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<HttpService> _logger;

        public HttpService(IHttpClientFactory httpClientFactory, ILogger<HttpService> logger)
        {
            _httpClient = httpClientFactory.CreateClient("HackerNewsClient");
            _logger = logger;
        }


        public async Task<TResponse> GetData<TResponse>(string url) where TResponse : class
        {
            try
            {
                HttpResponseMessage response;

                response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    TResponse result = JsonConvert.DeserializeObject<TResponse>(responseBody);
                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"The request failed. Message: { ex.Message }");
            }
            return null;
        }
    }
}
