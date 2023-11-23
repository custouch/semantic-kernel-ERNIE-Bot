using System.Text.Json.Serialization;

namespace ERNIE_Bot.SDK.Models
{
    /// <summary>
    /// Access Token Response
    /// </summary>
    public class TokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; } = string.Empty;

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; } = string.Empty;

        [JsonPropertyName("expires_in")]
        public long Expiration { get; set; }

        [JsonPropertyName("session_key")]
        public string SessionKey { get; set; } = string.Empty;

        [JsonPropertyName("session_secret")]
        public string SessionSecret { get; set; } = string.Empty;

        [JsonPropertyName("scope")]
        public string Scope { get; set; } = string.Empty;
    }
}
