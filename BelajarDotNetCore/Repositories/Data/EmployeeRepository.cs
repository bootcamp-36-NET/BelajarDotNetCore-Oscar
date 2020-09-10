using BelajarDotNetCoreAPI.Context;
using BelajarDotNetCoreAPI.Models;
using BelajarDotNetCoreAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BelajarDotNetCoreAPI.Repositories.Data
{
    public class EmployeeRepository
    {
        private readonly MyContext myContext;

        public EmployeeRepository(MyContext myContext)
        {
            this.myContext = myContext;
        }

        public async Task<int> Create(Employee model)
        {
            model.JoinDate = DateTimeOffset.Now;
            model.IsActive = true;
            await this.myContext.Employees.AddAsync(model);
            var result = await this.myContext.SaveChangesAsync();
            return result;
        }

        public async Task<int> Delete(string id)
        {
            var item = await GetById(id);
            if (item == null)
            {
                return 0;
            }

            item.IsActive = false;

            this.myContext.Entry(item).State = EntityState.Modified;
            var result = await this.myContext.SaveChangesAsync();
            return result;
        }

        public async Task<List<Employee>> GetAll()
        {
            var allItem = await this.myContext.Employees.Where(Q => Q.IsActive == true).ToListAsync();
            return allItem;
        }

        public async Task<Employee> GetById(string id)
        {
            var item = await this.myContext.Employees.FindAsync(id);
            return item;
        }

        public async Task<int> Update(Employee model)
        {
            this.myContext.Entry(model).State = EntityState.Modified;
            var result = await this.myContext.SaveChangesAsync();
            return result;
        }
    }
}
