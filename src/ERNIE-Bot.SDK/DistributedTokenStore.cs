using Microsoft.Extensions.Caching.Distributed;

namespace ERNIE_Bot.SDK
{
    public class DistributedTokenStore : ITokenStore
    {
        private readonly IDistributedCache _cache;

        public DistributedTokenStore(IDistributedCache cache)
        {
            this._cache = cache;
        }
        public async Task<string?> GetTokenAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await _cache.GetStringAsync(Defaults.TokenCacheName);
        }

        public async Task SaveTokenAsync(string accessToken, TimeSpan expiration, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await _cache.SetStringAsync(Defaults.TokenCacheName, accessToken, new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = expiration
            }, cancellationToken);
        }
    }
}
