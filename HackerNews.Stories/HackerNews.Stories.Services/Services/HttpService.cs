using HackerNews.Stories.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HackerNews.Stories.Services.Services
{
    public class HttpService : IHttpService
    {
        private readonly HttpClient _httpClient;

        public HttpService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("HackerNewsClient");
        }


        public async Task<TResponse> GetData<TResponse>(string url) where TResponse : class
        {
            HttpResponseMessage response;

            response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                TResponse result = JsonConvert.DeserializeObject<TResponse>(responseBody);
                return result;
            }
            else
            {
                // Handle error cases
                throw new HttpRequestException($"Request failed with status code: {response.StatusCode}");
            }
        }
    }
}
