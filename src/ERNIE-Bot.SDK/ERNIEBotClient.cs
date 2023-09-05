﻿using ERNIE_Bot.SDK.Models;
using Microsoft;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ERNIE_Bot.SDK
{
    public class ERNIEBotClient
    {
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly HttpClient _client;
        private readonly ITokenStore _tokenStore;
        private readonly ILogger _logger;

        public ERNIEBotClient(string clientId, string clientSecret, HttpClient? client = null, ITokenStore? tokenStore = null, ILogger<ERNIEBotClient>? logger = null)
        {
            Requires.NotNullOrWhiteSpace(clientId, nameof(clientId));
            Requires.NotNullOrWhiteSpace(clientSecret, nameof(clientSecret));

            this._logger = logger ?? NullLoggerFactory.Instance.CreateLogger(nameof(ERNIEBotClient));

            this._clientId = clientId;
            this._clientSecret = clientSecret;
            this._client = client ?? HttpClientProvider.CreateClient();
            this._tokenStore = tokenStore ?? new DefaultTokenStore();
        }

        /// <summary>
        /// API for Chat Completion
        /// </summary>
        /// <param name="request"></param>
        /// <param name="endpoint"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<ChatResponse> ChatAsync(ChatRequest request, string endpoint, CancellationToken cancellationToken = default)
        => this.ChatAsync(request, new ModelEndpoint(endpoint), cancellationToken);

        /// <summary>
        ///  API for Chat Completion
        /// </summary>
        /// <param name="request"></param>
        /// <param name="modelEndpoint"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ChatResponse> ChatAsync(ChatRequest request, ModelEndpoint modelEndpoint, CancellationToken cancellationToken = default)
        {
            if (request.Stream.HasValue && request.Stream.Value)
            {
                request.Stream = false;
            }

            OrganizeChatMessages(request.Messages);

            var webRequest = await CreateRequestAsync(HttpMethod.Post, Defaults.Endpoint(modelEndpoint), request);

            var response = await _client.SendAsync(webRequest, cancellationToken);

            return await ParseResponseAsync<ChatResponse>(response);
        }

        /// <summary>
        /// Stream API for Chat Completion
        /// </summary>
        /// <param name="request"></param>
        /// <param name="modelEndpoint"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public IAsyncEnumerable<ChatResponse> ChatStreamAsync(ChatRequest request, string modelEndpoint, CancellationToken cancellationToken = default)
        => this.ChatStreamAsync(request, new ModelEndpoint(modelEndpoint), cancellationToken);

        /// <summary>
        /// Stream API for Chat Completion
        /// </summary>
        /// <param name="request"></param>
        /// <param name="modelEndpoint"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async IAsyncEnumerable<ChatResponse> ChatStreamAsync(ChatRequest request, ModelEndpoint modelEndpoint, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            request.Stream = true;

            OrganizeChatMessages(request.Messages);

            var webRequest = await CreateRequestAsync(HttpMethod.Post, Defaults.Endpoint(modelEndpoint), request, cancellationToken);

            var response = await _client.SendAsync(webRequest, cancellationToken);

            await foreach (var item in ParseResponseStreamAsync(response, cancellationToken))
            {
                yield return item;
            }
        }

        /// <summary>
        /// Embedding Api For ERNIE-Bot
        /// </summary>
        /// <returns></returns>
        public async Task<EmbeddingsResponse> EmbeddingsAsync(EmbeddingsRequest request, CancellationToken cancellationToken = default)
        {
            var webRequest = await CreateRequestAsync(HttpMethod.Post, Defaults.Endpoint(ModelEndpoints.Embedding_v1), request);

            var response = await _client.SendAsync(webRequest, cancellationToken);

            return await ParseResponseAsync<EmbeddingsResponse>(response);
        }

        /// <summary>
        /// Embedding Api for custom Embedding Model
        /// </summary>
        /// <param name="request"></param>
        /// <param name="modelEndpoint"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<EmbeddingsResponse> EmbeddingsAsync(EmbeddingsRequest request, EmbeddingModelEndpoint modelEndpoint, CancellationToken cancellationToken = default)
        {
            var webRequest = await CreateRequestAsync(HttpMethod.Post, Defaults.Endpoint(modelEndpoint), request);

            var response = await _client.SendAsync(webRequest, cancellationToken);

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

            var url = $"{Defaults.AccessTokenEndpoint}?client_id={_clientId}&client_secret={_clientSecret}&grant_type=client_credentials";

            var webRequest = new HttpRequestMessage(HttpMethod.Post, url);
            var response = await _client.SendAsync(webRequest, cancellationToken);
            var accessToken = await ParseResponseAsync<TokenResponse>(response);

            if (string.IsNullOrWhiteSpace(accessToken.AccessToken))
            {
                throw new HttpRequestException($"Failed to get access token");
            }

            await _tokenStore.SaveTokenAsync(accessToken.AccessToken, TimeSpan.FromSeconds(accessToken.Expiration), cancellationToken);

            return accessToken.AccessToken;
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
            var error = JsonSerializer.Deserialize<ERNIEBotError>(responseJson);

            if (error?.Code != -1)
            {
                throw new ERNIEBotException(error);
            }

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

        private void OrganizeChatMessages(List<Message> messages)
        {
            if (!messages.Any())
            {
                throw new ERNIEBotException(-1, "no messages");
            }

            if (messages.Count % 2 == 0)
            {
                if (messages.First().Role != MessageRole.User)
                {
                    messages.RemoveAt(0);
                    _logger.LogWarning("Messages count must be odd, Remove the first message to ensure the API call is working properly.");
                }
            }
        }
        #endregion

    }
}
