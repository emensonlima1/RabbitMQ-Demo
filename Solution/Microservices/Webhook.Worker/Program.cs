using Common.Configurations;
using Common.Interfaces;
using Domain.Events;
using Webhook.Worker;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddTransient<IEventBus, EventBus>();
builder.Services.AddTransient<IEventHandler, Handler>();

var host = builder.Build();
host.Run();