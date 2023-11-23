using ERNIE_Bot.SDK;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Plugins.Memory;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped(svc =>
{
    var kernel = new KernelBuilder()
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
