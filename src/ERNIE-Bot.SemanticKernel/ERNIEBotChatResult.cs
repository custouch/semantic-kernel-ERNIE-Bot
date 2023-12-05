using ERNIE_Bot.SDK.Models;
using Microsoft.SemanticKernel.AI.ChatCompletion;
using System.Runtime.CompilerServices;
using System.Text;

namespace Connectors.AI.ERNIEBot
{
    internal class ERNIEBotChatMessage : ChatMessageContent
    {
        public ERNIEBotChatMessage(ChatResponse response)
            : base(AuthorRole.Assistant, response.Result)
        {

        }

        public ERNIEBotChatMessage(string content)
            : base(AuthorRole.Assistant, content)
        {
        }
    }

    internal class ERNIEBotStreamingChatMessage : StreamingChatMessageContent
    {
        public ERNIEBotStreamingChatMessage(ChatResponse response)
            : base(AuthorRole.Assistant, response.Result, response)
        {

        }
    }
}
