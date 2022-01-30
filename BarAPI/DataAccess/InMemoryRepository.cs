using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess;
using DataAccess.Domain;

namespace BarAPI.DataAccess
{
    public class InMemoryRepository: IMenuRepository
    {
        private static List<MenuItem> _positions = new()
        {
            new MenuItem
            {
                Name = "Negroni",
                Price = 6.99
            },
            new MenuItem
            {
                Name = "Margarita",
                Price = 8.99
            },
            new MenuItem
            {
                Name = "Apple Martini",
                Price = 7
            }
        };

        public Task Add(MenuItem item)
        {
            _positions.Add(item);
            return Task.CompletedTask;
        }

        public Task<MenuItem[]> Get()
        {
            return Task.FromResult(_positions.ToArray());
        }

        public Task<MenuItem> GetItem(string name)
        {
            return Task.FromResult(_positions.FirstOrDefault(p => p.Name == name));
        }

        public Task Remove(string name)
        {
            _positions.RemoveAt(_positions.FindIndex(p=>p.Name==name));
            return Task.CompletedTask;
        }
    }
}
