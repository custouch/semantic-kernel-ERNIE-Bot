# ERNIE-Bot Semantic Kernel

[![NuGet](https://img.shields.io/nuget/v/ERNIE-Bot.SemanticKernel?label=sk)](https://www.nuget.org/packages/ERNIE-Bot.SemanticKernel/)


ERNIE-Bot(文心千帆) Semantic Kernel 集成

## 安装

```
dotnet add package ERNIE-Bot.SemanticKernel --prerelease
```

## 使用

```

builder.Services.AddScoped(svc =>
{
    var kernel = Kernel.Builder
        // 使用 ERNIE Bot
        .WithERNIEBotChatCompletionService(svc, builder.Configuration, "ernie_bot", ModelEndpoints.ERNIE_Bot)
        .Build();
    return kernel;
});

builder.Services.AddScoped(svc =>
{
    var memory = new MemoryBuilder()
    .WithERNIEBotEmbeddingGenerationService(svc, builder.Configuration)
    .WithMemoryStore(new VolatileMemoryStore())
    .Build();
    return memory;
});
```


## 功能

- [x] IChatCompletion
- [x] ITextCompletion
- [x] ITextEmbeddingGeneration
