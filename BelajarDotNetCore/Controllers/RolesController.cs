using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BelajarDotNetCoreAPI.Context;
using BelajarDotNetCoreAPI.Models;
using BelajarDotNetCoreAPI.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BelajarDotNetCoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : Controller
    {
        private readonly MyContext myContext;
        public RolesController(MyContext myContext)
        {
            this.myContext = myContext;
        }

        // GET: Roles
        [HttpGet]
        [Route("getAll")]
        public ActionResult GetAllRoles()
        {
            var roles = this.myContext.Roles.ToList();
            List<RoleViewModel> list = new List<RoleViewModel>();
            foreach (var role in roles)
            {
                RoleViewModel roleData = new RoleViewModel()
                {
                    Id = role.Id,
                    Name = role.Name
                };
                list.Add(roleData);
            }
            return Ok(list);
        }

        // GET: Roles/Details/5
        [HttpGet]
        [Route("Details/{id}")]
        public async Task<ActionResult> GetRoleById(string id)
        {
            var existRole = await this.myContext.Roles.FindAsync(id);
            Role user = new Role()
            {
                Id = existRole.Id,
                Name = existRole.Name
            };
            return Ok(user);
        }

        // GET: Roles/Create
        [HttpGet]
        [Route("create")]
        public ActionResult Create()
        {
           
            return View();
        }

        // POST: Roles/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Role role)
        {
            Role user = new Role()
            {
                Id = Guid.NewGuid().ToString(),
                Name = role.Name
            };
            this.myContext.Roles.Add(user);
            await this.myContext.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        [Route("edit/{id}")]
        // GET: Roles/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            var existRole = await this.myContext.Roles.FindAsync(id);

            var userRoles = this.myContext.UserRoles.Join(this.myContext.Users, ur => ur.UserId, u => u.Id, (ur, u) => new { UserRoles = ur, Users = u }).Where(Q => Q.UserRoles.RoleId == id).ToList();

            List<UserRoleViewModel> users = new List<UserRoleViewModel>();
            foreach (var userRole in userRoles)
            {
                UserRoleViewModel user = new UserRoleViewModel()
                {
                    UserId = userRole.Users.Id,
                    UserName = userRole.Users.UserName
                };
                users.Add(user);
            }

            ManageRoleViewModel model = new ManageRoleViewModel()
            {
                Id = existRole.Id,
                RoleName = existRole.Name,
                Users = users
            };
            return Ok(model);
        }

        // POST: Roles/Edit/5
        [HttpPost]
        [Route("edit/{id}")]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> PostEdit(string id)
        {
            var existRole = await this.myContext.Roles.FindAsync(id);


            await this.myContext.SaveChangesAsync();

            return Ok("Successfully Update");
        }

        // POST: Roles/Manage/{id}
        [HttpPost]
        [Route("manage/{id}")]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Manage(string id, ManageRoleViewModel role)
        {
            var existRole = await this.myContext.Roles.FindAsync(id);

           // existRole.Name = role.Name;

            //await this.myContext.SaveChangesAsync();

            return Ok("Successfully Update");
        }


        // GET: Roles/Delete/5
        [HttpGet]
        [Route("delete/{id}")]
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Roles/Delete/5
        [HttpPost]
        [Route("delete/{id}")]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteRole(int id)
        {
            var existRole = this.myContext.Roles.Find(id);

            this.myContext.Roles.Remove(existRole);
            await this.myContext.SaveChangesAsync();

            return Ok("Delete Scuccess !");
        }
    }
}