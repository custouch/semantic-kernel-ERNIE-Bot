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

        [Fact]
        public async Task ERNIEBotStreamChatAsync()
        {
            await foreach (var r in InternalStreamChatAsync(ModelEndpoints.ERNIE_Bot))
            {
                Assert.NotNull(r);
                break;
            }
        }

        [Fact]
        public async Task ERNIEBotTurboStreamChatAsync()
        {
            await foreach (var r in InternalStreamChatAsync(ModelEndpoints.ERNIE_Bot_Turbo))
            {
                Assert.NotNull(r);
                break;
            }
        }

        [Fact]
        public async Task BLOOMZ7BStreamChatAsync()
        {
            await foreach (var r in InternalStreamChatAsync(ModelEndpoints.BLOOMZ_7B))
            {
                Assert.NotNull(r);
                break;
            }
        }

        [Fact]
        public async Task Llama_2_7b_ChatStreamAsync()
        {
            await foreach (var r in InternalStreamChatAsync(ModelEndpoints.Llama_2_7b_chat))
            {
                Assert.NotNull(r);
                break;
            }
        }

        [Fact]
        public async Task Llama_2_13b_ChatStreamAsync()
        {
            await foreach (var r in InternalStreamChatAsync(ModelEndpoints.Llama_2_13b_chat))
            {
                Assert.NotNull(r);
                break;
            }
        }

        [Fact]
        public async Task Llama_2_70b_ChatStreamAsync()
        {
            await foreach (var r in InternalStreamChatAsync(ModelEndpoints.Llama_2_70b_chat))
            {
                Assert.NotNull(r);
                break;
            }
        }

        [Fact]
        public async Task Qianfan_BLOOMZ_7B_compressedStreamAsync()
        {
            await foreach (var r in InternalStreamChatAsync(ModelEndpoints.Qianfan_BLOOMZ_7B_compressed))
            {
                Assert.NotNull(r);
                break;
            }
        }

        [Fact]
        public async Task Qianfan_Chinese_Llama_2_7bStreamAsync()
        {
            await foreach (var r in InternalStreamChatAsync(ModelEndpoints.Qianfan_Chinese_Llama_2_7b))
            {
                Assert.NotNull(r);
                break;
            }
        }

        [Fact]
        public async Task ChatGLM2_6b_32kStreamAsync()
        {
            await foreach (var r in InternalStreamChatAsync(ModelEndpoints.ChatGLM2_6b_32k))
            {
                Assert.NotNull(r);
                break;
            }
        }

        [Fact]
        public async Task AquilaChat_7bStreamAsync()
        {
            await foreach (var r in InternalStreamChatAsync(ModelEndpoints.AquilaChat_7b))
            {
                Assert.NotNull(r);
                break;
            }
        }

        #endregion StreamChatCompletion
    }
}
