using ERNIE_Bot.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERNIE_BOT.SDK.Tests
{
    internal class TokenStoreHelper : ITokenStore
    {
        private string? _token;

        public TokenStoreHelper(string? token)
        {
            _token = token;
        }

        public Task<string?> GetTokenAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(_token);
        }

        public Task SaveTokenAsync(string accessToken, TimeSpan expiration, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
