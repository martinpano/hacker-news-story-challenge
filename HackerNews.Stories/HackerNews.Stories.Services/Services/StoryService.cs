using HackerNews.Stories.Models;
using HackerNews.Stories.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace HackerNews.Stories.Services.Services
{
    public class StoryService : IStoryService
    {
        private readonly IHttpService _httpService;
        private readonly IMemoryCache _memoryCache;
        private const string memoryCacheKey = "CachedStories";


        public StoryService(IHttpService httpService, IMemoryCache memoryCache)
        {
            _httpService = httpService;
            _memoryCache = memoryCache;
        }

        public async Task<IEnumerable<Story>> GetAllStories()
        {
            try
            {
                if (!_memoryCache.TryGetValue(memoryCacheKey, out List<Story>? newsStories))
                {
                    List<int> storyIds = await _httpService.GetData<List<int>>("https://hacker-news.firebaseio.com/v0/newstories.json");
                    if (storyIds.Count > 0)
                    {
                        var allStoriesTasks = storyIds.Select(x => _httpService.GetData<Story>($"https://hacker-news.firebaseio.com/v0/item/{x}.json?print=pretty"));
                        newsStories = (await Task.WhenAll(allStoriesTasks)).Where(st => st.Url != null).ToList();

                        var cacheConfig = new MemoryCacheEntryOptions()
                            .SetAbsoluteExpiration(TimeSpan.FromMinutes(1))
                            .SetPriority(CacheItemPriority.Normal);
                        

                        _memoryCache.Set(memoryCacheKey, newsStories, cacheConfig);
                    }
                }
                return newsStories;
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
