﻿using ERNIE_Bot.SDK;
using ERNIE_Bot.SDK.Models;
using Microsoft.SemanticKernel.AI.Embeddings;
using Microsoft.SemanticKernel.Diagnostics;

public class ERNIEBotEmbeddingGeneration : ITextEmbeddingGeneration
{
    private readonly ERNIEBotClient _client;
    private readonly Dictionary<string, string> _attributes = new();
    public IReadOnlyDictionary<string, string> Attributes => this._attributes;

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
