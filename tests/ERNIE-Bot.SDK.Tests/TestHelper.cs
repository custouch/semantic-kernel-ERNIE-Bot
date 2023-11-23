using System.Net;

namespace ERNIE_BOT.SDK.Tests
{
    internal static class TestHelper
    {
        internal static async Task<HttpResponseMessage> GetHttpResponseFormFileAsync(string fileName, bool isStream = false)
        {
            var filePath = Path.Combine("./TestDatas", fileName);

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(fileName);
            }

            if (!isStream)
            {
                var content = File.ReadAllText(filePath);
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
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
                    StatusCode = HttpStatusCode.OK,
                    Content = streamContent
                };
            }
        }

        internal static async Task<HttpClient> FakeHttpClient(string fileName, bool isStream = false)
        {
            var response = await GetHttpResponseFormFileAsync(fileName, isStream);
            var client = new HttpClient(new MockHttpMessageHandler(response));

            return client;
        }
    }

    public class MockHttpMessageHandler : HttpMessageHandler
    {
        private readonly HttpResponseMessage _response;

        public MockHttpMessageHandler(HttpResponseMessage response)
        {
            this._response = response;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_response);
        }
    }
}
