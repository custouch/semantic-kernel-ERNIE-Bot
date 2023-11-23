using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;

namespace ERNIE_Bot.SDK
{
    public class MemoryTokenStore : ITokenStore
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
