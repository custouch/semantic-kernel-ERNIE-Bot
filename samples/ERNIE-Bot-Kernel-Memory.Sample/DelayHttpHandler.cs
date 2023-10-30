using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ERNIE_Bot_Kernel_Memory.Sample
{
    public class DelayHttpHandler : DelegatingHandler
    {
        private readonly TimeSpan _delay;

        public DelayHttpHandler(TimeSpan delay)
        {
            _delay = delay;
            InnerHandler = new HttpClientHandler();
        }

        public DelayHttpHandler(int delayMilliseconds) : this(TimeSpan.FromMilliseconds(delayMilliseconds))
        {

        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            await Task.Delay(_delay, cancellationToken);
            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
    }
}
