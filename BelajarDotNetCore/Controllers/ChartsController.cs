using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BelajarDotNetCoreAPI.Context;
using BelajarDotNetCoreAPI.ViewModel.ChartViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BelajarDotNetCoreAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ChartsController : ControllerBase
    {
        private readonly MyContext _myContext;

        public ChartsController(MyContext myContext)
        {
            this._myContext = myContext;
        }

        [HttpGet]
        [Route("pie")]
        public async Task<List<PieChartVM>> GetPie()
        {
            var chartdata = await _myContext.Divisions.Include("Department")
                            .Where(x => x.IsDelete == false)
                            .GroupBy(q => q.Department.Name)
                            .Select(q => new PieChartVM
                            {
                                DepartmentName = q.Key,
                                Total = q.Count()
                            })
                            .ToListAsync();

            return chartdata;
        }
    }
}