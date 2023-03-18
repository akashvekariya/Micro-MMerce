using Basket.API.GrpcServices;
using Basket.API.Interfaces;
using Basket.API.Repositories;
using Discount.Grpc.Protos;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddStackExchangeRedisCache(options => options.Configuration = configuration.GetValue<string>("CacheSettings:ConnectionString"));

builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(
   options => options.Address = new Uri(configuration.GetValue<string>("GrpcSettings:DiscountUrl"))
);

// ------------ dependency injection --------------------

builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.AddScoped<DiscountGrpcService>();

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
