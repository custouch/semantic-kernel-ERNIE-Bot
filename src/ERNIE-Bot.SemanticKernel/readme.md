# ERNIE-Bot Semantic Kernel

[![NuGet](https://img.shields.io/nuget/v/ERNIE-Bot.SemanticKernel?label=sk)](https://www.nuget.org/packages/ERNIE-Bot.SemanticKernel/)


ERNIE-Bot(文心千帆) Semantic Kernel 集成

## 安装

```
dotnet add package ERNIE-Bot.SemanticKernel --version 0.2.0-preview
```

## 使用

```
// 注册 HttpClient 
builder.Services.AddHttpClient();

// 注册 MemoryCache 并注册 ITokenStore, 用户亦可可自行实现 ITokenStore
builder.Services.AddMemoryCache();
builder.Services.AddScoped<ITokenStore, MemoryTokenStore>();

builder.Services.AddScoped(svc =>
{
    var kernel = Kernel.Builder
        // 使用 ERNIE Bot Turbo
        .WithERNIEBotTurboChatCompletionService(svc, builder.Configuration, "ernie_bot_turbo", true)
        // 使用 ERNIE Bot
        .WithERNIEBotChatCompletionService(svc, builder.Configuration, "ernie_bot")
        // 使用 Embedding
        .WithERNIEBotEmbeddingGenerationService(svc, builder.Configuration)
        .WithMemoryStorage(new VolatileMemoryStore())
        .Build();
    return kernel;
});
```

## 功能

- [x] IChatCompletion
- [x] ITextCompletion
- [x] ITextEmbeddingGeneration
