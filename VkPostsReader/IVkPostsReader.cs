namespace VkPostReader.VkPostsReader
{
    public interface IVkPostsReader
    {
        string GetWallPost(long ownerId, long postId);
        List<string> GetLastWallPosts(long ownerId, ulong postsNumber = 5);
    }
}
