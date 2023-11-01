using ERNIE_Bot.SDK;
using Microsoft.SemanticKernel.AI.Embeddings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERNIE_Bot.SDK.Models;
using Polly;
using Polly.Retry;

namespace ERNIE_Bot.KernelMemory
{
    /// <summary>
    /// Generating text embeddings using ERNIEBotClient.
    /// </summary>
    public class ERNIEBotTextEmbeddingGeneration : ITextEmbeddingGeneration
    {
        private readonly ERNIEBotClient _client;
        private readonly EmbeddingModelEndpoint _endpoint;
        private readonly ResiliencePipeline<EmbeddingsResponse> _embeddingsPipeline;
        

        /// <summary>
        /// Initializes a new instance of the <see cref="ERNIEBotTextEmbeddingGeneration"/> class.
        /// </summary>
        /// <param name="client">The ERNIEBotClient instance to use for generating embeddings.</param>
        /// <param name="endpoint">The endpoint to use for the embedding model. Defaults to ModelEndpoints.Embedding_v1.</param>
        public ERNIEBotTextEmbeddingGeneration(ERNIEBotClient client, EmbeddingModelEndpoint? endpoint = null)
        {
            this._client = client;
            _endpoint = endpoint ?? ModelEndpoints.Embedding_v1;
            //构建一个pipeline，返回值为EmbeddingsResponse
            _embeddingsPipeline = new ResiliencePipelineBuilder<EmbeddingsResponse>()
                //给pipeline添加一个重试策略
                .AddRetry(new RetryStrategyOptions<EmbeddingsResponse>()
                {
                    //这个pipeline的作用是在遇到qps异常和 Embeddings internal error 时进行重试
                    ShouldHandle = new PredicateBuilder<EmbeddingsResponse>()
                        .Handle<ERNIEBotException>(ex => ex.Error.Code is 18 or 336200),
                    //重试3次，每次间隔1秒
                    MaxRetryAttempts = 3,
                    Delay = TimeSpan.FromSeconds(1),
                    //重试间隔为固定值
                    BackoffType = DelayBackoffType.Constant,
                    //重试时的回调
                    OnRetry = args =>
                    {
                        // Console.WriteLine($"遇到了qps异常，即将在 {args.RetryDelay.TotalSeconds} 秒后进行第 {args.AttemptNumber + 1} 次重试，异常的具体内容为：\n{args.Outcome.Exception}");
                        return default;
                    }
                })
                .Build();
        }

        /// <inheritdoc/>
        public async Task<IList<ReadOnlyMemory<float>>> GenerateEmbeddingsAsync(IList<string> data,
            CancellationToken cancellationToken = default)
        {
            //kernel-memory会把一个document分成多个paragraph，以\r\n分割
            //每个paragraph里有多个line
            //每个line分成多个token
            //API要求，最多有16个paragraph，每个paragraph最多有384个token
            //这里的data是一个document的所有token
            //所以先把它分成paragraph，因为API的限制，只取前16个paragraph
            List<string> input = new(data[0].Split("\r\n", StringSplitOptions.RemoveEmptyEntries).Take(16));
            //执行pipeline，返回值为EmbeddingsResponse，重试策略在pipeline中执行
            var embeddings = await _embeddingsPipeline.ExecuteAsync(async _ =>
                    await _client.EmbeddingsAsync(new EmbeddingsRequest()
                    {
                        Input = input
                    }, _endpoint, cancellationToken)
                , cancellationToken);//这里的cancellationToken是用来取消任务的，如果不需要取消任务，可以不传

            return embeddings.Data.Select(d => new ReadOnlyMemory<float>(d.Embedding.Select(e => (float)e).ToArray())).ToList();
        }
    }
}