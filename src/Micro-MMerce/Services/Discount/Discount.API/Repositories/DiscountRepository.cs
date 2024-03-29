using Dapper;
using Npgsql;
using Discount.API.Interfaces;
using Discount.API.Models;

namespace Discount.API.Repositories;

public class DiscountRepository : IDiscountRepository
{
    private readonly IConfiguration _config;

    public DiscountRepository(IConfiguration config)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
    }

    public async Task<bool> CreateDiscount(Coupon coupon)
    {
        using var connection = new NpgsqlConnection(_config.GetValue<string>("DatabaseSettings:ConnectionString"));
        var affected = await connection.ExecuteAsync(
           "INSERT INTO Coupon (ProductName, Description, Amount) VALUES (@ProductName, @Description, @Amount)",
           new
           {
               coupon.ProductName,
               coupon.Description,
               coupon.Amount
           }
        );
        return affected != 0;
    }

    public async Task<bool> DeleteDiscount(string productName)
    {
        using var connection = new NpgsqlConnection(_config.GetValue<string>("DatabaseSettings:ConnectionString"));
        var affected = await connection.ExecuteAsync("DELETE FROM Coupon WHERE ProductName = @ProductName", new { productName });
        return affected != 0;
    }

    public async Task<Coupon> GetDiscount(string productName)
    {
        using var connection = new NpgsqlConnection(_config.GetValue<string>("DatabaseSettings:ConnectionString"));
        var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>("SELECT * FROM Coupon WHERE ProductName = @ProductName", new { productName });
        return coupon
           ?? new Coupon
           {
               ProductName = "No Discount",
               Amount = 0,
               Description = "No Discount Desc"
           };
    }

    public async Task<bool> UpdateDiscount(Coupon coupon)
    {
        using var connection = new NpgsqlConnection(_config.GetValue<string>("DatabaseSettings:ConnectionString"));
        var affected = await connection.ExecuteAsync(
           "UPDATE Coupon SET ProductName = @ProductName, Description = @Description, Amount = @Amount WHERE Id = @Id",
           new
           {
               coupon.ProductName,
               coupon.Description,
               coupon.Amount,
               coupon.Id
           }
        );
        return affected != 0;
    }
}
