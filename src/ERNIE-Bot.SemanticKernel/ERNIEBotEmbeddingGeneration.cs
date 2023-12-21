using ERNIE_Bot.SDK;
using ERNIE_Bot.SDK.Models;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Embeddings;

public class ERNIEBotEmbeddingGeneration : ITextEmbeddingGenerationService
{
    private readonly ERNIEBotClient _client;
    private readonly Dictionary<string, object?> _attributes = new();
    public IReadOnlyDictionary<string, object?> Attributes => this._attributes;

    public ERNIEBotEmbeddingGeneration(ERNIEBotClient client)
    {
        this._client = client;
    }

    public async Task<IList<ReadOnlyMemory<float>>> GenerateEmbeddingsAsync(IList<string> data, Kernel? kernel = null, CancellationToken cancellationToken = default)
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
            throw new KernelException(ex.Error.Message, ex);
        }
    }
}
