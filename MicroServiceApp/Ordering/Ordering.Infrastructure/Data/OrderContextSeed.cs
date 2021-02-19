using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ordering.Core.Entities;

namespace Ordering.Infrastructure.Data
{
    public class OrderContextSeed
    {

        public static async Task SeedAsync(OrderContext context, ILoggerFactory loggerFactory, int? retry)
        {
            var retryForAvailability = retry.Value;

                try
            {
                await context.Database.MigrateAsync();

                if (!context.Orders.Any())
                {
                    await context.Orders.AddRangeAsync(GetPreconfiguredOrders());
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {

                // Recursive call

                if (retryForAvailability < 3)
                {
                    retryForAvailability++;
                    var log = loggerFactory.CreateLogger<OrderContextSeed>();
                    log.LogError(ex.Message);
                    await SeedAsync(context, loggerFactory, retryForAvailability);
                }
            }
        }

        private static IEnumerable<Order> GetPreconfiguredOrders()
        {
            return new List<Order>()
            {
                new Order() {
                    UserName = "RichieAxe",
                    FirstName = "Richie",
                    LastName = "Axelsson",
                    Email = "Richiehstad@gmail.com",
                    Address = "Vägen 8",
                    Country = "Sweden"
                },
                new Order() {
                    UserName = "SaraMoore",
                    FirstName = "Sara",
                    LastName = "Kling",
                    Email = "Sara.Kling@hotmail.com",
                    Address = "Vägen 8",
                    Country = "Sweden"
                }
            };
        }
    }
}
