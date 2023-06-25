using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ERNIE_BOT.SDK.Tests
{
    internal static class TestHelper
    {
        internal static async Task<HttpResponseMessage> GetHttpResponseFormFileAsync(string fileName, HttpStatusCode code = HttpStatusCode.OK, bool isStream = false)
        {
            var filePath = Path.Combine("./TestData", fileName);

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(fileName);
            }

            if (!isStream)
            {
                var content = File.ReadAllText(filePath);
                return new HttpResponseMessage
                {
                    StatusCode = code,
                    Content = new StringContent(content)
                };
            }
            else
            {
                var content = File.ReadAllLines(filePath);
                var stream = new MemoryStream();
                var writer = new StreamWriter(stream);
                var streamContent = new StreamContent(stream);
                foreach (var line in content)
                {
                    await writer.WriteAsync(line);
                    await writer.FlushAsync();
                }
                return new HttpResponseMessage
                {
                    StatusCode = code,
                    Content = streamContent
                };
            }



        }

        internal static async Task<HttpClient> FakeHttpClient(string fileName, HttpStatusCode code = HttpStatusCode.OK, bool isStream = false)
        {
            var response = await GetHttpResponseFormFileAsync(fileName, code, isStream);
            var client = Substitute.For<HttpClient>();

            client.SendAsync(default)
                .ReturnsForAnyArgs(response);

            return client;
        }
    }
}
