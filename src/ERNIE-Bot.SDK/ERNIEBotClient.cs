using ERNIE_Bot.SDK.Models;
using IdentityModel.Client;
using Microsoft;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ERNIE_Bot.SDK
{
    public class ERNIEBotClient
    {
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly HttpClient _client;
        private readonly ITokenStore _tokenStore;

        public ERNIEBotClient(string clientId, string clientSecret, HttpClient client, ITokenStore tokenStore)
        {
            Requires.NotNullOrWhiteSpace(clientId, nameof(clientId));
            Requires.NotNullOrWhiteSpace(clientSecret, nameof(clientSecret));

            Requires.NotNull(client, nameof(HttpClient));
            Requires.NotNull(tokenStore, nameof(ITokenStore));

            this._clientId = clientId;
            this._clientSecret = clientSecret;
            this._client = client;
            this._tokenStore = tokenStore;
        }


        /// <summary>
        /// Api for ERNIE-Bot
        /// </summary>
        /// <returns></returns>
        public async Task<ChatResponse> ChatCompletionsAsync(ChatCompletionsRequest request, CancellationToken cancellationToken)
        {
            if (request.Stream.HasValue && request.Stream.Value)
            {
                request.Stream = false;
            }

            var webRequest = await CreateRequestAsync(HttpMethod.Post, Defaults.ERNIEBotEndpoint, request);

            var response = await _client.SendAsync(webRequest);

            return await ParseResponseAsync<ChatResponse>(response);
        }

        /// <summary>
        /// Api for ERNIE-Bot Stream
        /// </summary>
        /// <returns></returns>
        public async IAsyncEnumerable<ChatResponse> ChatCompletionsStreamAsync(ChatCompletionsRequest request, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            request.Stream = true;
            var webRequest = await CreateRequestAsync(HttpMethod.Post, Defaults.ERNIEBotEndpoint, request, cancellationToken);

            var response = await _client.SendAsync(webRequest, cancellationToken);

            await foreach (var item in ParseResponseStreamAsync(response, cancellationToken))
            {
                yield return item;
            }
        }

        /// <summary>
        /// Api for ERNIE-Bot-turbo 
        /// </summary>
        /// <returns></returns>
        public async Task<ChatResponse> ChatEBInstantAsync(ChatRequest request)
        {
            if (request.Stream.HasValue && request.Stream.Value)
            {
                request.Stream = false;
            }

            var webRequest = await CreateRequestAsync(HttpMethod.Post, Defaults.ERNIEBotTurboEndpoint, request);

            var response = await _client.SendAsync(webRequest);

            return await ParseResponseAsync<ChatResponse>(response);
        }

        /// <summary>
        /// Api for ERNIE-Bot-turbo Stream
        /// </summary>
        /// <returns></returns>
        public async IAsyncEnumerable<ChatResponse> ChatEBInstantStreamAsync(ChatRequest request, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            request.Stream = true;

            var webRequest = await CreateRequestAsync(HttpMethod.Post, Defaults.ERNIEBotTurboEndpoint, request, cancellationToken);

            var response = await _client.SendAsync(webRequest, cancellationToken);

            await foreach (var item in ParseResponseStreamAsync(response, cancellationToken))
            {
                yield return item;
            }
        }

        /// <summary>
        /// Embedding V1 Api for ERNIE-Bot
        /// </summary>
        /// <returns></returns>
        public async Task<EmbeddingsResponse> EmbeddingsAsync(EmbeddingsRequest request)
        {
            var webRequest = await CreateRequestAsync(HttpMethod.Post, Defaults.EmbeddingV1Endpoint, request);

            var response = await _client.SendAsync(webRequest);

            return await ParseResponseAsync<EmbeddingsResponse>(response);
        }

        /// <summary>
        /// Get Access Token
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="HttpRequestException"></exception>
        public async Task<string> GetAccessTokenAsync(CancellationToken cancellationToken = default)
        {
            var token = await _tokenStore.GetTokenAsync(cancellationToken);

            if (!string.IsNullOrWhiteSpace(token))
            {
                return token;
            }

            var urlBuilder = new RequestUrl(Defaults.AccessTokenEndpoint);
            var url = urlBuilder.Create(new Parameters
            {
                {"client_id",_clientId },
                {"client_secret",_clientSecret },
                {"grant_type","client_credentials" }
            });

            var requestToken = await _client.RequestTokenAsync(
                new TokenRequest()
                {
                    Address = url
                }) ?? throw new HttpRequestException($"Failed to get access token");

            if (requestToken.IsError || requestToken.AccessToken == null)
            {
                throw new HttpRequestException($"Failed to get access token: {requestToken.Error}");
            }

            await _tokenStore.SaveTokenAsync(requestToken.AccessToken, TimeSpan.FromSeconds(requestToken.ExpiresIn), cancellationToken);

            return requestToken.AccessToken;
        }

        #region ===== private methods =====


        private async Task<HttpRequestMessage> CreateRequestAsync<TRequest>(HttpMethod method, string path, TRequest? body = null, CancellationToken cancellationToken = default) where TRequest : class
        {
            var accessToken = await GetAccessTokenAsync(cancellationToken);

            var uri = path + "?access_token=" + accessToken;

            var request = new HttpRequestMessage(method, uri);

            if (body != null)
            {
                request.Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
            }

            return request;
        }

        private async Task<TResponse> ParseResponseAsync<TResponse>(HttpResponseMessage response)
        {
            var responseJson = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<TResponse>(responseJson);
                if (result != null)
                {
                    return result;
                }
                else
                {
                    throw new ERNIEBotException(-1, "Invalid response content");
                }
            }
            else
            {
                var error = JsonSerializer.Deserialize<ERNIEBotError>(responseJson);

                throw new ERNIEBotException(error);
            }
        }

        private async IAsyncEnumerable<ChatResponse> ParseResponseStreamAsync(HttpResponseMessage response, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            if (!response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();

                var error = JsonSerializer.Deserialize<ERNIEBotError>(responseJson);

                throw new ERNIEBotException(error);
            }

            await using var stream = await response.Content.ReadAsStreamAsync();
            using var reader = new StreamReader(stream);

            while (!reader.EndOfStream)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var line = await reader.ReadLineAsync();

                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }

                line = RemovePrefix(line, "data: ");

                ChatResponse? block;
                try
                {
                    block = JsonSerializer.Deserialize<ChatResponse>(line);
                }
                catch (Exception)
                {
                    throw new ERNIEBotException(-1, "Invalid response content");
                }

                if (block != null)
                {
                    yield return block;

                    if (block.IsEnd.HasValue && block.IsEnd.Value)
                    {
                        break;
                    };
                }
            }
        }

        private string RemovePrefix(string text, string prefix)
        {
            if (text.StartsWith(prefix))
            {
                return text.Substring(prefix.Length);
            }
            else
            {
                return text;
            }
        }
        #endregion

    }
}
