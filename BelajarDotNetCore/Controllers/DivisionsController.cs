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
    public class DivisionsController : BaseController<Division, DivisionRepository>
    {
        private readonly DivisionRepository _divisionRepository;

        public DivisionsController(DivisionRepository divisionRepository):base(divisionRepository)
        {
            this._divisionRepository = divisionRepository;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, Division model)
        {
            var item = await _divisionRepository.GetById(id);
            item.UpdateDate = DateTimeOffset.Now;
            item.Name = model.Name;
            item.DepartmentId = model.DepartmentId;

            var result = await _divisionRepository.Update(item);
            if (result > 0)
            {
                return Ok("Data Successfully Edited !");

            }
            return BadRequest("Data Failed to Edit !");
        }
    }
}