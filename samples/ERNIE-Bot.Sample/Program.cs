using ERNIE_Bot.SDK;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

builder.Services.AddMemoryCache();
builder.Services.AddScoped<ITokenStore, MemoryTokenStore>();

builder.Services.AddScoped(svc =>
{
    var factory = svc.GetRequiredService<IHttpClientFactory>();
    var client = factory.CreateClient();
    var tokenStore = svc.GetRequiredService<ITokenStore>();
    var logger = svc.GetRequiredService<ILogger<ERNIEBotClient>>();

    var clientId = builder.Configuration["ClientId"];
    var secret = builder.Configuration["ClientSecret"];

    return new ERNIEBotClient(clientId, secret, client, tokenStore, logger);
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
