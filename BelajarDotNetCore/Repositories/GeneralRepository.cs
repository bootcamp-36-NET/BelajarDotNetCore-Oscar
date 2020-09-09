using BelajarDotNetCoreAPI.Base;
using BelajarDotNetCoreAPI.Context;
using BelajarDotNetCoreAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BelajarDotNetCoreAPI.Repositories
{
    public class GeneralRepository<TEntity, TContext> : IRepository<TEntity>
        where TEntity : class, BaseModel
        where TContext : MyContext
    {
        private readonly MyContext myContext;

        public GeneralRepository(MyContext myContext)
        {
            this.myContext = myContext;
        }

        public async Task<int> Create(TEntity entity)
        {
            entity.CreateDate = DateTimeOffset.Now;
            entity.IsDelete = false;
            await this.myContext.Set<TEntity>().AddAsync(entity);
            var result = await this.myContext.SaveChangesAsync();
            return result;
        }

        public async Task<int> Delete(int id)
        {
            var item = await GetById(id);
            if (item == null)
            {
                return 0;
            }

            item.DeleteDate = DateTimeOffset.Now;
            item.IsDelete = true;

            this.myContext.Entry(item).State = EntityState.Modified;
            var result = await this.myContext.SaveChangesAsync();
            return result;

        }

        public virtual async Task<List<TEntity>> GetAll()
        {
            var allItem = await this.myContext.Set<TEntity>().Where(Q=>Q.IsDelete == false).ToListAsync();
            //if(!allItem.Count().Equals(0))
            //{
                return allItem;
            //}
            //return null;
        }

        public async Task<TEntity> GetById(int id)
        {
            var item = await this.myContext.Set<TEntity>().FindAsync(id);
            return item;
        }

        public async Task<int> Update(TEntity entity)
        {
            this.myContext.Entry(entity).State = EntityState.Modified;
            var result = await this.myContext.SaveChangesAsync();
            return result;
        }
    }
}
