using HackerNews.Stories.Models;
using HackerNews.Stories.Services.Interfaces;
using HackerNews.Stories.Services.Services;
using HackerNews.Stories.Services.Utils;
using MemoryCache.Testing.Moq;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HackerNews.Stories.Tests
{
    [TestClass]
    public class StoryServiceTests
    {
        private Mock<IHttpService> _httpServiceMock;
        private Mock<IMemoryCache> _memoryCacheMock;
        private Mock<ILogger<StoryService>> _loggerMock;
        private StoryService _storyService;

        [TestInitialize]
        public void Initialize()
        {
            _httpServiceMock = new Mock<IHttpService>();
            _memoryCacheMock = new Mock<IMemoryCache>();
            _loggerMock = new Mock<ILogger<StoryService>>();

        }

        [TestMethod]
        public async Task GetAllStories_CacheHit_ReturnsCachedData()
        {
            // Arrange
            var cachedStories = new List<Story>
        {
            new Story { Id = 1, Title = "Story 1" },
            new Story { Id = 2, Title = "Story 2" }
        };

            var mockedCacheMemory = Create.MockedMemoryCache();
            mockedCacheMemory.Set(CommonHelper.MEMORY_CACHE_KEY, cachedStories, new MemoryCacheEntryOptions()
                             .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
                             .SetPriority(CacheItemPriority.Normal));

            _storyService = new StoryService(_httpServiceMock.Object, mockedCacheMemory, _loggerMock.Object);

            // Act
            var result = await _storyService.GetAllStories();

            // Assert
            CollectionAssert.AreEqual(cachedStories, result.ToList());
            _httpServiceMock.Verify(x => x.GetData<List<int>>(CommonHelper.NEWS_STORY_BASE_URL), Times.Never);
            _httpServiceMock.Verify(x => x.GetData<Story>(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public async Task GetAllStories_CacheMiss_ReturnsFetchedDataAndCaches()
        {
            // Arrange
            var storyIds = new List<int> { 1, 2, 3 };
            var expectedStories = new List<Story>
        {
            new Story { Id = 1, Title = "Story 1", Url = "http://test-api.com/story1" },
            new Story { Id = 2, Title = "Story 2", Url = "http://test-api.com/story2" },
            new Story { Id = 3, Title = "Story 3", Url = "http://test-api.com/story3" }
        };


            _httpServiceMock.Setup(x => x.GetData<List<int>>(CommonHelper.NEWS_STORY_BASE_URL))
                            .ReturnsAsync(storyIds);

            _httpServiceMock.Setup(x => x.GetData<Story>(It.IsAny<string>()))
                .ReturnsAsync((string url) => expectedStories.FirstOrDefault(story => url.EndsWith($"{story.Id}.json?print=pretty")));

            _memoryCacheMock.Setup(x => x.CreateEntry(It.IsAny<object>()))
                            .Returns(Mock.Of<ICacheEntry>);

            var mockedCacheMemory = Create.MockedMemoryCache();
            _storyService = new StoryService(_httpServiceMock.Object, mockedCacheMemory, _loggerMock.Object);


            // Act
            var actualStories = await _storyService.GetAllStories();

            // Assert
            CollectionAssert.AreEqual(expectedStories.Where(st => st.Url != null).ToList(), actualStories.ToList());
            _httpServiceMock.Verify(x => x.GetData<List<int>>(CommonHelper.NEWS_STORY_BASE_URL), Times.Once);
            _httpServiceMock.Verify(x => x.GetData<Story>(It.IsAny<string>()), Times.Exactly(storyIds.Count));
        }
    }
}
