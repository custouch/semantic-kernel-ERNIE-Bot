namespace ERNIE_Bot.SDK
{
    /// <summary>
    /// Use ModelEndpoints to get the model name
    /// </summary>
    public abstract class Endpoint
    {
        public string Model { get; set; }
        public string Task { get; set; }
        public int MaxTokens { get; set; }

        internal Endpoint(string model, string task, int maxTokens)
        {
            Model = model;
            Task = task;
            MaxTokens = maxTokens;
        }

        public static implicit operator string(Endpoint endpoint)
        {
            return endpoint.Model;
        }
    }

    /// <inheritdoc/>
    public class ModelEndpoint : Endpoint
    {
        public ModelEndpoint(string model, int maxTokens = 2000) : base(model, "chat", maxTokens)
        {
        }
    }

    /// <inheritdoc/>
    public class EmbeddingModelEndpoint : Endpoint
    {
        public EmbeddingModelEndpoint(string endpoint, int maxTokens = 384) : base(endpoint, "embeddings", maxTokens)
        {
        }
    }
}
