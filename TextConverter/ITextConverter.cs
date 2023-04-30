namespace VkPostReader.TextParser
{
    public interface ITextConverter
    {
        Dictionary<char, int> GetCharsFrequency(string postText);
    }
}
