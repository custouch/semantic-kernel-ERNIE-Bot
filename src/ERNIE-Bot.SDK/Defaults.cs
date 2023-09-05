using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ERNIE_Bot.SDK
{
    public static class Defaults
    {
        public static string AccessTokenEndpoint = "https://aip.baidubce.com/oauth/2.0/token";

        public static string EmbeddingV1Endpoint = "https://aip.baidubce.com/rpc/2.0/ai_custom/v1/wenxinworkshop/embeddings/embedding-v1";

        public static string TokenCacheName = "ERNIE_BOT:AK";

        /// <summary>
        /// Use <see cref="ModelEndpoints">ModelEndpoints</see> to get the model name
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string Endpoint(string model) => $"https://aip.baidubce.com/rpc/2.0/ai_custom/v1/wenxinworkshop/chat/{model}";

        /// <summary>
        /// Use <see cref="ModelEndpoints">ModelEndpoints</see> to get the Model Endpoint
        /// </summary>
        public static string Endpoint(Endpoint endpoint) => $"https://aip.baidubce.com/rpc/2.0/ai_custom/v1/wenxinworkshop/{endpoint.Task}/{endpoint.Model}";

    }
}
