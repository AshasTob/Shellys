using System.Threading.Tasks;
using DataAccess.Domain;

namespace DataAccess
{
    public interface IOrderRepository
    {
        Task<Order> Get(int id);
        Task<int> Upsert(Order order);
    }
}
