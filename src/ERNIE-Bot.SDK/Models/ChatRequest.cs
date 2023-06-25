using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace ERNIE_Bot.SDK.Models
{
    public class ChatRequest
    {
        [JsonPropertyName("messages")]
        public List<Message> Messages { get; set; } = new List<Message>();
        public bool? Stream { get; set; }
        [JsonPropertyName("user_id")]
        public string? UserId { get; set; }
    }

    public class ChatCompletionsRequest:ChatRequest
    {
        [JsonPropertyName("temperature")]
        public float? Temperature { get; set; }
        [JsonPropertyName("top_p")]
        public float? TopP { get; set; }
        [JsonPropertyName("penalty_score")]
        public float? PenaltyScore { get; set; }  
    }

    public class Message
    {
        [JsonPropertyName("role")]
        public string Role { get; set; } = string.Empty;

        [JsonPropertyName("content")]
        public string Content { get; set; } = string.Empty;
    }
}
