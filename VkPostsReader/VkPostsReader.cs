using VkNet.Model;
using VkNet;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;

namespace VkPostReader.VkPostsReader
{
    public class VkPostsReader : IVkPostsReader
    {
        private readonly IConfiguration configuration;
        private readonly VkApi api = new();

        public VkPostsReader(IConfiguration conf)
        {
            configuration = conf;

            api.Authorize(new ApiAuthParams
            {
                AccessToken = configuration["VkAccessToken"]
            });
        }

        /// <summary>
        /// Возвращает текстовое содержимое последних постов.
        /// При этом ownerId должен быть отрицательным, если это идентификатор группы, а не личной страницы.
        /// </summary>
        /// <param name="ownerId">ID владельца стены: группа или личная страница.</param>
        /// <param name="postsNumber">Количество постов.</param>
        public List<string> GetLastWallPosts(long ownerId, ulong postsNumber = 5)
        {
            var posts = new List<string>();
            var wallGetParams = new WallGetParams
            {
                OwnerId = ownerId,
                Count = postsNumber
            };

            var wallPosts = api.Wall.Get(wallGetParams);
            posts = wallPosts.WallPosts.Select(post => post.Text).ToList();
            return posts;
        }

        /// <summary>
        /// Возвращает id последних постов.
        /// При этом ownerId должен быть отрицательным, если это идентификатор группы, а не личной страницы.
        /// </summary>
        /// <param name="ownerId">ID владельца стены: группа или личная страница.</param>
        /// <param name="postsNumber">Количество постов.</param>
        public List<long> GetLastWallPostsId(long ownerId, ulong postsNumber = 5)
        {
            var posts = new List<long>();
            var wallGetParams = new WallGetParams
            {
                OwnerId = ownerId,
                Count = postsNumber
            };

            var wallPosts = api.Wall.Get(wallGetParams);
            posts = wallPosts.WallPosts.Select(post => post.Id!.Value).ToList();
            return posts;
        }

        /// <summary>
        /// Возвращает текст из поста.
        /// При этом ownerId должен быть отрицательным, если это идентификатор группы, а не личной страницы.
        /// </summary>
        /// <param name="ownerId">ID владельца стены: группа или личная страница.</param>
        /// <param name="postId">ID поста.</param>
        public string GetWallPost(long ownerId, long postId)
        {
            var post = api.Wall.GetById(new[] { $"{ownerId}_{postId}" })[0];
            return post.Text;
        }
    }
}
