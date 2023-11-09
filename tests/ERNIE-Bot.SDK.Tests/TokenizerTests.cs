using ERNIE_Bot.SDK;

namespace ERNIE_Bot.SDK.Tests
{
    public class TokenizerTests
    {
        [Fact]
        public void TestApproxNumTokens()
        {
            string text = "����һ�β�������This is a test string.";
            int expected = 14;
            int actual = Tokenizer.ApproxNumTokens(text);
            Assert.Equal(expected, actual);
        }
    }
}
