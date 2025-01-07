using DMS.Monitor.Api.Extensions;
using DMS.Monitor.Application.Shared.Extensions;
using DMS.Monitor.Application.Write.Configuration;
using DMS.Monitor.Domain.Extensions;
using DMS.Monitor.Infrastructure.BoilerApi.Configuration;
using DMS.Monitor.Infrastructure.BoilerApi.Extensions;
using DMS.Monitor.Infrastructure.Persistence.Extensions;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions<BoilerTemperatureThresholds>()
    .BindConfiguration("BoilerTemperatureThresholds")
    .ValidateDataAnnotations()
    .ValidateOnStart();

var sqlDbConnectionString = builder.Configuration.GetConnectionString("dms-sql")!;
builder.Services.AddPersistence(sqlDbConnectionString);

var rabbitMqConnectionString = builder.Configuration.GetConnectionString("dms-mq")!;
builder.Services
    .AddMassTransit(rabbitMqConnectionString)
    .AddMediatR();

var devicesSettings = builder.Configuration.GetSection("Devices").Get<DevicesSettings>()!;
builder.Services.AddBoilerApiClient(devicesSettings.BoilerApi);

builder.Services.AddDomainConverters();

builder.Services.AddApi();

builder.Services.AddControllers();

var app = builder.Build();
InitializeApplication(app);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();

static void InitializeApplication(WebApplication app)
{
    using IServiceScope scope = app.Services.CreateScope();
    scope.MigrateDatabase();
    scope.SeedDatabase();
}