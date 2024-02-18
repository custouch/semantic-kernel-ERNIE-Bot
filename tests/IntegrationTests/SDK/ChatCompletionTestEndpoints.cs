using ERNIE_Bot.SDK;
using System.Collections;

namespace IntegrationTests.SDK
{
    public class ChatCompletionTestEndpoints : IEnumerable<object[]>
    {
        public ChatCompletionTestEndpoints()
        {
            _endpoints = typeof(ModelEndpoints).GetFields(System.Reflection.BindingFlags.Public |
                                               System.Reflection.BindingFlags.Static)
                   .Where(f => f.FieldType == typeof(ModelEndpoint))
                   .Select(f => (ModelEndpoint)f.GetValue(null)!)
                   .ToList();
        }
        public readonly List<ModelEndpoint> _endpoints;

        public IEnumerator<object[]> GetEnumerator()
            => this._endpoints.Select(e => new[] { e }).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => this._endpoints.GetEnumerator();
    }
}
