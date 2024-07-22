using Microsoft.Extensions.Diagnostics.HealthChecks;
using Radar.Placas.Listener.App.Services;
using Radar.Placas.Listener.App.Workers;
using Radar.Placas.Listener.Domain.Interfaces.MessageConsumers;
using Radar.Placas.Listener.Domain.Interfaces.Requests;
using Radar.Placas.Listener.Domain.Interfaces.Services;
using Radar.Placas.Listener.Infra.MessageConsumers;
using Radar.Placas.Listener.Infra.Requests;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<IDadosDeteccaoRadarService, DadosDeteccaoRadarService>();
builder.Services.AddTransient<ISendDadosDeteccaoRadarRequest, SendDadosDeteccaoRadarRequest>();
builder.Services.AddTransient<IDadosDeteccaoRadarMessageConsumer, DadosDeteccaoRadarMessageConsumer>();

builder.Services.AddHttpClient();
builder.Services.AddWindowsService();
builder.Services.AddHostedService<DadosDeteccaoRadarListener>();

IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

builder.Services.AddHealthChecks()
    .AddRabbitMQ(new Uri(config.GetConnectionString("RabbitMQConnection")!), failureStatus: HealthStatus.Unhealthy);

var app = builder.Build();

app.Run();