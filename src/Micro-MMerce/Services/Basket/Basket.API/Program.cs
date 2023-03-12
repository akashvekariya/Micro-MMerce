using Basket.API.Interfaces;
using Basket.API.Repositories;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddStackExchangeRedisCache(
   options => options.Configuration = configuration.GetValue<string>("CacheSettings:ConnectionString")
);

// ------------ dependency injection --------------------

builder.Services.AddScoped<IBasketRepository, BasketRepository>();

// ------------------------------------------------------

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
   app.UseSwagger();
   app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
