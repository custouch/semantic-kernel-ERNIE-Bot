using ERNIE_Bot.SDK;
using Microsoft.Extensions.Configuration;
using Microsoft;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationTests.SDK
{
    public class EmbeddingTest
    {
        private ERNIEBotClient _client;

        public EmbeddingTest()
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

        #region Embedding
        private async Task<List<double>> InternalEmbeddingAsync(ModelEndpoint endpoint)
        {
            var response = await _client.EmbeddingsAsync(new ERNIE_Bot.SDK.Models.EmbeddingsRequest()
            {
                Input = new List<string> { "Hello" }
            }, endpoint);
            return response.Data.First().Embedding;
        }

        [Fact]
        public async Task EmbeddingV1Async()
        {
            var result = await InternalEmbeddingAsync(ModelEndpoints.Embedding_v1);

            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task bge_large_zhAsync()
        {
            var result = await InternalEmbeddingAsync(ModelEndpoints.bge_large_zh);

            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task bge_large_enAsync()
        {
            var result = await InternalEmbeddingAsync(ModelEndpoints.bge_large_en);

            Assert.NotEmpty(result);
        }
        #endregion
    }
}
