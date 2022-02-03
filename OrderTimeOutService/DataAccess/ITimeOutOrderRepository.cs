using DataAccess.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace OrderTimeOutService.DataAccess
{
    public interface ITimeOutOrderRepository
    {
        Task<IEnumerable<Order>> Get(int orderStatus);
        Task<int> Update(Order order);
    }
}
