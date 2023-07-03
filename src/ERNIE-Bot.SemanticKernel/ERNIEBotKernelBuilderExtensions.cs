using ERNIE_Bot.SDK;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel.AI.ChatCompletion;
using Microsoft.SemanticKernel.AI.Embeddings;
using Microsoft.SemanticKernel.AI.TextCompletion;

namespace Microsoft.SemanticKernel
{
    public static class ERNIEBotKernelBuilderExtensions
    {
        public static KernelBuilder WithERNIEBotChatCompletionService(this KernelBuilder builder,
            IServiceProvider service, IConfiguration configuration,
            string? serviceId = null,
            bool alsoAsTextCompletion = true,
            bool setAsDefault = false)
        {
            var client = CreateERNIEBotClient(service, configuration);
            var generation = new ERNIEBotChatCompletion(client);
            builder.WithAIService<IChatCompletion>(serviceId, generation, setAsDefault);

            if (alsoAsTextCompletion)
            {
                builder.WithAIService<ITextCompletion>(serviceId, generation, setAsDefault);
            }

            return builder;
        }

        public static KernelBuilder WithERNIEBotTurboChatCompletionService(this KernelBuilder builder,
            IServiceProvider service, IConfiguration configuration,
            string? serviceId = null,
            bool alsoAsTextCompletion = true,
            bool setAsDefault = false)
        {
            var client = CreateERNIEBotClient(service, configuration);
            var generation = new ERNIEBotTurboChatCompletion(client);
            builder.WithAIService<IChatCompletion>(serviceId, generation, setAsDefault);

            if (alsoAsTextCompletion)
            {
                builder.WithAIService<ITextCompletion>(serviceId, generation, setAsDefault);
            }

            return builder;
        }

        public static KernelBuilder WithERNIEBotEmbeddingGenerationService(this KernelBuilder builder,
            IServiceProvider service, IConfiguration configuration,
            string? serviceId = null, bool setAsDefault = false)
        {
            var client = CreateERNIEBotClient(service, configuration);
            var generation = new ERNIEBotEmbeddingGeneration(client);
            builder.WithAIService<ITextEmbeddingGeneration>(serviceId, generation, setAsDefault);
            return builder;
        }

        private static ERNIEBotClient CreateERNIEBotClient(IServiceProvider service, IConfiguration configuration)
        {
            var client = service.GetService<IHttpClientFactory>()?.CreateClient();

            var tokenStore = service.GetService<ITokenStore>();
            var logger = service.GetService<ILogger<ERNIEBotClient>>();

            var clientId = configuration["ClientId"]!;
            var secret = configuration["ClientSecret"]!;

            Requires.NotNullOrWhiteSpace(clientId, "ClientId");
            Requires.NotNullOrWhiteSpace(secret, "ClientSecret");

            return new ERNIEBotClient(clientId, secret, client, tokenStore, logger);
        }
    }
}
