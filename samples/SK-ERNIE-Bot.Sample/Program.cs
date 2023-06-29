using ERNIE_Bot.SDK;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Memory;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped(svc =>
{
    var kernel = Kernel.Builder
        .WithERNIEBotTurboChatCompletionService(svc, builder.Configuration, "ernie_bot_turbo", true)
        .WithERNIEBotChatCompletionService(svc, builder.Configuration, "ernie_bot")
        .WithERNIEBotEmbeddingGenerationService(svc, builder.Configuration)
        .WithMemoryStorage(new VolatileMemoryStore())
        .Build();
    return kernel;
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
