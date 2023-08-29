using System;
using System.Collections.Generic;
using System.Text;

namespace ERNIE_Bot.SDK
{
    public static class Defaults
    {
        public static string AccessTokenEndpoint = "https://aip.baidubce.com/oauth/2.0/token";

        [Obsolete("Use Endpoint() and  ModelEndpoints to get the model endpoint")]
        public static string ERNIEBotEndpoint = "https://aip.baidubce.com/rpc/2.0/ai_custom/v1/wenxinworkshop/chat/completions";

        [Obsolete("Use Endpoint() and  ModelEndpoints to get the model endpoint")]
        public static string ERNIEBotTurboEndpoint = "https://aip.baidubce.com/rpc/2.0/ai_custom/v1/wenxinworkshop/chat/eb-instant";

        [Obsolete("Use Endpoint() and  ModelEndpoints to get the model endpoint")]
        public static string BLOOMZ7BEndpoint = "https://aip.baidubce.com/rpc/2.0/ai_custom/v1/wenxinworkshop/chat/bloomz_7b1";

        public static string EmbeddingV1Endpoint = "https://aip.baidubce.com/rpc/2.0/ai_custom/v1/wenxinworkshop/embeddings/embedding-v1";

        public static string TokenCacheName = "ERNIE_BOT:AK";

        /// <summary>
        /// Use ModelEndpoints to get the model name
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string Endpoint(string model) => $"https://aip.baidubce.com/rpc/2.0/ai_custom/v1/wenxinworkshop/chat/{model}";

    }

    public static class ModelEndpoints
    {
        public const string ERNIE_Bot = "completions";
        public const string ERNIE_Bot_Turbo = "eb-instant";
        public const string BLOOMZ_7B = "bloomz_7b1";
        public const string Llama_2_7b_chat = "llama_2_7b";
        public const string Llama_2_13b_chat = "llama_2_13b";
        public const string Llama_2_70b_chat = "llama_2_70b";
    }
}
