using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using VkNet.Model.Attachments;
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

        private readonly JsonSerializerSettings jsonSerializerSettings =
            new JsonSerializerSettings
        {
            Formatting = Formatting.None,
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
        async public Task<IActionResult> GetPostCharsFrequency(long ownerId, long postId)
        {
            try
            {
                logger.LogInformation(
                    $"Analyzing post: https://vk.com/wall{ownerId}_{postId}");

                var post = await vkPostsReader.GetWallPostAsync(ownerId, postId);
                var result = textConverter.GetCharsFrequency(post.Text);

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
                await ctx.SaveChangesAsync();

                logger.LogInformation($"Analyzing complete");

                return Ok(json);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                return StatusCode(500, "Произошла ошибка при обработке запроса.");
            }
        }

        [HttpGet("getLastPostsStats")]
        async public Task<IActionResult> GetLastPostsCharsFrequency(
            long ownerId, 
            ulong postsNumber = 5)
        {
            if (postsNumber < 1 || postsNumber > 100)
                return StatusCode(400, "Неправильный формат данных.");

            try
            {
                logger.LogInformation(
                    $"Analyzing last {postsNumber} posts: https://vk.com/wall{ownerId}");

                var posts = await vkPostsReader.GetLastWallPostsAsync(ownerId, postsNumber);

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
                await ctx.SaveChangesAsync();

                logger.LogInformation($"Analyzing complete");

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
