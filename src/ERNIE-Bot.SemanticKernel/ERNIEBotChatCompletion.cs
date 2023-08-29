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
    private readonly string _modelEndpoint;

    public ERNIEBotChatCompletion(ERNIEBotClient client, string modelEndpoint = ModelEndpoints.ERNIE_Bot)
    {
        this._client = client;
        this._modelEndpoint = modelEndpoint;
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
                                                             requestSettings.PresencePenalty,
                                                             cancellationToken
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
                                                             requestSettings.PresencePenalty,
                                                             cancellationToken
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
                                                    requestSettings.PresencePenalty,
                                                    cancellationToken
                                                    );

        yield return new ERNIEBotChatResult(results);
        await Task.CompletedTask;
    }

    public async IAsyncEnumerable<ITextStreamingResult> GetStreamingCompletionsAsync(string text, CompleteRequestSettings requestSettings, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var messages = StringToMessages(text);
        requestSettings ??= new CompleteRequestSettings();

        var results = InternalCompletionsStreamAsync(messages,
                                                     requestSettings.Temperature,
                                                     requestSettings.TopP,
                                                     requestSettings.PresencePenalty,
                                                     cancellationToken
                                                     );

        yield return new ERNIEBotChatResult(results);
        await Task.CompletedTask;
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

    protected virtual async Task<ChatResponse> InternalCompletionsAsync(List<Message> messages, double temperature, double topP, double presencePenalty, CancellationToken cancellationToken)
    {
        try
        {
            return await _client.ChatAsync(new ChatCompletionsRequest()
            {
                Messages = messages,
                Temperature = (float)temperature,
                TopP = (float)topP,
                PenaltyScore = (float)presencePenalty,
            }, _modelEndpoint, cancellationToken);
        }
        catch (ERNIEBotException ex)
        {
            throw new AIException(AIException.ErrorCodes.ServiceError, ex.Error.Message, ex);
        }
    }

    protected virtual IAsyncEnumerable<ChatResponse> InternalCompletionsStreamAsync(List<Message> messages, double temperature, double topP, double presencePenalty, CancellationToken cancellationToken)
    {
        try
        {
            return _client.ChatStreamAsync(new ChatCompletionsRequest()
            {
                Messages = messages,
                Temperature = (float)temperature,
                TopP = (float)topP,
                PenaltyScore = (float)presencePenalty,
            }, _modelEndpoint, cancellationToken);
        }
        catch (ERNIEBotException ex)
        {
            throw new AIException(AIException.ErrorCodes.ServiceError, ex.Error.Message, ex);
        }
    }

}