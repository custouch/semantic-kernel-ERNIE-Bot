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

        /// <summary>
        /// 生成停止标识，当模型生成结果以stop中某个元素结尾时，停止文本生成。
        /// <br/> 说明：
        /// <br/>（1）每个元素长度不超过20字符 
        /// <br/>（2）最多4个元素
        /// </summary>
        [JsonPropertyName("stop")]
        public string[]? Stops { get; set; }

        /// <summary>
        /// 是否强制关闭实时搜索功能，默认false，表示不关闭
        /// </summary>
        [JsonPropertyName("disable_search")]
        public bool? DisableSearch { get; set; }

        /// <summary>
        /// 是否开启引用返回，说明：
        ///（1）开启后，有概率触发搜索溯源信息search_info，search_info内容见响应参数介绍
        ///（2）默认false，不开启
        [JsonPropertyName("enable_citation")]
        public bool? EnableCitation { get; set; }
    }

    public class FunctionInfo
    {

    }

    public class Message
    {
        [JsonPropertyName("role")]
        public string Role { get; set; } = string.Empty;

        [JsonPropertyName("content")]
        public string Content { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string? Name { get; set; }
    }
    public static class MessageRole
    {
        public const string User = "user";
        public const string Assistant = "assistant";
    }
}
