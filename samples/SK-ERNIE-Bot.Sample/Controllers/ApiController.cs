using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Embeddings;
using Microsoft.SemanticKernel.TextGeneration;
using Microsoft.SemanticKernel.Memory;
using SK_ERNIE_Bot.Sample.Controllers.Models;
using System.Text;
using System.Threading;

namespace SK_ERNIE_Bot.Sample.Controllers
{
    [Route("api/")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly Kernel _kernel;

        public ApiController(Kernel kernel)
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

            var chat = _kernel.GetRequiredService<IChatCompletionService>();
            var history = new ChatHistory();
            history.AddUserMessage(input.Text);

            var result = await chat.GetChatMessageContentAsync(history, cancellationToken: cancellationToken);

            return Ok(result);
        }

        [HttpPost("text")]
        public async Task<IActionResult> TextAsync([FromBody] UserInput input, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(input.Text))
            {
                return NoContent();
            }

            var completion = _kernel.GetRequiredService<ITextGenerationService>();

            var result = await completion.GetTextContentAsync(input.Text, null, cancellationToken: cancellationToken);

            var text = result.Text;
            return Ok(text);
        }

        [HttpPost("stream")]
        public async Task ChatStreamAsync([FromBody] UserInput input, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(input.Text))
            {
                await Response.CompleteAsync();
            }

            var chat = _kernel.GetRequiredService<IChatCompletionService>();
            var history = new ChatHistory();
            history.AddUserMessage(input.Text);
            var results = chat.GetStreamingChatMessageContentsAsync(history, cancellationToken: cancellationToken);

            await foreach (var result in results)
            {
                await Response.WriteAsync("data: " + result + "\n\n", Encoding.UTF8);
                await Response.Body.FlushAsync();
            }

            await Response.CompleteAsync();
        }

        [HttpPost("embedding")]
#pragma warning disable SKEXP0003 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
        public async Task<IActionResult> EmbeddingAsync([FromBody] UserInput input, [FromServices] ISemanticTextMemory memory, CancellationToken cancellationToken)
#pragma warning restore SKEXP0003 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
        {
            const string collection = "demo";
            if (string.IsNullOrWhiteSpace(input.Text))
            {
                return NoContent();
            }

            var id = await memory.SaveInformationAsync(collection, input.Text, "1", cancellationToken: cancellationToken);
            var embedding = await memory.GetAsync(collection, id, true, cancellationToken: cancellationToken);

            var result = embedding!.Embedding;

            return Ok(result!.Value.ToArray());
        }

        [HttpPost("function")]
        public async Task<IActionResult> FuncAsync([FromBody] UserInput input, CancellationToken cancellationToken)
        {
            const string prompt = """
                翻译以下内容为英文：

                {{$input}}

                """;
            var func = _kernel.CreateFunctionFromPrompt(prompt);
            var result = await _kernel.InvokeAsync(func, new() { ["input"] = input.Text }, cancellationToken: cancellationToken);
            return Ok(result.GetValue<string>());
        }

        [HttpPost("semanticPlugin")]
        public async Task<IActionResult> SemanticPlugin([FromBody] UserInput input, CancellationToken cancellationToken)
        {
            var plugin = _kernel.ImportPluginFromPromptDirectory("Plugins/Demo", "Demo");

            var translateFunc = plugin["Translate"];

            var result = await _kernel.InvokeAsync(translateFunc, new() { ["input"] = input.Text }, cancellationToken: cancellationToken);
            return Ok(result.GetValue<string>());
        }

        [HttpPost("chat_with_system")]
        public async Task<IActionResult> ChatWithSystemAsync([FromBody] UserInput input, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(input.Text))
            {
                return NoContent();
            }

            var chat = _kernel.GetRequiredService<IChatCompletionService>();

            var history = new ChatHistory($"你是一个友善的AI助手。你的名字叫做Alice，今天是{DateTime.Today}.");

            history.AddUserMessage(input.Text);

            var result = await chat.GetChatMessageContentsAsync(history, null, cancellationToken: cancellationToken);

            var text = result[0].Content;
            return Ok(text);
        }
    }
}
