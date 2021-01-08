using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Basket.API.Data.Interfaces;
using Basket.API.Models;
using Basket.API.Repositories.Interfaces;
using Newtonsoft.Json;

namespace Basket.API.Repositories
{
    public class BasketRepository : IBasketRepository
    {

        private readonly IBasketContext _context;

        public BasketRepository(IBasketContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        //public async Task<IEnumerable<BasketCart>> GetBaskets()
        //{
            
        //}

        //public async Task<BasketCart> GetBasket(int id)
        //{

        //}

        public async Task<BasketCart> GetBasketByUserName(string userName)
        {
            var basket = await _context.Redis.StringGetAsync(userName);

            return JsonConvert.DeserializeObject<BasketCart>(basket);
        }

        //public async Task Create(BasketCart basketCart)
        //{
            
        //}

        public async Task<BasketCart> Update(BasketCart basketCart)
        {
            var updated =
                await _context.Redis.StringSetAsync(basketCart.UserName, JsonConvert.SerializeObject(basketCart));

            return await GetBasketByUserName(basketCart.UserName);
        }

        public async Task<bool> Delete(string userName)
        {
            return await _context.Redis.KeyDeleteAsync(userName);
        }
    }
}
