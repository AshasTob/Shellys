using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess.Data;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DataAccess.Repository
{
    public class MenuRepository : IMenuRepository
    {
        private BarDataBase _barDataBase;

        public MenuRepository()
        {
            _barDataBase = new BarDataBase();
        }
                 
        public async Task<MenuItem> Add(MenuItem item)
        {
            if (item == null) throw new NullReferenceException();
            EntityEntry<MenuItem> added = await _barDataBase.AddAsync(item);
            int affacted = await _barDataBase.SaveChangesAsync();
            if(affacted == 1)
            {
                return item;
            }
            else
            {
                return null;
            }
        }

        public async Task<MenuItem> GetItem(int id)
        {
            MenuItem menuItem = await _barDataBase.Menu.FindAsync(id);
            return menuItem;
        }

        public  Task<List<MenuItem>> GetAllItems()
        {
            return  Task<List<MenuItem>>.FromResult(_barDataBase.Menu.ToList());
        }

        public async Task<bool> Update (MenuItem menuItem)
        {
            _barDataBase.Menu.Update(menuItem);
            int affected = await _barDataBase.SaveChangesAsync();
            if(affected == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> Remove(int id)
        {
            MenuItem menuItem = _barDataBase.Menu.Find(id);
            if (menuItem == null) throw new ArgumentException($"No items with id={id} was found");
            _barDataBase.Menu.Remove(menuItem);
            int affected = await _barDataBase.SaveChangesAsync();
            if(affected == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
