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
    class ERNIEBotChatMessage : ChatMessage
    {
        public ERNIEBotChatMessage(string content)
            : base(AuthorRole.Assistant, content)
        {
        }
    }

    internal class ERNIEBotChatResult : IChatResult, ITextResult, IChatStreamingResult, ITextStreamingResult
    {
        private readonly ChatResponse? _response;
        private readonly IAsyncEnumerable<ChatResponse>? _responses;
        public ERNIEBotChatResult(IAsyncEnumerable<ChatResponse> responses)
        {
            this.ModelResult = new ModelResult(responses);
            this._responses = responses;
        }
        public ERNIEBotChatResult(ChatResponse response)
        {
            this.ModelResult = new ModelResult(response);
            this._response = response;
        }

        public ModelResult ModelResult { get; private set; }

        public async Task<ChatMessage> GetChatMessageAsync(CancellationToken cancellationToken = default)
        {
            var result = await GetCompletionAsync(cancellationToken);
            return new ERNIEBotChatMessage(result);
        }

        public async Task<string> GetCompletionAsync(CancellationToken cancellationToken = default)
        {
            if (this._response != null)
            {
                return this._response.Result;
            }
            else
            {
                var result = new StringBuilder();
                await foreach (var response in _responses.WithCancellation(cancellationToken))
                {
                    result.Append(response.Result);
                }

                return result.ToString();
            }
        }

        #region Streaming
        public async IAsyncEnumerable<string> GetCompletionStreamingAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await foreach (var response in _responses.WithCancellation(cancellationToken).ConfigureAwait(false))
            {
                yield return response.Result;
            }
        }

        public async IAsyncEnumerable<ChatMessage> GetStreamingChatMessageAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await foreach (var response in _responses.WithCancellation(cancellationToken))
            {
                yield return new ERNIEBotChatMessage(response.Result);
            }
        }
        #endregion
    }
}
