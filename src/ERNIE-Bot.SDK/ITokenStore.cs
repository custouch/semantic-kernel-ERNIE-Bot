using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ERNIE_Bot.SDK
{
    public interface ITokenStore
    {
        Task SaveTokenAsync(string accessToken, TimeSpan expiration,CancellationToken cancellationToken);

        Task<string?> GetTokenAsync(CancellationToken cancellationToken);
    }
}
