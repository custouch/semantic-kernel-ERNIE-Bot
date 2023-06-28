# ERNIE-Bot SDK

[![NuGet](https://img.shields.io/nuget/v/ERNIE-Bot.SDK?label=sdk)](https://www.nuget.org/packages/ERNIE-Bot.SDK/)

ERNIE-Bot(文心千帆) .NET SDK

## 安装

```
dotnet add package ERNIE-Bot.SDK --version 0.2.0-preview
```

## 使用 

引入该 Package 之后，实例化`ERNIEBotClient` 即可使用。

```csharp

var clientId = builder.Configuration["ClientId"];
var secret = builder.Configuration["ClientSecret"];
var httpClient = new HttpClient();
var tokenStore = new MemoryTokenStore();

var client = new ERNIEBotClient(clientId, secret, client, tokenStore);
```

#### API 鉴权令牌的存储和管理

由于百度的接口采用了AccessToken的鉴权方式，所以需要在使用之前先获取AccessToken，然后在请求的时候带上AccessToken。为了方便管理，SDK提供了`ITokenStore`接口，用于存储和管理AccessToken。

用户可自行实现该接口，自行管理AccessToken的存储和更新。SDK提供了`MemoryTokenStore`，用于在内存中存储AccessToken，该实现依赖于`MemoryCache`。


## 接口功能

- [x] API 鉴权
- [x] ERNIE-Bot
- [x] ERNIE-Bot (Stream)
- [x] ERNIE-Bot-turbo
- [x] ERNIE-Bot-turbo (Stream)
- [x] Embedding-V1
- [ ] BLOOMZ-7B
- [ ] Prompt模版


