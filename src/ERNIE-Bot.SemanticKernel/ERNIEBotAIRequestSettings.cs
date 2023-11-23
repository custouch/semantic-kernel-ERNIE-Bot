using Microsoft.SemanticKernel.AI;
using System.Text.Json;
using System.Text.Json.Serialization;

public class ERNIEBotAIRequestSettings : AIRequestSettings
{
    [JsonPropertyName("temperature")]
    public float? Temperature { get; set; }

    [JsonPropertyName("top_p")]
    public float? TopP { get; set; }

    [JsonPropertyName("penalty_score")]
    public float? PenaltyScore { get; set; }

    public static ERNIEBotAIRequestSettings FromRequestSettings(AIRequestSettings? requestSettings, int? defaultMaxTokens = null)
    {
        if (requestSettings is null)
        {
            return new ERNIEBotAIRequestSettings();
        }

        if (requestSettings is ERNIEBotAIRequestSettings requestSettingERNIEBotAIRequestSettings)
        {
            return requestSettingERNIEBotAIRequestSettings;
        }

        var json = JsonSerializer.Serialize(requestSettings);
        var ernieBotAIRequestSettings = JsonSerializer.Deserialize<ERNIEBotAIRequestSettings>(json, s_options);

        if (ernieBotAIRequestSettings is not null)
        {
            return ernieBotAIRequestSettings;
        }

        throw new ArgumentException($"Invalid request settings, cannot convert to {nameof(ERNIEBotAIRequestSettings)}", nameof(requestSettings));
    }

    private static readonly JsonSerializerOptions s_options = CreateOptions();

    private static JsonSerializerOptions CreateOptions()
    {
        JsonSerializerOptions options = new()
        {
            WriteIndented = true,
            MaxDepth = 20,
            AllowTrailingCommas = true,
            PropertyNameCaseInsensitive = true,
            ReadCommentHandling = JsonCommentHandling.Skip
        };

        return options;
    }
}
