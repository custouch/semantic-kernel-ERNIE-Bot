using Connectors.AI.ERNIEBot;
using ERNIE_Bot.SDK;
using ERNIE_Bot.SDK.Models;
using Microsoft.SemanticKernel.AI;
using Microsoft.SemanticKernel.AI.ChatCompletion;
using Microsoft.SemanticKernel.AI.TextCompletion;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

public class ERNIEBotChatCompletion : IChatCompletion, ITextCompletion
{
    protected readonly ERNIEBotClient _client;

    public ERNIEBotChatCompletion(ERNIEBotClient client)
    {
        this._client = client;
    }
    public ChatHistory CreateNewChat(string? instructions = null)
    {
        var history = new ChatHistory();

        if (instructions != null)
        {
            history.AddAssistantMessage(instructions);
        }

        return history;
    }


    public async Task<IReadOnlyList<IChatResult>> GetChatCompletionsAsync(ChatHistory chat, ChatRequestSettings? requestSettings = null, CancellationToken cancellationToken = default)
    {
        var messages = ChatHistoryToMessages(chat);
        requestSettings ??= new ChatRequestSettings();

        ChatResponse result = await InternalCompletionsAsync(messages,
                                                             requestSettings.Temperature,
                                                             requestSettings.TopP,
                                                             requestSettings.PresencePenalty
                                                             );
        return new List<ERNIEBotChatResult>() { new ERNIEBotChatResult(result) };
    }



    public async Task<IReadOnlyList<ITextResult>> GetCompletionsAsync(string text, CompleteRequestSettings requestSettings, CancellationToken cancellationToken = default)
    {
        requestSettings ??= new CompleteRequestSettings();
        var messages = StringToMessages(text);

        var result = await InternalCompletionsAsync(messages,
                                                             requestSettings.Temperature,
                                                             requestSettings.TopP,
                                                             requestSettings.PresencePenalty
                                                             );

        return new List<ERNIEBotChatResult>() { new ERNIEBotChatResult(result) };
    }

    public async IAsyncEnumerable<IChatStreamingResult> GetStreamingChatCompletionsAsync(ChatHistory chat, ChatRequestSettings? requestSettings = null, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var messages = ChatHistoryToMessages(chat);
        requestSettings ??= new ChatRequestSettings();

        var results = InternalCompletionsStreamAsync(messages,
                                                    requestSettings.Temperature,
                                                    requestSettings.TopP,
                                                    requestSettings.PresencePenalty
                                                    );

        await foreach (var result in results)
        {
            yield return new ERNIEBotChatResult(result);
        }
    }

    public async IAsyncEnumerable<ITextStreamingResult> GetStreamingCompletionsAsync(string text, CompleteRequestSettings requestSettings, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var messages = StringToMessages(text);
        requestSettings ??= new CompleteRequestSettings();

        var results = InternalCompletionsStreamAsync(messages,
                                                     requestSettings.Temperature,
                                                     requestSettings.TopP,
                                                     requestSettings.PresencePenalty
                                                     );

        await foreach (var result in results)
        {
            yield return new ERNIEBotChatResult(result);
        }
    }

    private List<Message> StringToMessages(string text)
    {
        return new List<Message>()
        {
            new Message()
            {
                 Role = MessageRole.User,
                 Content = text
            }
        };
    }

    private List<Message> ChatHistoryToMessages(ChatHistory chatHistory)
    {
        return chatHistory.Select(m => new Message()
        {
            Role = AuthorRoleToMessageRole(m.Role),
            Content = m.Content
        }).ToList();
    }

    private string AuthorRoleToMessageRole(AuthorRole role)
    {
        if (role == AuthorRole.User) return MessageRole.User;
        if (role == AuthorRole.Assistant) return MessageRole.Assistant;
        return MessageRole.User;
    }

    protected virtual async Task<ChatResponse> InternalCompletionsAsync(List<Message> messages, double temperature, double topP, double presencePenalty)
    {
        try
        {
            return await _client.ChatCompletionsAsync(new ChatCompletionsRequest()
            {
                Messages = messages,
                Temperature = (float)temperature,
                TopP = (float)topP,
                PenaltyScore = (float)presencePenalty,
            });
        }
        catch (ERNIEBotException ex)
        {
            throw new AIException(AIException.ErrorCodes.ServiceError, ex.Error.Message, ex);
        }
    }

    protected virtual IAsyncEnumerable<ChatResponse> InternalCompletionsStreamAsync(List<Message> messages, double temperature, double topP, double presencePenalty)
    {
        try
        {
            return _client.ChatCompletionsStreamAsync(new ChatCompletionsRequest()
            {
                Messages = messages,
                Temperature = (float)temperature,
                TopP = (float)topP,
                PenaltyScore = (float)presencePenalty,
            });
        }
        catch (ERNIEBotException ex)
        {
            throw new AIException(AIException.ErrorCodes.ServiceError, ex.Error.Message, ex);
        }
    }

}