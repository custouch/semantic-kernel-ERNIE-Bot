using ERNIE_Bot.SDK;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.TextGeneration;
using Microsoft.SemanticKernel.Memory;

namespace Microsoft.SemanticKernel
{
    public static class ERNIEBotKernelBuilderExtensions
    {
        /// <summary>
        /// Adds the ERNIE Bot chat completion service to the builder with the specified service and configuration,
        /// along with optional parameters.
        /// </summary>
        /// <param name="builder">The kernel builder.</param>
        /// <param name="service">The service provider.</param>
        /// <param name="configuration">The configuration provider.</param>
        /// <param name="serviceId">The service identifier.</param>
        /// <param name="modelEndpoint">The model endpoint.</param>
        /// <returns>The updated kernel builder.</returns>
        public static IKernelBuilder WithERNIEBotChatCompletionService(this IKernelBuilder builder,
            IServiceProvider service, IConfiguration configuration,
            string? serviceId = null,
            ModelEndpoint? modelEndpoint = null)
        {
            var client = CreateERNIEBotClient(service, configuration);
            var generation = new ERNIEBotChatCompletion(client, modelEndpoint);
            builder.Services.AddKeyedSingleton<IChatCompletionService>(serviceId, generation);
            builder.Services.AddKeyedSingleton<ITextGenerationService>(serviceId, generation);

            return builder;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="clientId"></param>
        /// <param name="secret"></param>
        /// <param name="serviceId"></param>
        /// <param name="modelEndpoint"></param>
        /// <param name="alsoAsTextCompletion"></param>
        /// <param name="setAsDefault"></param>
        /// <returns></returns>
        public static IKernelBuilder WithERNIEBotChatCompletionService(this IKernelBuilder builder,
            string clientId, string secret,
            string? serviceId = null,
            ModelEndpoint? modelEndpoint = null)
        {

            var client = CreateERNIEBotClient(clientId, secret);
            var generation = new ERNIEBotChatCompletion(client, modelEndpoint);
            builder.Services.AddKeyedSingleton<IChatCompletionService>(serviceId, generation);
            builder.Services.AddKeyedSingleton<ITextGenerationService>(serviceId, generation);

            return builder;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="service"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>

        public static MemoryBuilder WithERNIEBotEmbeddingGenerationService(this MemoryBuilder builder,
            IServiceProvider service, IConfiguration configuration)
        {
            var client = CreateERNIEBotClient(service, configuration);
            var generation = new ERNIEBotEmbeddingGeneration(client);
            builder.WithTextEmbeddingGeneration(generation);
            return builder;
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="clientId"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        public static MemoryBuilder WithERNIEBotEmbeddingGenerationService(this MemoryBuilder builder,
            string clientId, string secret)
        {
            var client = CreateERNIEBotClient(clientId, secret);
            var generation = new ERNIEBotEmbeddingGeneration(client);
            builder.WithTextEmbeddingGeneration(generation);
            return builder;
        }

        private static ERNIEBotClient CreateERNIEBotClient(string clientId, string secret)
        {
            Requires.NotNullOrWhiteSpace(clientId, "ClientId");
            Requires.NotNullOrWhiteSpace(secret, "ClientSecret");

            return new ERNIEBotClient(clientId, secret, null, null, null);
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
