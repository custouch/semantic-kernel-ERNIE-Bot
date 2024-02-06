using System.Text.Json.Serialization;

namespace ERNIE_Bot.SDK.Models
{
    public class ChatRequest
    {
        [JsonPropertyName("system")]
        public string? System { get; set; }

        [JsonPropertyName("messages")]
        public List<Message> Messages { get; set; } = new List<Message>();

        public bool? Stream { get; set; }

        [JsonPropertyName("user_id")]
        public string? UserId { get; set; }
    }

    public class ChatCompletionsRequest : ChatRequest
    {
        [JsonPropertyName("temperature")]
        public float? Temperature { get; set; }

        [JsonPropertyName("top_p")]
        public float? TopP { get; set; }

        [JsonPropertyName("penalty_score")]
        public float? PenaltyScore { get; set; }

        /// <summary>
        /// 指定响应内容的格式，说明：<br/>
        ///（1）可选值：<br/>
        ///· json_object：以json格式返回，可能出现不满足效果情况 <br/>
        ///· text：以文本格式返回 <br/>
        ///（2）如果不填写参数response_format值，默认为text <br/>
        /// </summary>
        [JsonPropertyName("response_format")]
        public string? ResponseFormat { get; set; }

        /// <summary>
        /// 指定模型最大输出token数，范围[2, 2048]
        /// </summary>
        [JsonPropertyName("max_output_tokens")]
        public int? MaxTokens { get; set; }

    }

    public class Message
    {
        [JsonPropertyName("role")]
        public string Role { get; set; } = string.Empty;

        [JsonPropertyName("content")]
        public string Content { get; set; } = string.Empty;
    }

    public static class MessageRole
    {
        public const string User = "user";
        public const string Assistant = "assistant";
    }
}
