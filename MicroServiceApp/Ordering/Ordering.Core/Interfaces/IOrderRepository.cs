using System.Collections.Generic;
using System.Threading.Tasks;
using Ordering.Core.Entities;
using Ordering.Core.Interfaces.Base;

namespace Ordering.Core.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<IEnumerable<Order>> GetOrderByUserName(string userName);
    }
}
