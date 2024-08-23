using Microsoft.AspNetCore.Mvc;
using OpenApi.Shared;
using OpenApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.Services.Configure<OpenAiOptions>(
    builder.Configuration.GetSection(OpenAiOptions.Name));
builder.Services.Configure<BingSearchOptions>(
    builder.Configuration.GetSection(BingSearchOptions.Name));

builder.Services.AddServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors();

app.UseHttpsRedirection();

var messages = new List<Message>();

app.MapPost("/chat/optionA", async  (IChatService service, [FromBody] Message message) =>
{
    message.Id = messages.Count + 1;

    var response = await service.SendAndReceive(message.Content);

    message.Content = $"[ChatGPT] {response.ToList().Last().ToString()}";
    messages.Add(message);
    return Results.Ok(message);
});

app.MapPost("/chat/optionB", async (IChatService service, Message message) =>
{
    message.Id = messages.Count + 1;
    var response = await service.SendAndReceiveWithPluginSupport(message.Content);
    message.Content = $"[ChatGPT:OptionB] {response.ToList().Last().ToString()}";
    messages.Add(message);
    return Results.Ok(message);
});

app.MapPost("/chat/optionC", async (IChatService service, Message message) =>
{
    message.Id = messages.Count + 1;
    var response = await service.SendAndReceiveWithSearchSupport(message.Content);
    message.Content = $"[ChatGPT:OptionC] {response.ToList().Last().ToString()}";
    messages.Add(message);
    return Results.Ok(message);
});

app.MapGet("/chat", () =>
{
    return Results.Ok(messages.OrderByDescending(m => m.Timestamp));
});

app.Run();
