using HackerNews.Stories.Models;
using HackerNews.Stories.Services.Interfaces;

namespace HackerNews.Stories.Services.Services
{
    public class StoryService : IStoryService
    {
        private readonly IHttpService _httpService;

        public StoryService(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<IEnumerable<Story>> GetAllStories()
        {
            List<int> storyIds = new();
            try
            {
                storyIds = await _httpService.GetData<List<int>>("https://hacker-news.firebaseio.com/v0/newstories.json");
                if(storyIds.Count > 0 )
                {
                    var allStories = storyIds.Select(x => _httpService.GetData<Story>($"https://hacker-news.firebaseio.com/v0/item/{x}.json?print=pretty"));
                    var test = await Task.WhenAll(allStories);
                    return test;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new List<Story>();
        }
    }
}
