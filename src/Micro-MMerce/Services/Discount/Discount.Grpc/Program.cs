using Discount.Grpc.Interfaces;
using Discount.Grpc.Repositories;
using Discount.Grpc.Services;

var builder = WebApplication.CreateBuilder(args);
// TODO: Add support for data seeding usign extensions

builder.Services.AddGrpc();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// ------------ dependency injection --------------------

builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();

// ------------------------------------------------------

var app = builder.Build();

app.MapGrpcService<DiscountService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
