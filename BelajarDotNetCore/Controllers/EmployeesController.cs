using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BelajarDotNetCoreAPI.Base;
using BelajarDotNetCoreAPI.Models;
using BelajarDotNetCoreAPI.Repositories.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BelajarDotNetCoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : Controller
    {
        private readonly EmployeeRepository _employeeRepository;

        public EmployeesController(EmployeeRepository employeeRepository)
        {
            this._employeeRepository = employeeRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetAll()
        {
            var allItem = await _employeeRepository.GetAll();
            return Ok(allItem);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetById(string id)
        {
            return await _employeeRepository.GetById(id);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Employee model)
        {
            var data = await _employeeRepository.Create(model);
            if (data > 0)
            {
                return Ok("Data Successfully Saved !");
            }
            return BadRequest("Data failed to Save !");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute]string id)
        {
            var result = await _employeeRepository.Delete(id);
            if (result > 0)
            {
                return Ok("Data Successfully Deleted !");
            }
            return BadRequest("Data failed to Delete !");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(string id, Employee model)
        {
            var item = await _employeeRepository.GetById(id);
            item.FirstName = model.FirstName;
            item.LastName = model.LastName;

            var result = await _employeeRepository.Update(item);
            if (result > 0)
            {
                return Ok("Data Successfully Edited !");

            }
            return BadRequest("Data Failed to Edit !");
        }
    }
}