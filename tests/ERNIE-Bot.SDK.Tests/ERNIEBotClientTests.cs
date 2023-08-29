using Xunit;
using ERNIE_Bot.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERNIE_BOT.SDK.Tests;
using ERNIE_Bot.SDK.Models;

namespace ERNIE_Bot.SDK.Tests
{
    public class ERNIEBotClientTests
    {
        [Fact]
        public void ClientCreateTest()
        {
            var clientId = "test";
            var clientSecret = "test";

            var client = new ERNIEBotClient(clientId, clientSecret);

            Assert.NotNull(client);
        }

        [Fact()]
        public async Task ChatCompletionsAsyncTest()
        {
            var httpClient = await TestHelper.FakeHttpClient("chat_response.txt");
            var fakeTokenStore = new TokenStoreHelper("test_token");
            var client = new ERNIEBotClient("test", "test", httpClient, fakeTokenStore);

            var result = await client.ChatAsync(new ChatCompletionsRequest()
            {
                Messages =
                {
                    new Message()
                    {
                         Role = MessageRole.User,
                         Content = "Hello?"
                    }
                }
            }, ModelEndpoints.ERNIE_Bot);

            Assert.NotEmpty(result.Result);
        }

        [Fact()]
        public async Task ChatCompletionsStreamAsyncTest()
        {
            var httpClient = await TestHelper.FakeHttpClient("chat_stream_response.txt");
            var fakeTokenStore = new TokenStoreHelper("test_token");
            var client = new ERNIEBotClient("test", "test", httpClient, fakeTokenStore);

            var results = client.ChatStreamAsync(new ChatCompletionsRequest()
            {
                Messages =
                {
                    new Message()
                    {
                         Role = MessageRole.User,
                         Content = "Hello?"
                    }
                }
            }, ModelEndpoints.ERNIE_Bot);

            await foreach (var result in results)
            {
                Assert.NotEmpty(result.Result);
            }
        }

        [Fact()]
        public async Task ChatEBInstantAsyncTest()
        {
            var httpClient = await TestHelper.FakeHttpClient("chat_response.txt");
            var fakeTokenStore = new TokenStoreHelper("test_token");
            var client = new ERNIEBotClient("test", "test", httpClient, fakeTokenStore);

            var result = await client.ChatAsync(new ChatRequest()
            {
                Messages =
                {
                    new Message()
                    {
                         Role = MessageRole.User,
                         Content = "Hello?"
                    }
                }
            }, ModelEndpoints.ERNIE_Bot_Turbo);

            Assert.NotEmpty(result.Result);
        }

        [Fact()]
        public async Task ChatEBInstantStreamAsyncTest()
        {
            var httpClient = await TestHelper.FakeHttpClient("chat_stream_response.txt");
            var fakeTokenStore = new TokenStoreHelper("test_token");
            var client = new ERNIEBotClient("test", "test", httpClient, fakeTokenStore);

            var results = client.ChatStreamAsync(new ChatRequest()
            {
                Messages =
                {
                    new Message()
                    {
                         Role = MessageRole.User,
                         Content = "Hello?"
                    }
                }
            }, ModelEndpoints.ERNIE_Bot_Turbo);

            await foreach (var result in results)
            {
                Assert.NotEmpty(result.Result);
            }
        }

        [Fact()]
        public async Task EmbeddingsAsyncTest()
        {
            var httpClient = await TestHelper.FakeHttpClient("embedding_response.txt");
            var fakeTokenStore = new TokenStoreHelper("test_token");
            var client = new ERNIEBotClient("test", "test", httpClient, fakeTokenStore);

            var result = await client.EmbeddingsAsync(new Models.EmbeddingsRequest()
            {
                Input = { "test" }
            });

            Assert.NotEmpty(result.Data);
        }

        [Fact()]
        public async Task GetAccessTokenAsyncTest()
        {
            var httpClient = await TestHelper.FakeHttpClient("token_response.txt");
            var fakeTokenStore = new TokenStoreHelper(null);
            var client = new ERNIEBotClient("test", "test", httpClient, fakeTokenStore);

            var ak = await client.GetAccessTokenAsync();

            Assert.NotNull(ak);
        }

        [Fact]
        public async Task ThrowErrorTest()
        {
            var httpClient = await TestHelper.FakeHttpClient("error.txt");
            var fakeTokenStore = new TokenStoreHelper("test");
            var client = new ERNIEBotClient("test", "test", httpClient, fakeTokenStore);

            await Assert.ThrowsAsync<ERNIEBotException>(async () =>
            {
                await client.EmbeddingsAsync(new EmbeddingsRequest());
            });
        }
    }
}