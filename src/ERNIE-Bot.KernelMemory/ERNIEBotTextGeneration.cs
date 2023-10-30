using ERNIE_Bot.SDK;
using Microsoft.KernelMemory.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ERNIE_Bot.KernelMemory
{
    /// <summary>
    /// Generating text using ERNIEBotClient.
    /// </summary>
    public class ERNIEBotTextGeneration : ITextGeneration
    {
        private readonly ERNIEBotClient _client;
        private readonly ModelEndpoint _endpoint;

        /// <summary>
        /// Initializes a new instance of the <see cref="ERNIEBotTextGeneration"/> class.
        /// </summary>
        /// <param name="client">The ERNIEBotClient instance to use for generating text.</param>
        /// <param name="endpoint">The endpoint to use for the model. Defaults to ModelEndpoints.ERNIE_Bot_Turbo.</param>
        public ERNIEBotTextGeneration(ERNIEBotClient client, ModelEndpoint? endpoint = null)
        {
            this._client = client;
            _endpoint = endpoint ?? ModelEndpoints.ERNIE_Bot_Turbo;
        }

        /// <inheritdoc/>
        public async IAsyncEnumerable<string> GenerateTextAsync(string prompt, TextGenerationOptions options, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var result = _client.ChatStreamAsync(new SDK.Models.ChatRequest()
            {
                Messages = new List<SDK.Models.Message>()
                 {
                     new SDK.Models.Message()
                     {
                          Role = SDK.Models.MessageRole.User,
                          Content = prompt
                     }
                 },
            }, _endpoint, cancellationToken);

            await foreach (var response in result)
            {
                yield return response.Result;
            }
        }
    }
}
