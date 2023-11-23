using System.Text.Json.Serialization;

namespace ERNIE_Bot.SDK.Models
{
    public class ChatResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("object")]
        public string Object { get; set; } = "chat.completion";

        [JsonPropertyName("created")]
        public int Created { get; set; }

        [JsonPropertyName("sentence_id")]
        public int? SentenceId { get; set; }

        [JsonPropertyName("is_truncated")]
        public bool? IsTruncated { get; set; }

        [JsonPropertyName("is_end")]
        public bool? IsEnd { get; set; }

        [JsonPropertyName("result")]
        public string Result { get; set; } = string.Empty;

        [JsonPropertyName("need_clear_history")]
        public bool NeedClearHistory { get; set; }

        [JsonPropertyName("usage")]
        public UsageData Usage { get; set; } = new UsageData();
    }
}
