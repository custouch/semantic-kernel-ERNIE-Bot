using Connectors.AI.ERNIEBot;
using ERNIE_Bot.SDK;
using ERNIE_Bot.SDK.Models;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.TextGeneration;
using Microsoft.SemanticKernel.Services;
using System;
using System.Runtime.CompilerServices;

public class ERNIEBotChatCompletion : IChatCompletionService, ITextGenerationService
{
    protected readonly ERNIEBotClient _client;
    private readonly ModelEndpoint _modelEndpoint;
    private readonly Dictionary<string, string> _attributes = new();

    public IReadOnlyDictionary<string, string> Attributes => this._attributes;

    IReadOnlyDictionary<string, object?> IAIService.Attributes => throw new NotImplementedException();

    public ERNIEBotChatCompletion(ERNIEBotClient client, ModelEndpoint? modelEndpoint = null)
    {
        this._client = client;

        this._modelEndpoint = modelEndpoint ?? ModelEndpoints.ERNIE_Bot;
    }

    public async Task<IReadOnlyList<ChatMessageContent>> GetChatMessageContentsAsync(ChatHistory chat, PromptExecutionSettings? executionSettings = null, Kernel? kernel = null, CancellationToken cancellationToken = default)
    {
        var messages = ChatHistoryToMessages(chat, out var system);
        executionSettings ??= new PromptExecutionSettings();

        var settings = ERNIEBotAIRequestSettings.FromRequestSettings(executionSettings);

        ChatResponse result = await InternalCompletionsAsync(messages,
                                                             settings.Temperature,
                                                             settings.TopP,
                                                             settings.PenaltyScore,
                                                             system,
                                                             cancellationToken
                                                             );

        return new List<ChatMessageContent>() { new ERNIEBotChatMessage(result) };
    }

    public async IAsyncEnumerable<StreamingChatMessageContent> GetStreamingChatMessageContentsAsync(ChatHistory chatHistory, PromptExecutionSettings? executionSettings = null, Kernel? kernel = null, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var messages = ChatHistoryToMessages(chatHistory, out var system);

        executionSettings ??= new PromptExecutionSettings();

        var settings = ERNIEBotAIRequestSettings.FromRequestSettings(executionSettings);

        var results = InternalCompletionsStreamAsync(messages,
                                                     settings.Temperature,
                                                     settings.TopP,
                                                     settings.PenaltyScore,
                                                     system,
                                                     cancellationToken
                                                    );
        await foreach (var result in results)
        {
            yield return new ERNIEBotStreamingChatMessage(result);
        }
    }

    public async Task<IReadOnlyList<TextContent>> GetTextContentsAsync(string prompt, PromptExecutionSettings? executionSettings = null, Kernel? kernel = null, CancellationToken cancellationToken = default)
    {
        executionSettings ??= new PromptExecutionSettings();
        var messages = StringToMessages(prompt);

        var settings = ERNIEBotAIRequestSettings.FromRequestSettings(executionSettings);

        var result = await InternalCompletionsAsync(messages,
                                                    settings.Temperature,
                                                    settings.TopP,
                                                    settings.PenaltyScore,
                                                    null,
                                                    cancellationToken
                                                    );

        return new List<TextContent>() { new(result.Result) }.AsReadOnly();
    }

    public async IAsyncEnumerable<StreamingTextContent> GetStreamingTextContentsAsync(string prompt, PromptExecutionSettings? executionSettings = null, Kernel? kernel = null, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var messages = StringToMessages(prompt);
        executionSettings ??= new PromptExecutionSettings();

        var settings = ERNIEBotAIRequestSettings.FromRequestSettings(executionSettings);

        var results = InternalCompletionsStreamAsync(messages,
                                                     settings.Temperature,
                                                     settings.TopP,
                                                     settings.PenaltyScore,
                                                     null,
                                                     cancellationToken
                                                     );
        await foreach (var result in results)
        {
            yield return new StreamingTextContent(result.Result);
        }
    }


    public ChatHistory CreateNewChat(string? instructions = null)
    {
        var history = new ChatHistory();

        if (instructions != null)
        {
            history.AddSystemMessage(instructions);
        }

        return history;
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

    private List<Message> ChatHistoryToMessages(ChatHistory chatHistory, out string? system)
    {
        if (chatHistory.First().Role == AuthorRole.System)
        {
            system = chatHistory.First().Content;
        }
        else
        {
            system = null;
        }
        return chatHistory
            .Where(_ => _.Role != AuthorRole.System)
            .Select(m => new Message()
            {
                Role = AuthorRoleToMessageRole(m.Role),
                Content = m.Content!
            }).ToList();
    }

    private string AuthorRoleToMessageRole(AuthorRole role)
    {
        if (role == AuthorRole.User) return MessageRole.User;
        if (role == AuthorRole.Assistant) return MessageRole.Assistant;
        return MessageRole.User;
    }

    protected virtual async Task<ChatResponse> InternalCompletionsAsync(List<Message> messages, float? temperature, float? topP, float? penaltyScore, string? system, CancellationToken cancellationToken)
    {
        try
        {
            return await _client.ChatAsync(new ChatCompletionsRequest()
            {
                Messages = messages,
                Temperature = temperature,
                TopP = topP,
                PenaltyScore = penaltyScore,
                System = system,
            }, _modelEndpoint, cancellationToken);
        }
        catch (ERNIEBotException ex)
        {
            throw new KernelException(ex.Error.Message, ex);
        }
    }

    protected virtual IAsyncEnumerable<ChatResponse> InternalCompletionsStreamAsync(List<Message> messages, float? temperature, float? topP, float? penaltyScore, string? system, CancellationToken cancellationToken)
    {
        try
        {
            return _client.ChatStreamAsync(new ChatCompletionsRequest()
            {
                Messages = messages,
                Temperature = temperature,
                TopP = topP,
                PenaltyScore = penaltyScore,
                System = system,
            }, _modelEndpoint, cancellationToken);
        }
        catch (ERNIEBotException ex)
        {
            throw new KernelException(ex.Error.Message, ex);
        }
    }

}
