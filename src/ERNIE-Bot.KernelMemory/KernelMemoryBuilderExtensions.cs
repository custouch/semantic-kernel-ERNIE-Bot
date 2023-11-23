using ERNIE_Bot.SDK;
using Microsoft.KernelMemory;

namespace ERNIE_Bot.KernelMemory
{
    /// <summary>
    /// Provides extension methods for configuring the KernelMemoryBuilder with ERNIEBot defaults.
    /// </summary>
    public static class KernelMemoryBuilderExtensions
    {
        /// <summary>
        /// Configures the KernelMemoryBuilder with ERNIEBot text generation.
        /// </summary>
        /// <param name="builder">The KernelMemoryBuilder instance to configure.</param>
        /// <param name="client">The ERNIEBotClient instance to use for text generation.</param>
        /// <param name="endpoint">The endpoint to use for the text generation model. Defaults to ModelEndpoints.ERNIE_Bot_Turbo.</param>
        /// <returns>The configured KernelMemoryBuilder instance.</returns>
        public static KernelMemoryBuilder WithERNIEBotTextGeneration(this KernelMemoryBuilder builder, ERNIEBotClient client, ModelEndpoint? endpoint = null)
        {
            builder.WithCustomTextGeneration(new ERNIEBotTextGeneration(client, endpoint));
            return builder;
        }

        /// <summary>
        /// Configures the KernelMemoryBuilder with ERNIEBot embedding generation.
        /// </summary>
        /// <param name="builder">The KernelMemoryBuilder instance to configure.</param>
        /// <param name="client">The ERNIEBotClient instance to use for embedding generation.</param>
        /// <param name="endpoint">The endpoint to use for the embedding generation model. Defaults to ModelEndpoints.Embedding_v1.</param>
        /// <returns>The configured KernelMemoryBuilder instance.</returns>
        public static KernelMemoryBuilder WithERNIEBotEmbeddingGeneration(this KernelMemoryBuilder builder, ERNIEBotClient client, EmbeddingModelEndpoint? endpoint = null)
        {
            builder.WithCustomEmbeddingGeneration(new ERNIEBotTextEmbeddingGeneration(client, endpoint));
            return builder;
        }

        /// <summary>
        /// Configures the KernelMemoryBuilder with ERNIEBot defaults.
        /// </summary>
        /// <param name="builder">The KernelMemoryBuilder instance to configure.</param>
        /// <param name="clientId">The client ID to use for the ERNIEBotClient instance.</param>
        /// <param name="clientSecret">The client secret to use for the ERNIEBotClient instance.</param>
        /// <returns>The configured KernelMemoryBuilder instance.</returns>
        public static KernelMemoryBuilder WithERNIEBotDefaults(this KernelMemoryBuilder builder, string clientId, string clientSecret)
        {
            var client = new ERNIEBotClient(clientId, clientSecret);
            return builder.WithERNIEBotDefaults(client);
        }

        /// <summary>
        /// Configures the KernelMemoryBuilder with ERNIEBot defaults.
        /// </summary>
        /// <param name="builder">The KernelMemoryBuilder instance to configure.</param>
        /// <param name="client">The ERNIEBotClient instance to use for text and embedding generation.</param>
        /// <returns>The configured KernelMemoryBuilder instance.</returns>
        public static KernelMemoryBuilder WithERNIEBotDefaults(this KernelMemoryBuilder builder, ERNIEBotClient client)
        {
            builder.WithERNIEBotTextGeneration(client);
            builder.WithERNIEBotEmbeddingGeneration(client);
            return builder;
        }
    }
}
