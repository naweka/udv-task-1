using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using VkPostReader.Models;
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
        private readonly DatabaseContext ctx;

        private readonly JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public VkAnalyzerController(
            ILogger<VkAnalyzerController> log,
            ITextConverter conv,
            IVkPostsReader reader,
            DatabaseContext ctx)
        {
            logger = log;
            textConverter = conv;
            vkPostsReader = reader;
            this.ctx = ctx;
        }


        [HttpGet("getPostStats")]
        public IActionResult GetPostCharsFrequency(long ownerId, long postId)
        {
            try
            {
                var result = textConverter.GetCharsFrequency(
                    vkPostsReader.GetWallPost(ownerId, postId).Text);

                var sortedDict = result.OrderBy(x => x.Key);
                var json = JsonConvert.SerializeObject(
                    sortedDict.ToDictionary(x => x.Key, x => x.Value),
                    jsonSerializerSettings);

                var model = new VkPostStatistics()
                {
                    OwnerID = ownerId,
                    PostsID = new[] { postId },
                    StatisticString = json,
                    DateTime = DateTime.UtcNow
                };

                ctx.VkStatisticRecords.Add(model);

                ctx.SaveChanges();

                return Ok(json);
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
                var posts = vkPostsReader.GetLastWallPosts(ownerId, postsNumber);

                var result = textConverter.GetCharsFrequency(
                    posts.Select(p => p.Text).Aggregate((a,b) => a+b));

                var sortedDict = result.OrderBy(x => x.Key);
                var json = JsonConvert.SerializeObject(
                    sortedDict.ToDictionary(x => x.Key, x => x.Value),
                    jsonSerializerSettings);

                var model = new VkPostStatistics()
                {
                    OwnerID = ownerId,
                    PostsID = posts.Select(p => p.Id!.Value).ToArray(),
                    StatisticString = json,
                    DateTime = DateTime.UtcNow
                };

                ctx.VkStatisticRecords.Add(model);

                ctx.SaveChanges();

                return Ok(json);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                return StatusCode(500, "Произошла ошибка при обработке запроса.");
            }
        }
    }
}
