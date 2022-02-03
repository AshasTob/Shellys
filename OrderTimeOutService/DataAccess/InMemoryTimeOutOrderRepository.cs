using DataAccess.Domain;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace OrderTimeOutService.DataAccess
{
    public class InMemoryTimeOutOrderRepository : ITimeOutOrderRepository
    {
        private static readonly List<Order> _orders = new List<Order>();

        public Task<IEnumerable<Order>> Get(int orderStatus)
        {
            return Task.FromResult(_orders.Where(o => ((int)o.Status) == orderStatus));
        }

        public Task<int> Update(Order order)
        {
            var index = _orders.FindIndex(o => o == order);
            if (index == -1)
            {
                return Task.FromResult(-1);
            }
            order.Status = OrderStatus.Finalized;
            _orders[index] = order;
            return Task.FromResult(order.Id);
        }
    }
}
