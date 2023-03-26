using EventBus.Messages.Common;
using MassTransit;
using Ordering.API.EventBusConsumer;
using Ordering.API.Extensions;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ------------ dependency injection --------------------

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// MassTransit-RabbitMQ Configuration
builder.Services.AddMassTransit(config =>
{
    config.AddConsumer<BasketCheckoutConsumer>();
    config.UsingRabbitMq((_, cfg) =>
    {
        cfg.Host(configuration.GetValue<string>("EventBusSettings:HostAddress"));
        cfg.ReceiveEndpoint(EventBusConstants.BasketCheckoutQueue, c =>
            c.ConfigureConsumer<BasketCheckoutConsumer>(_)
        );
    });
});

// General configuration
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<BasketCheckoutConsumer>();

// ------------------------------------------------------

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ------------ Migrations & seeding --------------------

app.MigrateDatabase<OrderContext>((context, services) =>
{
    var logger = services.GetService<ILogger<OrderContextSeed>>();
    OrderContextSeed.SeedAsync(context, logger).Wait();
});

// ------------------------------------------------------

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
