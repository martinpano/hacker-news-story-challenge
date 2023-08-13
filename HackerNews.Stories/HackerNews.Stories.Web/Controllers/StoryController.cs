using HackerNews.Stories.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HackerNews.Stories.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoryController : ControllerBase
    {
        private readonly IStoryService _storyService;
        private readonly ILogger<StoryController> _logger;
        public StoryController(IStoryService storyService, ILogger<StoryController> logger) 
        {
            _storyService = storyService;
            _logger = logger;
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var stories = await _storyService.GetAllStories();
                if(stories.Any())
                {
                    return Ok(stories);
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fetch of story data failed! Message: {ex.Message}");
            }
            return BadRequest("Something went wrong, please try again later.");
        }
    }
}
