namespace VkPostReader.TextParser
{
    public class TextConverter : ITextConverter
    {
        public Dictionary<char, int> GetCharsFrequency(string text)
        {
            text = text.ToLower();

            Dictionary<char, int> charFrequency = new();

            foreach (char c in text)
            {
                if (charFrequency.ContainsKey(c))
                    charFrequency[c]++;
                else
                    charFrequency.Add(c, 1);
            }

            return charFrequency;
        }

    }
}
