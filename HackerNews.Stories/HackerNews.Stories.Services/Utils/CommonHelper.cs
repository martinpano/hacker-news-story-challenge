using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace HackerNews.Stories.Services.Utils
{
    public static class CommonHelper
    {
        public const string MEMORY_CACHE_KEY = "CachedStories";
        public const string NEWS_STORY_BASE_URL = "https://hacker-news.firebaseio.com/v0/newstories.json";
        public const string SINGLE_STORY_URL = "https://hacker-news.firebaseio.com/v0/item";
    }
}
