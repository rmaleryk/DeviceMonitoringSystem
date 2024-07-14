using BoilerMonitor.Api.Configuration;
using BoilerMonitor.Api.Consumers;
using BoilerMonitor.Domain.Extensions;
using BoilerMonitor.Infrastructure.BoilerApi.Configuration;
using BoilerMonitor.Infrastructure.BoilerApi.Extensions;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<TemperatureThresholds>(builder.Configuration.GetSection("TemperatureThresholds"));

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<TemperatureRequestConsumer>();

    x.UsingRabbitMq((context, config) =>
    {
        var rabbitMqSettings = builder.Configuration.GetSection("RabbitMq").Get<RabbitMqSettings>();

        config.Host(rabbitMqSettings.Host, "/", h =>
        {
            h.Username(rabbitMqSettings.Username);
            h.Password(rabbitMqSettings.Password);
        });

        config.ConfigureEndpoints(context);
    });
});

var boilerApiSettings = builder.Configuration.GetSection("BoilerApi").Get<BoilerApiSettings>();
builder.Services.AddBoilerApiClient(boilerApiSettings);

builder.Services.AddDomainConverters();

builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
