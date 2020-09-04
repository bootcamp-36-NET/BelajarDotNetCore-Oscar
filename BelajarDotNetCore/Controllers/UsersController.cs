﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BCrypt.Net;
using BelajarDotNetCoreAPI.Context;
using BelajarDotNetCoreAPI.Models;
using BelajarDotNetCoreAPI.Services;
using BelajarDotNetCoreAPI.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BelajarDotNetCoreAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly MyContext myContext;
        private readonly SendEmailService sendEmailService;
        //private readonly UserManager<User> userManager;
        // private readonly SignInManager<User> signInManager;

        public UsersController(MyContext myContext, SendEmailService sendEmailService)
        {
            this.myContext = myContext;
            this.sendEmailService = sendEmailService;
            //this.userManager = userManager;
            //this.signInManager = signInManager;
        }

        // GET: Users
        [HttpGet]
        [Route("getAll")]
        public ActionResult GetAllUsers()
        {
            var users = this.myContext.Users.ToList();
            List<UserListViewModel> list = new List<UserListViewModel>();
            foreach (var user in users)
            {
                UserListViewModel userdata = new UserListViewModel()
                {
                    Id = user.Id,
                    Email = user.Email,
                    UserName = user.UserName
                };
                list.Add(userdata);
            }
            return Ok(list);
        }

        // GET: Users/Details/{id}
        [HttpGet]
        [Route("Details/{id}")]
        public async Task<ActionResult> GetUserById(string id)
        {

            var existUser = await this.myContext.Users.FindAsync(id);
            var userRole = this.myContext.UserRoles.Where(Q => Q.UserId == id).Select(Q => Q.RoleId).ToList();
            var role = this.myContext.Roles.Where(Q => userRole.Any(X => X == Q.Id)).ToList();
            var roleName = role.Select(Q => Q.Name).ToList();


            UserViewModel user = new UserViewModel()
            {
                Id = existUser.Id,
                Email = existUser.Email,
                UserName = existUser.UserName,
                RoleName = roleName
            };
            return Ok(user);

            //var user = await userManager.FindByIdAsync(id);
            ////var userRoles = await userManager.GetRolesAsync(user.Id);
            //var selectedUser = new UserViewModel
            //{
            //    Id = user.Id,
            //    Email = user.Email,
            //    UserName = user.UserName
            //    //RoleName = userRoles.FirstOrDefault()
            //};
            //return Ok(selectedUser);
        }

        // GET: Users/{id}
        [HttpGet]
        [Route("Edit/{id}")]
        public ActionResult GetEdit(string id)
        {
            //List<SelectListItem> list = new List<SelectListItem>();
            //foreach (var role in RoleManager.Roles)
            //{
            //    list.Add(new SelectListItem() { Value = role.Name, Text = role.Name });
            //}
            //ViewBag.Roles = list;

            var existUser = this.myContext.Users.Where(Q => Q.Id == id).FirstOrDefault();
            // var userRole = this.myContext.UserRoles.Where(Q => Q.UserId == id).Select(Q => Q.RoleId).ToList();
            //var role = this.myContext.Roles.Where(Q => userRole.Any(X => X == Q.Id)).ToList();
            //var roleName = role.Select(Q => Q.Name).ToList();
            //var user = await userManager.FindByIdAsync(id);

            //var userRoles = await userManager.GetRolesAsync(id);
            var editedUser = new UserEditViewModel
            {
                Id = existUser.Id,
                UserName = existUser.UserName
            };
            return Ok(editedUser);
        }

        // PUT: Users/{id}
        [HttpPut]
        [Route("Edit/{id}")]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> PostEdit(string id, UserEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("failed to update !");
            }
            var existUser = await this.myContext.Users.FindAsync(id);
            var existUserRole = this.myContext.UserRoles.Where(Q => Q.UserId == existUser.Id);

            var isValid = BCrypt.Net.BCrypt.Verify(model.OldPassword, existUser.PasswordHash);
            if (!isValid)
            {
                return BadRequest("failed to update !");
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.NewPassword, 12);
            existUser.PasswordHash = hashedPassword;
            existUser.UserName = model.UserName;
            //existUser.Email = model.Email;
            //existUser.PhoneNumber = model.Phone;

            await this.myContext.SaveChangesAsync();
            return Ok(model);



            //var user = await userManager.FindByIdAsync(id);
            //user.UserName = model.UserName;
            //await userManager.UpdateAsync(user);

            //var userRoles = await UserManager.GetRolesAsync(model.Id);
            //await UserManager.RemoveFromRoleAsync(model.Id, userRoles.FirstOrDefault());
            //await UserManager.AddToRoleAsync(model.Id, model.RoleName);

            //return Ok("Edit Success !");
        }

        // //GET: Users/Delete/{id}
        //[HttpGet]
        //[Route("delete/{id}")]
        //public async Task<ActionResult> Delete(string id)
        //{
        //    var user = await userManager.FindByIdAsync(id);
        //    var userRoles = await userManager.GetRolesAsync(id);
        //    var selectedUser = new UserViewModel
        //    {
        //        Id = user.Id,
        //        Email = user.Email,
        //        UserName = user.UserName
        //        //RoleName = userRoles.FirstOrDefault()
        //    };
        //    return Ok(selectedUser);
        //}

        // POST: Users/Delete/5
        [HttpDelete]
        [Route("delete/{id}")]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteUser(string id)
        {
            var existUser = this.myContext.Users.Find(id);

            this.myContext.Users.Remove(existUser);
            await this.myContext.SaveChangesAsync();

            return Ok("Delete Scuccess !");

            //var user = await userManager.FindByIdAsync(id);
            //await userManager.DeleteAsync(user);
            //return Ok("Delete Scuccess !");
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        [HttpGet]
        [Route("register")]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password, 12);
            var role = this.myContext.Roles.Where(Q => Q.Name == "SALES").FirstOrDefault();
            var userId = Guid.NewGuid().ToString();

            var rand = new Random();
            var emailRandomCode = rand.Next(0, 9999).ToString("D4");

            sendEmailService.SendEmail(model.Email, emailRandomCode);

            User user = new User()
            {
                Id = userId,
                UserName = model.Email,
                Email = model.Email,
                NormalizedEmail = model.Email.ToUpper(),
                SecurityStamp = emailRandomCode,
                EmailConfirmed = false,
                PasswordHash = hashedPassword,
                PhoneNumber = null,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                AccessFailedCount = 0
            };

            UserRole userRole = new UserRole()
            {
                UserId = userId,
                RoleId = role.Id
            };

            this.myContext.Users.Add(user);
            this.myContext.UserRoles.Add(userRole);
            await this.myContext.SaveChangesAsync();
            return Ok("Successfully Created");

            ////Register using UserManager
            //if (ModelState.IsValid)
            //    {
            //        var user = new User { UserName = model.Email, Email = model.Email };
            //        var result = await userManager.CreateAsync(user, model.Password);
            //        if (result.Succeeded)
            //        {
            //            //await signInManager.SignInAsync(user, isPersistent: false);
            //            return Ok("Register Succeed !");
            //        }
            //        //foreach (var error in result.Errors)
            //        //{
            //        //    ModelState.AddModelError("", error.Description);
            //        //}
            //    }
            //return BadRequest("Register Error !");
        }

        [HttpPost]
        [Route("Login")]
        public ActionResult Login(LoginViewModel loginViewModel)
        {
            var isExist = this.myContext.Users.Where(Q => Q.Email == loginViewModel.Email || Q.UserName == loginViewModel.Email).FirstOrDefault();
            if (isExist == null)
            {
                return BadRequest("User not registered !");
            }
            var isValid = BCrypt.Net.BCrypt.Verify(loginViewModel.Password, isExist.PasswordHash);
            if (!isValid)
            {
                return BadRequest("Password not match !");
            }

            var userRole = this.myContext.UserRoles.Where(Q => Q.UserId == isExist.Id).Select(Q => Q.RoleId).ToList();
            var role = this.myContext.Roles.Where(Q => userRole.Any(X => X == Q.Id)).ToList();
            var roleName = role.Select(Q => Q.Name).ToList();

            UserViewModel user = new UserViewModel()
            {
                Id = isExist.Id,
                Email = isExist.Email,
                UserName = isExist.UserName,
                RoleName = roleName,
                EmailConfirmed = isExist.EmailConfirmed
            };

            return Ok(user);
        }

        [HttpPost]
        [Route("verify/{id}")]
        public async Task<IActionResult> Verify(string id, string code)
        {
            var isTrue = this.myContext.Users.Where(Q => Q.SecurityStamp == code).Any();
            if (!isTrue)
            {
                return BadRequest("Verification Code is Wrong !");
            }

            var user = this.myContext.Users.Where(Q => Q.Id == id).FirstOrDefault();

            user.SecurityStamp = null;
            user.EmailConfirmed = true;

            await this.myContext.SaveChangesAsync();

            return Ok("Account Verified !");
        }
    }
}