using ERNIE_Bot.SDK;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Plugins.Memory;
using Microsoft.SemanticKernel.TemplateEngine;
using Microsoft.SemanticKernel.TemplateEngine.Basic;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped(svc =>
{
    var kernel = Kernel.Builder
        .WithERNIEBotChatCompletionService(svc, builder.Configuration, "ernie_bot", ModelEndpoints.ERNIE_Bot)
        .WithPromptTemplateEngine(new BasicPromptTemplateEngine())
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


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
