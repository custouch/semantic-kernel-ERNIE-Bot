using ERNIE_Bot.SDK;
using Microsoft.KernelMemory;
using Microsoft.KernelMemory.AI;
using Microsoft.SemanticKernel.AI.Embeddings;

namespace ERNIE_Bot.KernelMemory
{
    /// <summary>
    /// Generating text embeddings using ERNIEBotClient.
    /// </summary>
    public class ERNIEBotTextEmbeddingGenerator : ITextEmbeddingGenerator
    {
        private readonly ERNIEBotClient _client;
        private readonly EmbeddingModelEndpoint _endpoint;

        /// <summary>
        /// Initializes a new instance of the <see cref="ERNIEBotTextEmbeddingGenerator"/> class.
        /// </summary>
        /// <param name="client">The ERNIEBotClient instance to use for generating embeddings.</param>
        /// <param name="endpoint">The endpoint to use for the embedding model. Defaults to ModelEndpoints.Embedding_v1.</param>
        public ERNIEBotTextEmbeddingGenerator(ERNIEBotClient client, EmbeddingModelEndpoint? endpoint = null)
        {
            this._client = client;
            _endpoint = endpoint ?? ModelEndpoints.Embedding_v1;
        }

        public int MaxTokens => _endpoint.MaxTokens;

        /// <inheritdoc/>
        public async Task<Embedding> GenerateEmbeddingAsync(string text, CancellationToken cancellationToken = default)
        {
            var embeddings = await _client.EmbeddingsAsync(new SDK.Models.EmbeddingsRequest()
            {
                Input = [text]
            }, _endpoint, cancellationToken).ConfigureAwait(false);

            return new Embedding(embeddings.Data[0].Embedding.Select(e => (float)e).ToArray());
        }

        public int CountTokens(string text)
        {
            return Tokenizer.ApproxNumTokens(text);
        }
    }
}
