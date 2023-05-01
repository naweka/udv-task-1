using VkNet.Model.Attachments;

namespace VkPostReader.VkPostsReader
{
    public interface IVkPostsReader
    {
        Task<Post> GetWallPostAsync(long ownerId, long postId);
        Task<List<Post>> GetLastWallPostsAsync(long ownerId, ulong postsNumber = 5);
    }
}
