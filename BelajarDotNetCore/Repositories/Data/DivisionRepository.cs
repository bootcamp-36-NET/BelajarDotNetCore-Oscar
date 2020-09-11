using BelajarDotNetCoreAPI.Context;
using BelajarDotNetCoreAPI.Models;
using BelajarDotNetCoreAPI.Repositories.Interfaces;
using BelajarDotNetCoreAPI.ViewModel.DivisionViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BelajarDotNetCoreAPI.Repositories.Data
{
    public class DivisionRepository : GeneralRepository<Division, MyContext>
    {
        private readonly MyContext _myContext;
        public DivisionRepository(MyContext myContext) : base(myContext)
        {
            this._myContext = myContext;
        }

        public override async Task<List<Division>> GetAll()
        {
            var data = await this._myContext.Divisions.Include("Department").Where(q => q.IsDelete == false).GroupBy(q => q.Department.Name).Select(q => new
            {
                GroupId = q.Key,
                Count = q.Count()
            }).ToListAsync();
            //var departments = await this._myContext.Departments.ToListAsync();
            var allItem = await this._myContext.Divisions.Include("Department").Where(q => q.IsDelete == false).ToListAsync();

            //var query = from di in this._myContext.Divisions
            //            join de in this._myContext.Departments on di.DepartmentId equals de.Id
            //            where di.IsDelete == false
            //            select new Division
            //            {
            //                Id = di.Id,
            //                Name = di.Name,
            //                CreateDate = di.CreateDate,
            //                UpdateDate = di.UpdateDate,
            //                IsDelete = di.IsDelete,
            //                DeleteDate = di.DeleteDate,
            //                DepartmentId = de.Id,
            //                Department = de
            //            };


            //List<Division> allItem = await query.ToListAsync();

            return allItem;
        }
    }
}
