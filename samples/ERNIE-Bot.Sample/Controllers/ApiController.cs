using ERNIE_Bot.Sample.Controllers.Models;
using ERNIE_Bot.SDK;
using ERNIE_Bot.SDK.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    }
}
