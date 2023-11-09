namespace ERNIE_Bot.SDK
{
    public static class ModelEndpoints
    {
        /// <summary>
        /// <see href="https://cloud.baidu.com/doc/WENXINWORKSHOP/s/clntwmv7t">ERNIE Bot 4 Chat Model</see>
        /// </summary>
        public static readonly ModelEndpoint ERNIE_Bot_4 = new("completions_pro");

        /// <summary>
        /// <see href="https://cloud.baidu.com/doc/WENXINWORKSHOP/s/jlil56u11">ERNIE Bot Chat Model</see>
        /// </summary>
        public static readonly ModelEndpoint ERNIE_Bot = new("completions");

        /// <summary>
        /// <see href="https://cloud.baidu.com/doc/WENXINWORKSHOP/s/4lilb2lpf"> ERNIE Bot Turbo Chat Model</see>
        /// </summary>
        public static readonly ModelEndpoint ERNIE_Bot_Turbo = new("eb-instant");

        /// <summary>
        /// <see href="https://cloud.baidu.com/doc/WENXINWORKSHOP/s/Jljcadglj">BLOOMZ 7B Chat Model</see>
        /// </summary>
        public static readonly ModelEndpoint BLOOMZ_7B = new("bloomz_7b1");

        /// <summary>
        /// <see href="https://cloud.baidu.com/doc/WENXINWORKSHOP/s/Rlki1zlai"> Llama2 7b Chat Model</see>
        /// </summary>
        public static readonly ModelEndpoint Llama_2_7b_chat = new("llama_2_7b");

        /// <summary>
        /// <see href="https://cloud.baidu.com/doc/WENXINWORKSHOP/s/2lki2us1e">Llama2 13b Chat Model</see> 
        /// </summary>
        public static readonly ModelEndpoint Llama_2_13b_chat = new("llama_2_13b");

        /// <summary>
        /// <see cref="https://cloud.baidu.com/doc/WENXINWORKSHOP/s/8lkjfhiyt"> Llama2 70b Chat Model</see>
        /// </summary>
        public static readonly ModelEndpoint Llama_2_70b_chat = new("llama_2_70b");

        /// <summary>
        /// <see href="https://cloud.baidu.com/doc/WENXINWORKSHOP/s/nllyzpcmp">Qianfan BLOOMZ 7B Chat Model</see>
        /// </summary>
        public static readonly ModelEndpoint Qianfan_BLOOMZ_7B_compressed = new("qianfan_bloomz_7b_compressed");

        /// <summary>
        /// <see href="https://cloud.baidu.com/doc/WENXINWORKSHOP/s/Sllyztytp">Qianfan Chinese Llama2 7B Chat Model</see>
        /// </summary>
        public static readonly ModelEndpoint Qianfan_Chinese_Llama_2_7b = new("qianfan_chinese_llama_2_7b");

        /// <summary>
        /// <see href="https://cloud.baidu.com/doc/WENXINWORKSHOP/s/Bllz001ff">ChatGLM2 6b 32k Chat Model</see>
        /// </summary>
        public static readonly ModelEndpoint ChatGLM2_6b_32k = new("chatglm2_6b_32k");

        /// <summary>
        /// <see href="https://cloud.baidu.com/doc/WENXINWORKSHOP/s/ollz02e7i">AquilaChat-7B Chat Model</see>
        /// </summary>
        public static readonly ModelEndpoint AquilaChat_7b = new("aquilachat_7b");

        /// <summary>
        /// <see href="https://cloud.baidu.com/doc/WENXINWORKSHOP/s/alj562vvu">Embedding-V1 Embedding Model</see>
        /// </summary>
        public static readonly EmbeddingModelEndpoint Embedding_v1 = new("embedding-v1");

        /// <summary>
        /// <see href="https://cloud.baidu.com/doc/WENXINWORKSHOP/s/dllz04sro">bge-large-zh Embedding Model</see>
        /// </summary>
        public static readonly EmbeddingModelEndpoint bge_large_zh = new("bge_large_zh");

        /// <summary>
        /// <see href="https://cloud.baidu.com/doc/WENXINWORKSHOP/s/mllz05nzk">bge-large-en Embedding Model</see>
        /// </summary>
        public static readonly EmbeddingModelEndpoint bge_large_en = new("bge_large_en");
    }
}
