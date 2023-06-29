using ERNIE_Bot.Sample.Controllers.Models;
using ERNIE_Bot.SDK;
using ERNIE_Bot.SDK.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace ERNIE_Bot.Sample.Controllers
{
    [Route("api")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly ERNIEBotClient _client;

        public ApiController(ERNIEBotClient client)
        {
            this._client = client;
        }

        [HttpPost("Chat")]
        public async Task<IActionResult> ChatAsync([FromBody] UserInput input)
        {
            if (string.IsNullOrWhiteSpace(input.Text))
            {
                return NoContent();
            }

            var result = await _client.ChatCompletionsAsync(new ChatCompletionsRequest()
            {
                Messages = new List<Message>
                 {
                      new Message()
                      {
                           Content = input.Text,
                           Role = MessageRole.User
                      }
                 }
            });

            return Ok(result.Result);
        }

        [HttpPost("ChatTurbo")]
        public async Task<IActionResult> ChatTurboAsync([FromBody] UserInput input)
        {
            if (string.IsNullOrWhiteSpace(input.Text))
            {
                return NoContent();
            }

            var result = await _client.ChatEBInstantAsync(new ChatCompletionsRequest()
            {
                Messages = new List<Message>
                 {
                      new Message()
                      {
                           Content = input.Text,
                           Role = MessageRole.User
                      }
                 }
            });

            return Ok(result.Result);
        }

        [HttpPost("ChatStream")]
        public async Task ChatStreamAsync([FromBody] UserInput input)
        {
            if (string.IsNullOrWhiteSpace(input.Text))
            {
                await Response.CompleteAsync();
            }

            var results = _client.ChatCompletionsStreamAsync(new ChatCompletionsRequest()
            {
                Messages = new List<Message>
                 {
                      new Message()
                      {
                           Content = input.Text,
                           Role = MessageRole.User
                      }
                 }
            });

            await foreach (var result in results)
            {
                await Response.WriteAsync("data: " + result.Result + "\n\n", Encoding.UTF8);
                await Response.Body.FlushAsync();
            }

            await Response.CompleteAsync();
        }

        [HttpPost("Embedding")]
        public async Task<IActionResult> EmbeddingAsync([FromBody] UserInput input)
        {
            if (string.IsNullOrWhiteSpace(input.Text))
            {
                return NoContent();
            }

            var result = await _client.EmbeddingsAsync(new EmbeddingsRequest()
            {
                Input = new List<string>()
                 {
                      input.Text
                 }
            });

            return Ok(result.Data.FirstOrDefault()?.Embedding);
        }

        [HttpPost("History")]
        public async Task<IActionResult> HistoryAsync([FromBody] UserHistoryInput input)
        {

            if (!input.Messages.Any())
            {
                return NoContent();
            }

            var messages = input.Messages.Select(_ => new Message()
            {
                Role = _.Role,
                Content = _.Text
            });

            var result = await _client.ChatEBInstantAsync(new ChatCompletionsRequest()
            {
                Messages = messages.ToList()
            });

            return Ok(result.Result);
        }
    }
}
