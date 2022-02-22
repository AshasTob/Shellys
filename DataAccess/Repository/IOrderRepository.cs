using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess.Data;

namespace DataAccess.Repository
{
    public interface IOrderRepository
    {
        Task<Order> Get(int id);
        Task<List<Order>> GetOrdersByStatus(OrderStatus status);
        Task<bool> Upsert(Order order);
    }
}
