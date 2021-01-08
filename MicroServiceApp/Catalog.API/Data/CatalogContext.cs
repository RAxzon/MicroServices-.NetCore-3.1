using Catalog.API.Data.Interfaces;
using Catalog.API.Settings;
using Domain.Models;
using MongoDB.Driver;

namespace Catalog.API.Data
{
    public class CatalogContext : ICatalogContext
    {
        public IMongoCollection<Product> Products { get; }

        public CatalogContext(ICatalogDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var db = client.GetDatabase(settings.DatabaseName);
            Products = db.GetCollection<Product>(settings.CollectionName);
            CatalogContextSeed.SeedData(Products);
        }
    }
}
