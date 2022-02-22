using System;
using System.Collections.Generic;
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

        public  Task<Order> Get(int id)
        {
            Order order = _barDataBase.Orders.Find(id);
            return Task.FromResult(order);
        }

        public Task<List<Order>> GetOrdersByStatus(OrderStatus status)
        {
            return Task<List<Order>>.FromResult(_barDataBase.Orders.Where(order => order.Status == status).ToList());
        }

        public Task<bool> Upsert(Order order)
        {
            bool ord = _barDataBase.Orders.Contains(order);          
            if (ord == false)
            {
                _barDataBase.Orders.Add(order);
            }
            else
            {
                _barDataBase.Orders.Update(order);
            }
            int affected = _barDataBase.SaveChanges();
            bool isUpsert = (affected == 1) ? true : false;
            return Task.FromResult(isUpsert);
        }
    }
}
