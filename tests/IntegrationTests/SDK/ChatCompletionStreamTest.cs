using ERNIE_Bot.SDK;
using Microsoft;
using Microsoft.Extensions.Configuration;

namespace IntegrationTests.SDK
{
    public class ChatCompletionStreamTest
    {
        private ERNIEBotClient _client;

        public ChatCompletionStreamTest()
        {
            var config = new ConfigurationBuilder()
                .AddUserSecrets(GetType().Assembly)
                .Build();

            var clientId = config["ClientId"];
            var clientSecret = config["ClientSecret"];

            Requires.NotNullOrEmpty(clientId!);
            Requires.NotNullOrEmpty(clientSecret!);

            _client = new ERNIEBotClient(clientId, clientSecret);
        }

        #region StreamChatCompletion

        private async IAsyncEnumerable<string> InternalStreamChatAsync(ModelEndpoint endpoint)
        {
            var response = _client.ChatStreamAsync(new ERNIE_Bot.SDK.Models.ChatRequest()
            {
                Messages = new List<ERNIE_Bot.SDK.Models.Message>()
               {
                    new ERNIE_Bot.SDK.Models.Message()
                    {
                         Role = ERNIE_Bot.SDK.Models.MessageRole.User,
                         Content = "Hello"
                    }
               }
            }, endpoint);

            await foreach (var r in response)
            {
                yield return r.Result;
            }
        }

        [Theory]
        [ClassData(typeof(ChatCompletionTestEndpoints))]
        public async Task StreamChatAsync(ModelEndpoint endpoint)
        {
            await foreach (var r in InternalStreamChatAsync(endpoint))
            {
                Assert.NotNull(r);
                break;
            }
        }
        #endregion StreamChatCompletion
    }
}
