using ERNIE_Bot.SDK;
using Microsoft.SemanticKernel.AI.Embeddings;
using Microsoft.SemanticKernel.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ERNIEBotEmbeddingGeneration : IEmbeddingGeneration<string, double>, IAIService
{
    private readonly ERNIEBotClient _client;

    public ERNIEBotEmbeddingGeneration(ERNIEBotClient client)
    {
        this._client = client;
    }

    public async Task<IList<Embedding<double>>> GenerateEmbeddingsAsync(IList<string> data, CancellationToken cancellationToken)
    {
        var embeddings = await _client.EmbeddingsAsync(new ERNIE_Bot.SDK.Models.EmbeddingsRequest()
        {
            Input = data.ToList()
        }, cancellationToken);
        return embeddings.Data.Select(d => new Embedding<double>(d.Embedding)).ToList();
    }
}
