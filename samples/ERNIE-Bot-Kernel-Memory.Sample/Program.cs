using ERNIE_Bot.KernelMemory;
using ERNIE_Bot.SDK;
using ERNIE_Bot_Kernel_Memory.Sample;
using Microsoft.Extensions.Configuration;
using Microsoft.KernelMemory;
using Microsoft.KernelMemory.Handlers;
using System;

var config = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();


var client = new ERNIEBotClient(config["ClientId"]!, config["ClientSecret"]!,
    new HttpClient(new DelayHttpHandler(200)));

async Task HugePdfSample()
{
    var memory = new KernelMemoryBuilder()
        .WithERNIEBotDefaults(client)
        .With(new TextPartitioningOptions
        {
            MaxTokensPerParagraph = 200,
            MaxTokensPerLine = 60,
            OverlappingTokens = 30
        })
        .BuildServerlessClient();

    await memory.ImportDocumentAsync("muduo.pdf",
        steps: new[] { "extract", "partition", "gen_embeddings", "save_embeddings" });

    var question = "What's muduo?";

    Console.WriteLine($"\n\nQuestion: {question}");

    var answer = await memory.AskAsync(question);

    Console.WriteLine($"\nAnswer: {answer.Result}");

    Console.WriteLine("\n\n  Sources:\n");

    foreach (var x in answer.RelevantSources)
    {
        Console.WriteLine($"  - {x.SourceName}  - {x.Link} [{x.Partitions.First().LastUpdate:D}]");
    }
}

async Task NormalPdfSample()
{
    var memory = new KernelMemoryBuilder()
        .WithERNIEBotDefaults(client)
        .With(new TextPartitioningOptions
        {
            MaxTokensPerParagraph = 200,
            MaxTokensPerLine = 60,
            OverlappingTokens = 30
        })
        .BuildServerlessClient();

    //默认的step是
    //[0] "extract"
    //[1] "partition"
    //[2] "gen_embeddings"
    //[3] "save_embeddings"
    //[4] "summarize"
    //[5] "gen_embeddings"
    //[6] "save_embeddings"
    await memory.ImportDocumentAsync("sample-SK-Readme.pdf");

    var question = "What's Semantic Kernel?";

    Console.WriteLine($"\n\nQuestion: {question}");

    var answer = await memory.AskAsync(question);

    Console.WriteLine($"\nAnswer: {answer.Result}");

    Console.WriteLine("\n\n  Sources:\n");

    foreach (var x in answer.RelevantSources)
    {
        Console.WriteLine($"  - {x.SourceName}  - {x.Link} [{x.Partitions.First().LastUpdate:D}]");
    }
}

await NormalPdfSample();
await HugePdfSample();

