namespace ERNIE_Bot.SDK
{
    /// <summary>
    /// Use ModelEndpoints to get the model name
    /// </summary>
    public abstract class Endpoint
    {
        public string Model { get; set; }
        public string Task { get; set; }

        internal Endpoint(string model, string task)
        {
            Model = model;
            Task = task;
        }

        public static implicit operator string(Endpoint endpoint)
        {
            return endpoint.Model;
        }
    }


    /// <inheritdoc/>
    public class ModelEndpoint : Endpoint
    {
        public ModelEndpoint(string model) : base(model, "chat")
        {
        }
    }

    /// <inheritdoc/>
    public class EmbeddingModelEndpoint : Endpoint
    {
        public EmbeddingModelEndpoint(string endpoint) : base(endpoint, "embeddings")
        {
        }
    }
}
