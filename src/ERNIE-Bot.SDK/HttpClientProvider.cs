using System;
using System.Collections.Generic;
using System.Text;

namespace ERNIE_Bot.SDK
{
    public class HttpClientProvider
    {
        public static HttpClient CreateClient()
        {
            return new HttpClient();
        }
    }
}
