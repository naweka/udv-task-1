using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using NUnit.Framework;
using VkNet.Model.Attachments;
using VkPostReader.VkPostsReader;

namespace VkPostReaderTests
{
    public class VkPostsReaderTests
    {
        private IVkPostsReader postsReader;
        private IConfiguration config;

        [SetUp]
        public void Setup()
        {
            config = new ConfigurationBuilder()
                .AddUserSecrets<VkPostsReaderTests>()
                .Build();

            postsReader = new VkPostsReader(config["VkAccessToken"] 
                ?? throw new NullReferenceException("VkAccessToken"));
        }

        [Test]
        public void ReaderShouldReturnNullWhenIncorrectData()
        {
            Post post = null;

            Action action = () => {
                post = postsReader.GetWallPostAsync(951949494949819, 9846846468684).Result;};

            action.Should().Throw<Exception>();
            post.Should().BeNull();
        }

        [TestCase(-149279263)]
        [TestCase(206433218)]
        public async Task GetLastWallPostsAsyncWorksWithGroupAndUser(long ownerId)
        {
            List<Post> posts = await postsReader.GetLastWallPostsAsync(ownerId, 10);

            posts.Should().NotBeNull();
            posts.Should().HaveCount(10);
        }

        [Test]
        public async Task GetLastWallPostsAsyncReturnsCorrectNumberOfPosts()
        {
            List<Post> posts = await postsReader.GetLastWallPostsAsync(-149279263, 10);

            posts.Should().NotBeNull();
            posts.Should().HaveCount(10);
        }

        [Test]
        public async Task GetWallPostAsyncReturnsCorrectPost()
        {
            Post post = await postsReader.GetWallPostAsync(206433218, 13580);

            post.Should().NotBeNull();
            post.Id.Should().Be(13580);
        }
    }
}