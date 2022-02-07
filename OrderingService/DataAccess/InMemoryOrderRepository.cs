using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Data;
using DataAccess.Repository;

namespace OrderingService.DataAccess
{
    public class InMemoryOrderRepository : IOrderRepository
    {
        private static readonly List<Order> _orders = new List<Order>();
        private static int _idIndex = 1;

        public Task<Order> Get(int id)
        {
            return Task.FromResult(_orders.FirstOrDefault(o => o.Id == id));
        }

        public Task<int> Upsert(Order order)
        {
            var index = _orders.FindIndex(o => o == order);
            if (index == -1)
            {
                order.Id = _idIndex;
                _orders.Add(order);
                _idIndex++;
            }
            else
            {
                _orders[index] = order;
            }

            return Task.FromResult(order.Id);
        }
    }
}
