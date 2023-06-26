using ERNIE_Bot.SDK;
using ERNIE_Bot.SDK.Models;

public class ERNIEBotTurboChatCompletion : ERNIEBotChatCompletion
{
    public ERNIEBotTurboChatCompletion(ERNIEBotClient client) : base(client)
    {

    }

    protected override async Task<ChatResponse> InternalCompletionsAsync(List<Message> messages, double temperature, double topP, double presencePenalty)
    {
        return await _client.ChatEBInstantAsync(new ChatRequest()
        {
            Messages = messages
        });
    }

    protected override IAsyncEnumerable<ChatResponse> InternalCompletionsStreamAsync(List<Message> messages, double temperature, double topP, double presencePenalty)
    {
        return _client.ChatEBInstantStreamAsync(new ChatRequest()
        {
            Messages = messages
        });
    }
}
