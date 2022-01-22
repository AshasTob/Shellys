using System.Collections.Generic;
using System.Threading.Tasks;
using BarAPI.Domain;

namespace BarAPI.DataAccess
{
    public interface IMenuRepository
    {
        public Task Add(MenuItem item);
        public Task<MenuItem> GetItem(string name);
        public Task<MenuItem[]> Get();
        public Task Remove(string name);
    }
}
