using Basket.API.GrpcServices;
using Basket.API.Interfaces;
using Basket.API.Repositories;
using Discount.Grpc.Protos;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ------------ dependency injection --------------------

// General Configuration
builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.AddAutoMapper(typeof(Program));

// Redis Configuration
builder.Services.AddStackExchangeRedisCache(
    options => options.Configuration = configuration.GetValue<string>("CacheSettings:ConnectionString")
);

// Grpc Configuration
builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(
   options => options.Address = new Uri(configuration.GetValue<string>("GrpcSettings:DiscountUrl"))
);
builder.Services.AddScoped<DiscountGrpcService>();

// MassTransit-RabbitMQ Configuration
builder.Services.AddMassTransit(
    config => config.UsingRabbitMq(
        (_, cfg) => cfg.Host(configuration.GetValue<string>("EventBusSettings:HostAddress"))
    )
);

// ------------------------------------------------------

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
