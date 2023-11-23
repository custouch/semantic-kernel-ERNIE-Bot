namespace ERNIE_Bot.SDK
{
    public class DefaultTokenStore : ITokenStore
    {
        private static string? _access_token = null;
        private static DateTime? _expires_at = null;

        public Task<string?> GetTokenAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (_expires_at.HasValue && _expires_at.Value > DateTime.UtcNow)
            {
                return Task.FromResult(_access_token);
            }
            return Task.FromResult((string?)null);
        }

        public Task SaveTokenAsync(string accessToken, TimeSpan expiration, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            _access_token = accessToken;
            _expires_at = DateTime.UtcNow.Add(expiration);
            return Task.CompletedTask;
        }
    }
}
