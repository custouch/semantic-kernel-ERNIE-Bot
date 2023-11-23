using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text;
using System.Threading.RateLimiting;

namespace ERNIE_Bot.SDK
{
    public class HttpClientProvider
    {
        public static HttpClient CreateClient()
        {
            return new HttpClient();
        }

        public static HttpClient CreateRateLimitedClient(RateLimiter limiter)
        {
            return new HttpClient(new ClientSideRateLimitedHandler(limiter));
        }

        public static HttpClient CreateFixedWindowRateLimitedClient(FixedWindowRateLimiterOptions fixedWindowRateLimiterOptions)
        {
            return CreateRateLimitedClient(new FixedWindowRateLimiter(fixedWindowRateLimiterOptions));
        }
    }
    /// <summary>
    /// ref: https://learn.microsoft.com/dotnet/core/extensions/http-ratelimiter
    /// </summary>
    internal sealed class ClientSideRateLimitedHandler
    : DelegatingHandler, IAsyncDisposable
    {
        private readonly RateLimiter _rateLimiter;

        public ClientSideRateLimitedHandler(RateLimiter limiter)
            : base(new HttpClientHandler()) => _rateLimiter = limiter;

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            using RateLimitLease lease = await _rateLimiter.AcquireAsync(
                permitCount: 1, cancellationToken);

            if (lease.IsAcquired)
            {
                return await base.SendAsync(request, cancellationToken);
            }

            var response = new HttpResponseMessage(HttpStatusCode.TooManyRequests);
            if (lease.TryGetMetadata(
                    MetadataName.RetryAfter, out TimeSpan retryAfter))
            {
                response.Headers.Add(
                    "Retry-After",
                    ((int)retryAfter.TotalSeconds).ToString(
                        NumberFormatInfo.InvariantInfo));
            }

            return response;
        }

        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            await _rateLimiter.DisposeAsync().ConfigureAwait(false);

            Dispose(disposing: false);
            GC.SuppressFinalize(this);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                _rateLimiter.Dispose();
            }
        }
    }
}
