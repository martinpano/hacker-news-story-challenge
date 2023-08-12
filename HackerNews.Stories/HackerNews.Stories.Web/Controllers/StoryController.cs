using HackerNews.Stories.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HackerNews.Stories.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoryController : ControllerBase
    {
        private readonly IStoryService _storyService;
        public StoryController(IStoryService storyService) 
        {
            _storyService = storyService;
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var stories = await _storyService.GetAllStories();
                return Ok(stories);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
