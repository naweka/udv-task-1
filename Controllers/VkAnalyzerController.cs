using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using VkPostReader.TextParser;
using VkPostReader.VkPostsReader;

namespace VkPostReader.Controllers
{
    [ApiController]
    [Route("analyzer")]
    public class VkAnalyzerController : ControllerBase
    {
        private readonly ILogger<VkAnalyzerController> logger;
        private readonly ITextConverter textConverter;
        private readonly IVkPostsReader vkPostsReader;

        public VkAnalyzerController(
            ILogger<VkAnalyzerController> log,
            ITextConverter conv,
            IVkPostsReader reader)
        {
            logger = log;
            textConverter = conv;
            vkPostsReader = reader;
        }


        [HttpGet("getPostStats")]
        public IActionResult GetPostCharsFrequency(long ownerId, long postId)
        {
            try
            {
                var result = textConverter.GetCharsFrequency(vkPostsReader.GetWallPost(ownerId, postId));
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                return StatusCode(500, "Произошла ошибка при обработке запроса.");
            }
        }

        [HttpGet("getLastPostsStats")]
        public IActionResult GetLastPostsCharsFrequency(long ownerId, ulong postsNumber = 5)
        {
            if (postsNumber < 1 || postsNumber > 100)
                return StatusCode(400, "Неправильный формат данных.");
            try
            {
                var result = textConverter.GetCharsFrequency(vkPostsReader.GetLastWallPosts(ownerId, postsNumber).Aggregate((a,b) => a+b));
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                return StatusCode(500, "Произошла ошибка при обработке запроса.");
            }
        }
    }
}
