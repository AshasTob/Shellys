using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess.Data;

namespace DataAccess.Repository
{
    public interface IMenuRepository
    {
        Task<MenuItem> Add(MenuItem item);
        Task<MenuItem> GetItem(int id);
        Task<List<MenuItem>> GetAllItems();
        Task<bool> Update(MenuItem menuItem);
        Task<bool> Remove(int id);
    }
}
