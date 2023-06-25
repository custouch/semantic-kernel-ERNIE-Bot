using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ERNIE_Bot.SDK
{
    internal class MemoryTokenStore : ITokenStore
    {
        private readonly IMemoryCache _cache;

        public MemoryTokenStore(IMemoryCache cache)
        {
            this._cache = cache;
        }
        public Task<string?> GetTokenAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            _cache.TryGetValue(Defaults.TokenCacheName, out string? accessToken);
            return Task.FromResult(accessToken);

        }

        public Task SaveTokenAsync(string accessToken, TimeSpan expiration, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            _cache.Set(Defaults.TokenCacheName, accessToken, expiration);
            return Task.CompletedTask;
        }
    }
}
