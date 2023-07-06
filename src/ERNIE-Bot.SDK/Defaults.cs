using System;
using System.Collections.Generic;
using System.Text;

namespace ERNIE_Bot.SDK
{
    public static class Defaults
    {
        public static string AccessTokenEndpoint = "https://aip.baidubce.com/oauth/2.0/token";

        public static string ERNIEBotEndpoint = "https://aip.baidubce.com/rpc/2.0/ai_custom/v1/wenxinworkshop/chat/completions";

        public static string ERNIEBotTurboEndpoint = "https://aip.baidubce.com/rpc/2.0/ai_custom/v1/wenxinworkshop/chat/eb-instant";

        public static string BLOOMZ7BEndpoint = "https://aip.baidubce.com/rpc/2.0/ai_custom/v1/wenxinworkshop/chat/bloomz_7b1";

        public static string EmbeddingV1Endpoint = "https://aip.baidubce.com/rpc/2.0/ai_custom/v1/wenxinworkshop/embeddings/embedding-v1";

        public static string TokenCacheName = "ERNIE_BOT:AK";
    }
}
