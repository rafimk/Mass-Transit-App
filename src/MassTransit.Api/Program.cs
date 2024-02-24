using MassTransit;
using MassTransit.Api.Consumers;
using MassTransit.Api.Publishers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(busConfiguration =>
{
    busConfiguration.SetKebabCaseEndpointNameFormatter();

    busConfiguration.AddConsumers(typeof(Program).Assembly); 
    
    busConfiguration.UsingInMemory((context, config) => 
        config.ConfigureEndpoints(context));
    
    // busConfiguration.UsingRabbitMq((context,cfg) =>
    // {
    //     cfg.Host("localhost", "/", h => {
    //         h.Username("guest");
    //         h.Password("guest");
    //     });
    //
    //     cfg.ConfigureEndpoints(context);
    // });
    
    // busConfiguration.UsingAzureServiceBus((context,cfg) =>
    // {
    //     cfg.Host("");
    //
    //     cfg.ConfigureEndpoints(context);
    // });
});

builder.Services.AddHostedService<MessagePublisher>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}