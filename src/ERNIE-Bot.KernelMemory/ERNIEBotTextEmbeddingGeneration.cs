using ERNIE_Bot.SDK;
using Microsoft.SemanticKernel.AI.Embeddings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERNIE_Bot.KernelMemory
{

    /// <summary>
    /// Generating text embeddings using ERNIEBotClient.
    /// </summary>
    public class ERNIEBotTextEmbeddingGeneration : ITextEmbeddingGeneration
    {
        private readonly ERNIEBotClient _client;
        private readonly EmbeddingModelEndpoint _endpoint;



        /// <summary>
        /// Initializes a new instance of the <see cref="ERNIEBotTextEmbeddingGeneration"/> class.
        /// </summary>
        /// <param name="client">The ERNIEBotClient instance to use for generating embeddings.</param>
        /// <param name="endpoint">The endpoint to use for the embedding model. Defaults to ModelEndpoints.Embedding_v1.</param>
        public ERNIEBotTextEmbeddingGeneration(ERNIEBotClient client, EmbeddingModelEndpoint? endpoint = null)
        {
            this._client = client;
            _endpoint = endpoint ?? ModelEndpoints.Embedding_v1;
        }

        private readonly Dictionary<string, string> _attributes = new();
        public IReadOnlyDictionary<string, string> Attributes => _attributes;

        /// <inheritdoc/>
        public async Task<IList<ReadOnlyMemory<float>>> GenerateEmbeddingsAsync(IList<string> data, CancellationToken cancellationToken = default)
        {
            var embeddings = await _client.EmbeddingsAsync(new SDK.Models.EmbeddingsRequest()
            {
                Input = data.ToList()
            }, _endpoint, cancellationToken);

            return embeddings.Data.Select(d => new ReadOnlyMemory<float>(d.Embedding.Select(e => (float)e).ToArray())).ToList();
        }
    }
}
