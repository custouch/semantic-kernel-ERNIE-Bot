using Connectors.AI.ERNIEBot;
using ERNIE_Bot.SDK;
using ERNIE_Bot.SDK.Models;
using Microsoft.SemanticKernel.AI;
using Microsoft.SemanticKernel.AI.ChatCompletion;
using Microsoft.SemanticKernel.AI.TextCompletion;
using Microsoft.SemanticKernel.Diagnostics;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

public class ERNIEBotChatCompletion : IChatCompletion, ITextCompletion
{
    protected readonly ERNIEBotClient _client;
    private readonly ModelEndpoint _modelEndpoint;

    public ERNIEBotChatCompletion(ERNIEBotClient client, ModelEndpoint? modelEndpoint = null)
    {
        this._client = client;

        this._modelEndpoint = modelEndpoint ?? ModelEndpoints.ERNIE_Bot;
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


    public async Task<IReadOnlyList<IChatResult>> GetChatCompletionsAsync(ChatHistory chat, AIRequestSettings? requestSettings = null, CancellationToken cancellationToken = default)
    {
        var messages = ChatHistoryToMessages(chat);
        requestSettings ??= new AIRequestSettings();

        var (temperature, topP, penaltyScore) = ParseRequestSettings(requestSettings.ExtensionData);

        ChatResponse result = await InternalCompletionsAsync(messages,
                                                             temperature,
                                                             topP,
                                                             penaltyScore,
                                                             cancellationToken
                                                             );
        return new List<ERNIEBotChatResult>() { new ERNIEBotChatResult(result) };
    }



    public async Task<IReadOnlyList<ITextResult>> GetCompletionsAsync(string text, AIRequestSettings? requestSettings, CancellationToken cancellationToken = default)
    {
        requestSettings ??= new AIRequestSettings();
        var messages = StringToMessages(text);

        var (temperature, topP, penaltyScore) = ParseRequestSettings(requestSettings.ExtensionData);

        var result = await InternalCompletionsAsync(messages,
                                                    temperature,
                                                    topP,
                                                    penaltyScore,
                                                    cancellationToken
                                                    );

        return new List<ERNIEBotChatResult>() { new ERNIEBotChatResult(result) };
    }

    public async IAsyncEnumerable<IChatStreamingResult> GetStreamingChatCompletionsAsync(ChatHistory chat, AIRequestSettings? requestSettings = null, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var messages = ChatHistoryToMessages(chat);
        requestSettings ??= new AIRequestSettings();

        var (temperature, topP, penaltyScore) = ParseRequestSettings(requestSettings.ExtensionData);

        var results = InternalCompletionsStreamAsync(messages,
                                                     temperature,
                                                     topP,
                                                     penaltyScore,
                                                     cancellationToken
                                                    );

        yield return new ERNIEBotChatResult(results);
        await Task.CompletedTask;
    }

    public async IAsyncEnumerable<ITextStreamingResult> GetStreamingCompletionsAsync(string text, AIRequestSettings? requestSettings, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var messages = StringToMessages(text);
        requestSettings ??= new AIRequestSettings();

        var (temperature, topP, penaltyScore) = ParseRequestSettings(requestSettings.ExtensionData);

        var results = InternalCompletionsStreamAsync(messages,
                                                     temperature,
                                                     topP,
                                                     penaltyScore,
                                                     cancellationToken
                                                     );

        yield return new ERNIEBotChatResult(results);
        await Task.CompletedTask;
    }

    private (float? temperature, float? TopP, float? penaltyScore) ParseRequestSettings(Dictionary<string, object> extensionData)
    {
        float? TryGetValue(string key, float? defaultValue = null)
        {
            if (extensionData.TryGetValue(key, out var value) && value is float t)
            {
                return t;
            }

            return defaultValue;
        }

        float? temperature = TryGetValue("temperature");
        float? topP = TryGetValue("top_p");
        float? penaltyScore = TryGetValue("penalty_score");

        return (temperature, topP, penaltyScore);
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

    protected virtual async Task<ChatResponse> InternalCompletionsAsync(List<Message> messages, float? temperature, float? topP, float? penaltyScore, CancellationToken cancellationToken)
    {
        try
        {
            return await _client.ChatAsync(new ChatCompletionsRequest()
            {
                Messages = messages,
                Temperature = temperature,
                TopP = topP,
                PenaltyScore = penaltyScore,
            }, _modelEndpoint, cancellationToken);
        }
        catch (ERNIEBotException ex)
        {
            throw new SKException(ex.Error.Message, ex);
        }
    }

    protected virtual IAsyncEnumerable<ChatResponse> InternalCompletionsStreamAsync(List<Message> messages, float? temperature, float? topP, float? penaltyScore, CancellationToken cancellationToken)
    {
        try
        {
            return _client.ChatStreamAsync(new ChatCompletionsRequest()
            {
                Messages = messages,
                Temperature = temperature,
                TopP = topP,
                PenaltyScore = penaltyScore,
            }, _modelEndpoint, cancellationToken);
        }
        catch (ERNIEBotException ex)
        {
            throw new SKException(ex.Error.Message, ex);
        }
    }
}