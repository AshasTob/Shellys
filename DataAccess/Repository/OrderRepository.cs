using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Data;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DataAccess.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private BarDataBase _barDataBase;

        public OrderRepository()
        {
            _barDataBase = new BarDataBase();
        }

        public async Task<Order> Get(int id)
        {
            Order order = await _barDataBase.Orders.FindAsync(id);
            if(order == null)
            {
                throw new ArgumentException($"No order with {id} was found");
            }
            else
            {
                return order;
            }
        }

        public async Task<int> Upsert(Order order)
        {
            Order ord = _barDataBase.Orders.Find(order.Id);
            int affrcted;
            if (ord == null)
            {
                _barDataBase.Orders.Add(order);
                affrcted = await _barDataBase.SaveChangesAsync();
                return affrcted;
            }
            else
            {
                _barDataBase.Orders.Update(order);
                affrcted = await _barDataBase.SaveChangesAsync();
                return affrcted;
            }
        }
    }
}
