using ERNIE_Bot.SDK;
using ERNIE_Bot.SDK.Models;
using Microsoft.SemanticKernel.AI;
using Microsoft.SemanticKernel.AI.Embeddings;
using Microsoft.SemanticKernel.Diagnostics;
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

    public async Task<IList<ReadOnlyMemory<float>>> GenerateEmbeddingsAsync(IList<string> data, CancellationToken cancellationToken)
    {
        try
        {
            var embeddings = await _client.EmbeddingsAsync(new EmbeddingsRequest()
            {
                Input = data.ToList()
            }, cancellationToken);

            // TODO: ITextEmbeddingGeneration not support ReadOnlyMemory<double> 
            return embeddings.Data.Select(d => new ReadOnlyMemory<float>(d.Embedding.Select(e => (float)e).ToArray())).ToList();
        }
        catch (ERNIEBotException ex)
        {
            throw new SKException(ex.Error.Message, ex);
        }
    }
}
