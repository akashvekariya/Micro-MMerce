using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Persistence;

public class OrderContextSeed
{
    public static async Task SeedAsync(OrderContext orderContext, ILogger<OrderContextSeed> logger)
    {
        if (!orderContext.Orders.Any())
        {
            orderContext.Orders.AddRange(GetPreconfiguredOrders());
            await orderContext.SaveChangesAsync();
            logger.LogInformation($"Seed database associated with context {typeof(OrderContext).Name}");
        }
    }

    private static IEnumerable<Order> GetPreconfiguredOrders()
    {
        return new List<Order>()
        {
            new Order() {
                UserName = "akash1234",
                FirstName = "Akash",
                LastName = "Vekariya",
                EmailAddress = "akash@example.com",
                AddressLine = "HSR, Bengaluru",
                Country = "India",
                State = "Karnataka",
                ZipCode = "123456",
                CardName = "Akash Vekariya",
                CardNumber = "1234567890123456",
                Expiration = "12/2025",
                CVV = "123",
                PaymentMethod = 1,
                TotalPrice = 350
            }
        };
    }
}
