using ERNIE_Bot.SDK;
using ERNIE_Bot.SDK.Models;
using Microsoft.SemanticKernel.AI;
using Microsoft.SemanticKernel.AI.Embeddings;
using Microsoft.SemanticKernel.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ERNIEBotEmbeddingGeneration : ITextEmbeddingGeneration
{
    private readonly ERNIEBotClient _client;

    public ERNIEBotEmbeddingGeneration(ERNIEBotClient client)
    {
        this._client = client;
    }

    public async Task<IList<Embedding<float>>> GenerateEmbeddingsAsync(IList<string> data, CancellationToken cancellationToken)
    {
        try
        {
            var embeddings = await _client.EmbeddingsAsync(new EmbeddingsRequest()
            {
                Input = data.ToList()
            });

            // TODO: ITextEmbeddingGeneration not support Embedding<double> 
            return embeddings.Data.Select(d => new Embedding<float>(d.Embedding.Select(e => (float)e))).ToList();
        }
        catch (ERNIEBotException ex)
        {
            throw new AIException(AIException.ErrorCodes.ServiceError, ex.Error.Message, ex);
        }
    }
}
