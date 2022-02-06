using System.Threading.Tasks;
using DataAccess.Data;

namespace DataAccess.Repository
{
    public interface IOrderRepository
    {
        Task<Order> Get(int id);
        Task<int> Upsert(Order order);
    }
}
