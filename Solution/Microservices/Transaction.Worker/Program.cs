using Domain.BusinessRules;
using Domain.BusinessRules.Interfaces;
using Domain.Events;
using Transaction.Worker;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddTransient<IEventBus, EventBus>();
builder.Services.AddTransient<ITransactionRule, TransactionRule>();
var host = builder.Build();
host.Run();