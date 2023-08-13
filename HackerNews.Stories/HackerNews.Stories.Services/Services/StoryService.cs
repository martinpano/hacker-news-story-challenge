using HackerNews.Stories.Models;
using HackerNews.Stories.Services.Interfaces;
using HackerNews.Stories.Services.Utils;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HackerNews.Stories.Services.Services
{
    public class StoryService : IStoryService
    {
        private readonly IHttpService _httpService;
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<StoryService> _logger;

        

        public StoryService(IHttpService httpService, 
                            IMemoryCache memoryCache,
                            ILogger<StoryService> logger)
        {
            _httpService = httpService;
            _memoryCache = memoryCache;
            _logger = logger;
        }

        public async Task<IEnumerable<Story>> GetAllStories()
        {
            try
            {
                if (!_memoryCache.TryGetValue(CommonHelper.MEMORY_CACHE_KEY, out List<Story>? newsStories))
                {
                    List<int> storyIds = await _httpService.GetData<List<int>>(CommonHelper.NEWS_STORY_BASE_URL);
                    if (storyIds.Count > 0)
                    {
                        var allStoriesTasks = storyIds.Select(stId => _httpService.GetData<Story>($"{CommonHelper.SINGLE_STORY_URL}/{stId}.json?print=pretty"));
                        newsStories = (await Task.WhenAll(allStoriesTasks)).Where(st => st.Url != null).ToList();

                        var cacheConfig = new MemoryCacheEntryOptions()
                            .SetAbsoluteExpiration(TimeSpan.FromMinutes(1))
                            .SetPriority(CacheItemPriority.Normal);
                        

                        _memoryCache.Set(CommonHelper.MEMORY_CACHE_KEY, newsStories, cacheConfig);
                    }
                }
                return newsStories;
               
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fetch data failed.", ex);
            }
            return new List<Story>();
        }
    }
}
