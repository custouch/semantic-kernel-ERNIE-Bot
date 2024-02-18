using ERNIE_Bot.SDK;
using Microsoft;
using Microsoft.Extensions.Configuration;

namespace IntegrationTests.SDK
{
    public class ChatCompletionTest
    {
        private ERNIEBotClient _client;

        public ChatCompletionTest()
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

        #region ChatCompletion

        private async Task<string> InternalChatAsync(ModelEndpoint endpoint)
        {
            var response = await _client.ChatAsync(new ERNIE_Bot.SDK.Models.ChatRequest()
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
            return response.Result;
        }

        [Theory]
        [ClassData(typeof(ChatCompletionTestEndpoints))]
        public async Task ChatAsync(ModelEndpoint endpoint)
        {
            var result = await InternalChatAsync(endpoint);

            Assert.NotNull(result);
        }

        #endregion ChatCompletion
    }
}
