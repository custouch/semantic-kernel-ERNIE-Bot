using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.AI.ChatCompletion;
using Microsoft.SemanticKernel.AI.Embeddings;
using Microsoft.SemanticKernel.AI.TextCompletion;
using SK_ERNIE_Bot.Sample.Controllers.Models;
using System.Text;

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

        [HttpPost("text")]
        public async Task<IActionResult> TextAsync([FromBody] UserInput input, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(input.Text))
            {
                return NoContent();
            }

            var completion = _kernel.GetService<ITextCompletion>();

            var result = await completion.GetCompletionsAsync(input.Text, new CompleteRequestSettings(), cancellationToken: cancellationToken);

            var text = await result.First().GetCompletionAsync();
            return Ok(text);

        }

        [HttpPost("stream")]
        public async Task ChatStreamAsync([FromBody] UserInput input, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(input.Text))
            {
                await Response.CompleteAsync();
            }

            var chat = _kernel.GetService<IChatCompletion>();
            var history = chat.CreateNewChat();
            history.AddUserMessage(input.Text);

            var results = chat.GenerateMessageStreamAsync(history, cancellationToken: cancellationToken);

            await foreach (var result in results)
            {
                await Response.WriteAsync("data: " + result + "\n\n", Encoding.UTF8);
                await Response.Body.FlushAsync();
            }

            await Response.CompleteAsync();
        }

        [HttpPost("embedding")]
        public async Task<IActionResult> EmbeddingAsync([FromBody] UserInput input, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(input.Text))
            {
                return NoContent();
            }

            var embedding = _kernel.GetService<ITextEmbeddingGeneration>();

            var result = await embedding.GenerateEmbeddingAsync(input.Text, cancellationToken);

            return Ok(result);
        }
    }
}
