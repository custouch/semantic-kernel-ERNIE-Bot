using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace ERNIE_Bot.SDK.Models
{
    public class EmbeddingsResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("object")]
        public string ObjectType { get; set; } = "embedding_list";

        [JsonPropertyName("created")]
        public int Created { get; set; }

        [JsonPropertyName("data")]
        public List<EmbeddingData> Data { get; set; } = new List<EmbeddingData>();

        [JsonPropertyName("usage")]
        public UsageData Usage { get; set; } = new UsageData();
    }

    public class EmbeddingData
    {
        [JsonPropertyName("object")]
        public string ObjectType { get; set; } = "embedding";

        [JsonPropertyName("embedding")]
        public List<double> Embedding { get; set; } = new List<double>();

        [JsonPropertyName("index")]
        public int Index { get; set; }
    }
}
