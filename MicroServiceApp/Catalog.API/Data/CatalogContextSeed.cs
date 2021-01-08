using System.Collections.Generic;
using Domain.Models;
using MongoDB.Driver;

namespace Catalog.API.Data
{
    public class CatalogContextSeed
    {
        public static void SeedData(IMongoCollection<Product> productCollection)
        {
            // If any products exist
            var productExists = productCollection.Find(p => true).Any();
            if (!productExists)
            {
                productCollection.InsertManyAsync(GetPreconfiguredProducts());
            }
        }

        private static IEnumerable<Product> GetPreconfiguredProducts()
        {
            return new List<Product>()
            {
                new Product()
                {
                    Name = "Alienware Desktop",
                    Summery = "Summary",
                    Description = "Description Alienware",
                    ImageFile = "product-1.png",
                    Price = 2000.00M,
                    Category = "Desktop"
                },
                new Product()
                {
                    Name = "ASUS Laptop",
                    Summery = "Summary",
                    Description = "Laptop",
                    ImageFile = "product-2.png",
                    Price = 200.00M,
                    Category = "Laptop"
                },
                new Product()
                {
                    Name = "Lenovo Laptop",
                    Summery = "Summary",
                    Description = "Lenovo",
                    ImageFile = "product-3.png",
                    Price = 260.00M,
                    Category = "Laptop"
                }
            };
        }

    }
}
