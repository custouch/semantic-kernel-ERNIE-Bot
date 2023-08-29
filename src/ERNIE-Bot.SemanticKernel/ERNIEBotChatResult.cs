using ERNIE_Bot.SDK.Models;
using Microsoft.SemanticKernel.AI.ChatCompletion;
using Microsoft.SemanticKernel.AI.TextCompletion;
using Microsoft.SemanticKernel.Orchestration;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;

namespace Connectors.AI.ERNIEBot
{
    class ERNIEBotChatMessage : ChatMessageBase
    {
        public ERNIEBotChatMessage(string content)
            : base(AuthorRole.Assistant, content)
        {
        }
    }
    internal class ERNIEBotChatResult : IChatStreamingResult, ITextStreamingResult
    {
        public ERNIEBotChatResult(ChatResponse response)
        {
            this.ModelResult = new(response);
        }
        public ModelResult ModelResult { get; }
        private ChatResponse _response => ModelResult.GetResult<ChatResponse>();

        public async Task<ChatMessageBase> GetChatMessageAsync(CancellationToken cancellationToken = default)
        {
            return await Task.FromResult(new ERNIEBotChatMessage(this._response.Result));
        }

        public Task<string> GetCompletionAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_response.Result);
        }

        public async IAsyncEnumerable<string> GetCompletionStreamingAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            yield return _response.Result;
            await Task.CompletedTask;
        }

        public async IAsyncEnumerable<ChatMessageBase> GetStreamingChatMessageAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            yield return new ERNIEBotChatMessage(this._response.Result);
            await Task.CompletedTask;
        }
    }
}
