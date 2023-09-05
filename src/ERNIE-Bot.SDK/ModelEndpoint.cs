namespace ERNIE_Bot.SDK
{
    /// <summary>
    /// Use ModelEndpoints to get the model name
    /// </summary>
    public class ModelEndpoint
    {
        public string Endpoint { get; set; }
        public string Task { get; set; }

        internal ModelEndpoint(string endpoint, string task = "chat")
        {
            Endpoint = endpoint;
            Task = task;
        }

        public static implicit operator string(ModelEndpoint endpoint)
        {
            return endpoint.Endpoint;
        }
    }
}
