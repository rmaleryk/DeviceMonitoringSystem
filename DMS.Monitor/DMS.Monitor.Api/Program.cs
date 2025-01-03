using DMS.Monitor.Application.Configuration;
using DMS.Monitor.Application.Extensions;
using DMS.Monitor.Domain.Extensions;
using DMS.Monitor.Infrastructure.BoilerApi.Configuration;
using DMS.Monitor.Infrastructure.BoilerApi.Extensions;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddOptions<TemperatureThresholds>()
    .BindConfiguration("BoilerTemperatureThresholds")
    .ValidateDataAnnotations()
    .ValidateOnStart();

var rabbitMqConnectionString = builder.Configuration.GetConnectionString("dms-mq")!;
builder.Services.AddMassTransit(rabbitMqConnectionString);

var devicesSettings = builder.Configuration.GetSection("Devices").Get<DevicesSettings>()!;
builder.Services.AddBoilerApiClient(devicesSettings.BoilerDeviceApi);

builder.Services.AddDomainConverters();

builder.Services.AddControllers();

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
