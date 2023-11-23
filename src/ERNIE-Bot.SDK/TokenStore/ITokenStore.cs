namespace ERNIE_Bot.SDK
{
    public interface ITokenStore
    {
        Task SaveTokenAsync(string accessToken, TimeSpan expiration, CancellationToken cancellationToken);

        Task<string?> GetTokenAsync(CancellationToken cancellationToken);
    }
}
