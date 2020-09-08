using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BelajarDotNetCoreAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BelajarDotNetCoreAPI.Base
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController<TEntity, TRepository> : ControllerBase
        where TEntity : class
        where TRepository : IRepository<TEntity>
    {
        private IRepository<TEntity> _repo;

        public BaseController(TRepository repository)
        {
            _repo = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TEntity>>> GetAll()
        {
            var allItem = await _repo.GetAll();
            return Ok(allItem);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TEntity>> GetById(int id) => await _repo.GetById(id);

        [HttpPost]
        public async Task<ActionResult> Create(TEntity entity)
        {
            var data = await _repo.Create(entity);
            if (data > 0)
            {
                return Ok("Data Successfully Saved !");
            }
            return BadRequest("Data failed to Save !");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _repo.Delete(id);
            if (result > 0)
            {
                return Ok("Data Successfully Deleted !");
            }
            return BadRequest("Data failed to Delete !");
        }


    }
}