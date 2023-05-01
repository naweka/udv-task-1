using FluentAssertions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkNet.Model.Attachments;
using VkPostReader.TextParser;
using VkPostReader.VkPostsReader;

namespace Tests
{
    public class TextConverterTests
    {
        private ITextConverter textConverter;

        [SetUp]
        public void Setup()
        {
            textConverter = new TextConverter();
        }

        [Test]
        [TestCase("Hello World", 'o', 2)]
        [TestCase("Bears is awesome!", 'e', 3)]
        [TestCase("1234567890", '5', 1)]
        [TestCase("", 'a', 0)]
        public void GetCharsFrequency_ReturnsCorrectCharFrequency(string input, char targetChar, int expectedFrequency)
        {
            var result = textConverter.GetCharsFrequency(input);

            if (input.Contains(targetChar))
                result.Should().ContainKey(targetChar)
                    .And.Subject[targetChar].Should().Be(expectedFrequency);
            else
                result.Should().NotContainKey(targetChar);
        }
    }
}
