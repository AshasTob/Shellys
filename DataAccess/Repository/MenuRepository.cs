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
                 
        public Task<bool> Add(MenuItem item)
        {
            EntityEntry<MenuItem> added =  _barDataBase.Add(item);
            int affected = _barDataBase.SaveChanges();
            bool isAdded = (affected == 1) ? true : false;
            return Task.FromResult(isAdded);
        }

        public Task<MenuItem> GetItem(int id)
        {
            MenuItem menuItem = _barDataBase.Menu.Find(id);
            return Task.FromResult(menuItem);
        }

        public  Task<List<MenuItem>> GetAllItems()
        {
            return  Task<List<MenuItem>>.FromResult(_barDataBase.Menu.ToList());
        }

        public Task<bool> Update (MenuItem menuItem)
        {
            _barDataBase.Menu.Update(menuItem);
            int affected = _barDataBase.SaveChanges();
            bool isUpdate = (affected == 1) ? true : false;
            return Task.FromResult(isUpdate);
        }

        public Task<bool> Remove(int id)
        {
            MenuItem menuItem = _barDataBase.Menu.Find(id);
            if (menuItem == null) return Task.FromResult(false);
            _barDataBase.Menu.Remove(menuItem);
            int affected = _barDataBase.SaveChanges();
            bool isRemove = (affected == 1) ? true : false;
            return Task.FromResult(isRemove);
        }
    }
}
