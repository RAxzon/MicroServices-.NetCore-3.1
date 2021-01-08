using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Basket.API.Models;

namespace Basket.API.Repositories.Interfaces
{
    public interface IBasketRepository
    {
        //Task<IEnumerable<BasketCart>> GetBaskets();
        //Task<BasketCart> GetBasket(int id);
        Task<BasketCart> GetBasketByUserName(string userName);
        //Task Create(BasketCart basketCart);
        Task<BasketCart> Update(BasketCart basketCart);
        Task<bool> Delete(string userName);
    }
}
