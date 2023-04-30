using VkNet.Model.Attachments;

namespace VkPostReader.VkPostsReader
{
    public interface IVkPostsReader
    {
        Post GetWallPost(long ownerId, long postId);
        List<Post> GetLastWallPosts(long ownerId, ulong postsNumber = 5);
    }
}
