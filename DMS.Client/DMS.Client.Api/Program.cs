using DMS.Client.Api.Caching;
using DMS.Client.Api.Configuration;
using DMS.Client.Api.Services;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions<BoilerConfiguration>()
    .BindConfiguration("BoilerConfiguration")
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddMassTransit(config =>
{
    config.SetKebabCaseEndpointNameFormatter();

    config.AddConsumers(typeof(Program).Assembly);

    config.UsingRabbitMq((context, config) =>
    {
        var rabbitMqConnectionString = builder.Configuration.GetConnectionString("RabbitMq")!;

        config.Host(new Uri(rabbitMqConnectionString), "/");
        config.ConfigureEndpoints(context);
    });
});

builder.Services.AddMemoryCache();
builder.Services.AddTransient<BoilerTemperatureCache>();
builder.Services.AddHostedService<BoilerTemperatureRequestService>();

builder.Services.AddControllers();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
