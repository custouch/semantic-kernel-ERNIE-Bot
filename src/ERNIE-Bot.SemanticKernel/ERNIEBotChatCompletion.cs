using Connectors.AI.ERNIEBot;
using ERNIE_Bot.SDK;
using ERNIE_Bot.SDK.Models;
using Microsoft.SemanticKernel.AI;
using Microsoft.SemanticKernel.AI.ChatCompletion;
using Microsoft.SemanticKernel.AI.TextCompletion;
using Microsoft.SemanticKernel.Diagnostics;
using System.Runtime.CompilerServices;

public class ERNIEBotChatCompletion : IChatCompletion, ITextCompletion
{
    protected readonly ERNIEBotClient _client;
    private readonly ModelEndpoint _modelEndpoint;
    private readonly Dictionary<string, string> _attributes = new();

    public IReadOnlyDictionary<string, string> Attributes => this._attributes;

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
            history.AddSystemMessage(instructions);
        }

        return history;
    }

    public async Task<IReadOnlyList<IChatResult>> GetChatCompletionsAsync(ChatHistory chat, AIRequestSettings? requestSettings = null, CancellationToken cancellationToken = default)
    {
        var messages = ChatHistoryToMessages(chat, out var system);
        requestSettings ??= new AIRequestSettings();

        var settings = ERNIEBotAIRequestSettings.FromRequestSettings(requestSettings);

        ChatResponse result = await InternalCompletionsAsync(messages,
                                                             settings.Temperature,
                                                             settings.TopP,
                                                             settings.PenaltyScore,
                                                             system,
                                                             cancellationToken
                                                             );
        return new List<IChatResult>() { new ERNIEBotChatResult(result) };
    }

    public async Task<IReadOnlyList<ITextResult>> GetCompletionsAsync(string text, AIRequestSettings? requestSettings, CancellationToken cancellationToken = default)
    {
        requestSettings ??= new AIRequestSettings();
        var messages = StringToMessages(text);

        var settings = ERNIEBotAIRequestSettings.FromRequestSettings(requestSettings);

        var result = await InternalCompletionsAsync(messages,
                                                    settings.Temperature,
                                                    settings.TopP,
                                                    settings.PenaltyScore,
                                                    null,
                                                    cancellationToken
                                                    );

        return new List<ITextResult>() { new ERNIEBotChatResult(result) }.AsReadOnly();
    }

    public async IAsyncEnumerable<IChatStreamingResult> GetStreamingChatCompletionsAsync(ChatHistory chat, AIRequestSettings? requestSettings = null, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var messages = ChatHistoryToMessages(chat, out var system);

        requestSettings ??= new AIRequestSettings();

        var settings = ERNIEBotAIRequestSettings.FromRequestSettings(requestSettings);

        var results = InternalCompletionsStreamAsync(messages,
                                                     settings.Temperature,
                                                     settings.TopP,
                                                     settings.PenaltyScore,
                                                     system,
                                                     cancellationToken
                                                    );

        yield return new ERNIEBotChatResult(results);
        await Task.CompletedTask;
    }

    public async IAsyncEnumerable<ITextStreamingResult> GetStreamingCompletionsAsync(string text, AIRequestSettings? requestSettings, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var messages = StringToMessages(text);
        requestSettings ??= new AIRequestSettings();

        var settings = ERNIEBotAIRequestSettings.FromRequestSettings(requestSettings);

        var results = InternalCompletionsStreamAsync(messages,
                                                     settings.Temperature,
                                                     settings.TopP,
                                                     settings.PenaltyScore,
                                                     null,
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
                Content = m.Content
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
            throw new SKException(ex.Error.Message, ex);
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
            throw new SKException(ex.Error.Message, ex);
        }
    }
}
