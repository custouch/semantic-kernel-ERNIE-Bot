
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

var memory = new KernelMemoryBuilder()
        .WithERNIEBotDefaults(client)
        .With(new TextPartitioningOptions
        {
            MaxTokensPerParagraph = 300,
            MaxTokensPerLine = 100,
            OverlappingTokens = 50
        })
        .BuildServerlessClient();

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