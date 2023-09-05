# ERNIE-Bot SDK

[![NuGet](https://img.shields.io/nuget/v/ERNIE-Bot.SDK?label=sdk)](https://www.nuget.org/packages/ERNIE-Bot.SDK/)

ERNIE-Bot(文心千帆) .NET SDK

## 安装

```
dotnet add package ERNIE-Bot.SDK --prerelease
```

## 使用 

引入该 Package 之后，实例化`ERNIEBotClient` 即可使用。

```csharp
var clientId = builder.Configuration["ClientId"];
var secret = builder.Configuration["ClientSecret"];

var client = new ERNIEBotClient(clientId, secret);
```

### ChatCompletion 

直接使用ChatAsync方法可以完成对话。

其中 ModelEndpoint 是预置的模型地址，可以使用 ModelEndpoints 类来获取所有支持的模型地址。

```csharp
await client.ChatAsync(new ChatCompletionsRequest()
{
    Messages = new List<Message>
        {
            new Message()
            {
                Content = input.Text,
                Role = MessageRole.User
            }
        }
}, ModelEndpoints.ERNIE_Bot);
```

### ChatCompletionStream

使用 ChatCompletionStreamAsync 方法可以获取一个流，该流会不断的返回对话结果。

```csharp
var results = client.ChatStreamAsync(new ChatCompletionsRequest()
{
    Messages = new List<Message>
        {
            new Message()
            {
                Content = input.Text,
                Role = MessageRole.User
            }
        }
}, ModelEndpoints.ERNIE_Bot);

```

可以使用一下方法获取流中的数据。

```csharp
await foreach (var result in results)
{
    // do something with result
}
```

### Embedding

使用 EmbeddingAsync 方法可以获取文本的 Embedding。
同样可以使用 ModelEndpoints 类来获取所支持的Embedding模型的地址。

```csharp
var result = await _client.EmbeddingsAsync(new EmbeddingsRequest()
{
    Input = new List<string>()
        {
            input.Text
        }
},ModelEndpoints.Embedding_v1);
```

### 自定义模型

如果您使用了自定义的模型服务，可以通过以下方法声明自定义模型的地址。

```csharp
var endpoint = new ModelEndpoint("{申请发布时填写的API地址}");
```


#### API 鉴权令牌的存储和管理

由于百度的接口采用了AccessToken的鉴权方式，所以需要在使用之前先获取AccessToken，然后在请求的时候带上AccessToken。为了方便管理，SDK提供了`ITokenStore`接口，用于存储和管理AccessToken。

用户可自行实现该接口，自行管理AccessToken的存储和更新。SDK提供了`MemoryTokenStore`，在内存中存储AccessToken，该实现依赖于`MemoryCache`。


## 接口支持

- [x] API 鉴权
- [x] ERNIE-Bot
- [x] ERNIE-Bot-turbo
- [x] Embedding-V1
- [x] BLOOMZ-7B
- [x] Llama_2_7b_chat
- [x] Llama_2_13b_chat
- [x] Llama_2_70b_chat
- [x] Qianfan_BLOOMZ_7B_compressed
- [x] Qianfan_Chinese_Llama_2_7b
- [x] ChatGLM2_6b_32k
- [x] AquilaChat_7b
- [x] bge_large_zh
- [x] bge_large_en
- [x] 自定义chat模型
- [ ] Prompt模版


