using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrderingService.DataAccess
{
    public interface IOrderRepository
    {
        Task<Order> Get(int id);
        Task Upsert(Order order);
    }
}
