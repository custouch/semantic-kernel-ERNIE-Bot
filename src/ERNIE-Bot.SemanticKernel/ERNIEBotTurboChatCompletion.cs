using ERNIE_Bot.SDK;
using ERNIE_Bot.SDK.Models;
using Microsoft.SemanticKernel.AI;

[Obsolete("Please use ERNIEBotChatCompletion instead")]
public class ERNIEBotTurboChatCompletion : ERNIEBotChatCompletion
{
    public ERNIEBotTurboChatCompletion(ERNIEBotClient client) : base(client)
    {

    }

    protected override async Task<ChatResponse> InternalCompletionsAsync(List<Message> messages, double temperature, double topP, double presencePenalty, CancellationToken cancellationToken)
    {
        try
        {
            return await _client.ChatEBInstantAsync(new ChatRequest()
            {
                Messages = messages
            }, cancellationToken);
        }
        catch (ERNIEBotException ex)
        {
            throw new AIException(AIException.ErrorCodes.ServiceError, ex.Error.Message, ex);
        }
    }

    protected override IAsyncEnumerable<ChatResponse> InternalCompletionsStreamAsync(List<Message> messages, double temperature, double topP, double presencePenalty, CancellationToken cancellationToken)
    {
        try
        {
            return _client.ChatEBInstantStreamAsync(new ChatRequest()
            {
                Messages = messages
            }, cancellationToken);
        }
        catch (ERNIEBotException ex)
        {
            throw new AIException(AIException.ErrorCodes.ServiceError, ex.Error.Message, ex);
        }
    }
}
