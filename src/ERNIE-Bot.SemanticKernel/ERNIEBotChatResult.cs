using ERNIE_Bot.SDK.Models;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace Connectors.AI.ERNIEBot
{
    internal class ERNIEBotChatMessage : ChatMessageContent
    {
        public ERNIEBotChatMessage(ChatResponse response, IReadOnlyDictionary<string, object?>? metadata = null)
            : base(AuthorRole.Assistant, response.Result, metadata: metadata)
        {

        }
    }

    internal class ERNIEBotStreamingChatMessage : StreamingChatMessageContent
    {
        public ERNIEBotStreamingChatMessage(ChatResponse response, IReadOnlyDictionary<string, object?>? metadata = null)
            : base(AuthorRole.Assistant, response.Result, response, metadata: metadata)
        {

        }
    }
}
