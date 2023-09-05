using ERNIE_Bot.SDK;
using Microsoft;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


        [Fact]
        public async Task ERNIEBotChatAsync()
        {
            var result = await InternalChatAsync(ModelEndpoints.ERNIE_Bot);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task ERNIEBotTurboChatAsync()
        {
            var result = await InternalChatAsync(ModelEndpoints.ERNIE_Bot_Turbo);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task BLOOMZ7BChatAsync()
        {
            var result = await InternalChatAsync(ModelEndpoints.BLOOMZ_7B);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task Llama_2_7b_chatAsync()
        {
            var result = await InternalChatAsync(ModelEndpoints.Llama_2_7b_chat);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task Llama_2_13b_chatAsync()
        {
            var result = await InternalChatAsync(ModelEndpoints.Llama_2_13b_chat);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task Llama_2_70b_chatAsync()
        {
            var result = await InternalChatAsync(ModelEndpoints.Llama_2_70b_chat);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task Qianfan_BLOOMZ_7B_compressedAsync()
        {
            var result = await InternalChatAsync(ModelEndpoints.Qianfan_BLOOMZ_7B_compressed);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task Qianfan_Chinese_Llama_2_7bAsync()
        {
            var result = await InternalChatAsync(ModelEndpoints.Qianfan_Chinese_Llama_2_7b);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task ChatGLM2_6b_32kAsync()
        {
            var result = await InternalChatAsync(ModelEndpoints.ChatGLM2_6b_32k);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task AquilaChat_7bAsync()
        {
            var result = await InternalChatAsync(ModelEndpoints.AquilaChat_7b);

            Assert.NotNull(result);
        }
        #endregion



    }
}
