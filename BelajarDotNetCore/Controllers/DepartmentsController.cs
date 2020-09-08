using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BelajarDotNetCoreAPI.Base;
using BelajarDotNetCoreAPI.Models;
using BelajarDotNetCoreAPI.Repositories.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BelajarDotNetCoreAPI.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : BaseController<Department, DepartmentRepository>
    {
        private readonly DepartmentRepository _repo;

        public DepartmentsController(DepartmentRepository departmentRepository) : base(departmentRepository)
        {
            this._repo = departmentRepository;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, Department model)
        {
            var item = await _repo.GetById(id);
            item.UpdateDate = DateTimeOffset.Now;
            item.Name = model.Name;

            var result = await _repo.Update(item);
            if (result > 0)
            {
                return Ok("Data Successfully Edited !");
                
            }
            return BadRequest("Data Failed to Edit !");
        }
    }
}