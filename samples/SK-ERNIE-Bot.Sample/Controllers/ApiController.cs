using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.AI.ChatCompletion;
using SK_ERNIE_Bot.Sample.Controllers.Models;

namespace SK_ERNIE_Bot.Sample.Controllers
{
    [Route("api/")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly IKernel _kernel;

        public ApiController(IKernel kernel)
        {
            this._kernel = kernel;
        }

        [HttpPost]
        public async Task<IActionResult> ChatAsync([FromBody] UserInput input, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(input.Text))
            {
                return NoContent();
            }

            var chat = _kernel.GetService<IChatCompletion>();
            var history = chat.CreateNewChat();
            history.AddUserMessage(input.Text);

            var result = await chat.GenerateMessageAsync(history, cancellationToken: cancellationToken);

            return Ok(result);

        }
    }
}
