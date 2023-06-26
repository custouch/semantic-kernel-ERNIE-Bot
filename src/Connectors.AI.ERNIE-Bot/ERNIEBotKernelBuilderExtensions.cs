using ERNIE_Bot.SDK;
using Microsoft;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Connectors.AI.ERNIEBot
{
    public static class ERNIEBotKernelBuilderExtensions
    {
        public static KernelBuilder WithERNIEBotChatCompletionService(this KernelBuilder builder,
            IServiceProvider service, IConfiguration configuration,
            string? serviceId = null, bool setAsDefault = false)
        {
            var client = CreateERNIEBotClient(service, configuration);
            var generation = new ERNIEBotChatCompletion(client);
            builder.WithAIService(serviceId, generation, setAsDefault);
            return builder;
        }

        public static KernelBuilder WithERNIEBotTurboChatCompletionService(this KernelBuilder builder,
            IServiceProvider service, IConfiguration configuration,
            string? serviceId = null, bool setAsDefault = false)
        {
            var client = CreateERNIEBotClient(service, configuration);
            var generation = new ERNIEBotTurboChatCompletion(client);
            builder.WithAIService(serviceId, generation, setAsDefault);
            return builder;
        }

        public static KernelBuilder WithERNIEBotEmbeddingGenerationService(this KernelBuilder builder,
            IServiceProvider service, IConfiguration configuration,
            string? serviceId = null, bool setAsDefault = false)
        {
            var client = CreateERNIEBotClient(service, configuration);
            var generation = new ERNIEBotEmbeddingGeneration(client);
            builder.WithAIService(serviceId, generation, setAsDefault);
            return builder;
        }

        private static ERNIEBotClient CreateERNIEBotClient(IServiceProvider service, IConfiguration configuration)
        {
            var factory = service.GetRequiredService<IHttpClientFactory>();
            var client = factory.CreateClient();
            var tokenStore = service.GetRequiredService<ITokenStore>();

            var clientId = configuration["ClientId"]!;
            var secret = configuration["ClientSecret"]!;

            Requires.NotNullOrWhiteSpace(clientId);
            Requires.NotNullOrWhiteSpace(secret);

            return new ERNIEBotClient(clientId, secret, client, tokenStore);
        }
    }
}
