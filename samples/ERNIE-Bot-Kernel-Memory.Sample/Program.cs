
using ERNIE_Bot.KernelMemory;
using Microsoft.Extensions.Configuration;
using Microsoft.SemanticMemory;
using Microsoft.SemanticMemory.Handlers;
using System;

var config = new ConfigurationBuilder()
            .AddUserSecrets<Program>()
            .Build();

var httpClient = new HttpClient();

var memory = new MemoryClientBuilder()
        .WithERNIEBotDefaults(config["ClientId"]!, config["ClientSecret"]!)
        .With(new TextPartitioningOptions
        {
            MaxTokensPerParagraph = 300,
            MaxTokensPerLine = 100,
            OverlappingTokens = 50
        })
        .BuildServerlessClient();

//TODO: "Open api qps request limit reached" issue
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