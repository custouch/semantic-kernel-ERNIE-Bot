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

            var result = await _client.ChatAsync(new ChatCompletionsRequest()
            {
                Messages = new List<Message>
                 {
                      new Message()
                      {
                           Content = input.Text,
                           Role = MessageRole.User
                      }
                 }
            }, ModelEndpoints.ERNIE_Bot);

            return Ok(result.Result);
        }

        [HttpPost("ChatTurbo")]
        public async Task<IActionResult> ChatTurboAsync([FromBody] UserInput input)
        {
            if (string.IsNullOrWhiteSpace(input.Text))
            {
                return NoContent();
            }

            var result = await _client.ChatAsync(new ChatCompletionsRequest()
            {
                Messages = new List<Message>
                 {
                      new Message()
                      {
                           Content = input.Text,
                           Role = MessageRole.User
                      }
                 }
            }, ModelEndpoints.ERNIE_Bot_Turbo);

            return Ok(result.Result);
        }

        [HttpPost("ChatPro")]
        public async Task<IActionResult> ChatProAsync([FromBody] UserInput input)
        {
            if (string.IsNullOrWhiteSpace(input.Text))
            {
                return NoContent();
            }

            var result = await _client.ChatAsync(new ChatCompletionsRequest()
            {
                Messages = new List<Message>
                 {
                      new Message()
                      {
                           Content = input.Text,
                           Role = MessageRole.User
                      }
                 }
            }, ModelEndpoints.ERNIE_Bot_4);

            return Ok(result.Result);
        }

        [HttpPost("ChatBLOOMZ")]
        public async Task<IActionResult> ChatBLOOMZAsync([FromBody] UserInput input)
        {
            if (string.IsNullOrWhiteSpace(input.Text))
            {
                return NoContent();
            }

            var result = await _client.ChatAsync(new ChatCompletionsRequest()
            {
                Messages = new List<Message>
                 {
                      new Message()
                      {
                           Content = input.Text,
                           Role = MessageRole.User
                      }
                 }
            }, ModelEndpoints.BLOOMZ_7B);

            return Ok(result.Result);
        }

        [HttpPost("ChatStream")]
        public async Task ChatStreamAsync([FromBody] UserInput input)
        {
            if (string.IsNullOrWhiteSpace(input.Text))
            {
                await Response.CompleteAsync();
            }

            var results = _client.ChatStreamAsync(new ChatCompletionsRequest()
            {
                Messages = new List<Message>
                 {
                      new Message()
                      {
                           Content = input.Text,
                           Role = MessageRole.User
                      }
                 }
            }, ModelEndpoints.ERNIE_Bot);

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
            }, ModelEndpoints.bge_large_en);

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

            var result = await _client.ChatAsync(new ChatCompletionsRequest()
            {
                Messages = messages.ToList()
            }, ModelEndpoints.ERNIE_Bot);

            return Ok(result.Result);
        }
    }
}
