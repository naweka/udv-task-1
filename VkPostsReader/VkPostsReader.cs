using VkNet.Model;
using VkNet;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;
using VkNet.Model.GroupUpdate;

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

        public VkPostsReader(string vkToken)
        {
            api.Authorize(new ApiAuthParams
            {
                AccessToken = vkToken
            });
        }

        /// <summary>
        /// Возвращает содержимое последних постов.
        /// При этом ownerId должен быть отрицательным, если это идентификатор группы, а не личной страницы.
        /// </summary>
        /// <param name="ownerId">ID владельца стены: группа или личная страница.</param>
        /// <param name="postsNumber">Количество постов.</param>
        public async Task<List<Post>> GetLastWallPostsAsync(long ownerId, ulong postsNumber = 5)
        {
            var posts = new List<Post>();
            var wallGetParams = new WallGetParams
            {
                OwnerId = ownerId,
                Count = postsNumber
            };

            var wallPosts = await api.Wall.GetAsync(wallGetParams) ?? throw new Exception("");
            
            posts = wallPosts.WallPosts.ToList();
            return posts;
        }

        /// <summary>
        /// Возвращает пост.
        /// При этом ownerId должен быть отрицательным, если это идентификатор группы, а не личной страницы.
        /// </summary>
        /// <param name="ownerId">ID владельца стены: группа или личная страница.</param>
        /// <param name="postId">ID поста.</param>
        public async Task<Post> GetWallPostAsync(long ownerId, long postId)
        {
            var post = await api.Wall.GetByIdAsync(new[] { $"{ownerId}_{postId}" });

            return post[0];
        }
    }
}
