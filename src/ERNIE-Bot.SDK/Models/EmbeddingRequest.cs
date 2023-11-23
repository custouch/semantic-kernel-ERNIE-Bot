using System.Text.Json.Serialization;

namespace ERNIE_Bot.SDK.Models
{
    public class EmbeddingsRequest
    {
        [JsonPropertyName("input")]
        public List<string> Input { get; set; } = new List<string>();

        [JsonPropertyName("user_id")]
        public string? UserId { get; set; }
    }
}
