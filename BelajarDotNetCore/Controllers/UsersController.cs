using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
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
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BelajarDotNetCoreAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        public IConfiguration configuration;
        private readonly MyContext myContext;
        private readonly SendEmailService sendEmailService;
        private readonly TokenService tokenService;
        

        public UsersController(MyContext myContext, SendEmailService sendEmailService, IConfiguration configuration,TokenService tokenService)
        {
            this.myContext = myContext;
            this.sendEmailService = sendEmailService;
            this.configuration = configuration;
            this.tokenService = tokenService;
        }

        // GET: Users/getAll
        [Authorize(AuthenticationSchemes = "Bearer")]
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
            if (existUser == null)
            {
                return BadRequest("Retrieving User Data Failed !");
            }
            var userRole = this.myContext.UserRoles.Where(Q => Q.UserId == id).Select(Q => Q.RoleId).ToList();
            if (userRole == null)
            {
                return BadRequest("Retrieving User Role Data Failed !");
            }
            var role = this.myContext.Roles.Where(Q => userRole.Any(X => X == Q.Id)).ToList();
            if (userRole == null)
            {
                return BadRequest("Retrieving Role Data Failed !");
            }
            var roleName = role.Select(Q => Q.Name).ToList();

            UserViewModel user = new UserViewModel()
            {
                Id = existUser.Id,
                Email = existUser.Email,
                UserName = existUser.UserName,
                RoleName = roleName
            };
            return Ok(user);
        }

        // GET: Users/Edit/{id}
        [HttpGet]
        [Route("Edit/{id}")]
        public ActionResult GetEdit(string id)
        {
            var existUser = this.myContext.Users.Where(Q => Q.Id == id).FirstOrDefault();
            if (existUser == null)
            {
                return BadRequest("Retrieving User Data Failed !");
            }
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

        // PUT: Users/Edit/{id}
        [HttpPut]
        [Route("Edit/{id}")]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> PostEdit(string id, UserEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Data is not valid !");
            }

            var existUser = await this.myContext.Users.FindAsync(id);
            if (existUser == null)
            {
                return BadRequest("Retrieving User Data Failed !");
            }

            var isValid = BCrypt.Net.BCrypt.Verify(model.OldPassword, existUser.PasswordHash);
            if (!isValid)
            {
                return BadRequest("Old Passoword is wrong !");
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.NewPassword, 12);
            existUser.PasswordHash = hashedPassword;
            existUser.UserName = model.UserName;

            var result = await this.myContext.SaveChangesAsync();
            if (result == 0)
            {
                return BadRequest("Server Error !");
            }
            return Ok(model);
        }

        //GET: Users/Delete/{id}
        [HttpGet]
        [Route("delete/{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var user = await this.myContext.Users.FindAsync(id);
            if (user == null)
            {
                return BadRequest("Retrieving User Data Failed !");
            }
            var selectedUser = new UserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName
            };
            return Ok(selectedUser);
        }

        // POST: Users/delete/5
        [HttpDelete]
        [Route("delete/{id}")]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteUser(string id)
        {
            var existUser = this.myContext.Users.Find(id);
            if (existUser == null)
            {
                return BadRequest("Retrieving User Data Failed !");
            }
            var userRole = this.myContext.UserRoles.Where(Q => Q.UserId == existUser.Id).ToList();
            if (userRole == null)
            {
                return BadRequest("Retrieving User Role Data Failed !");
            }

            this.myContext.UserRoles.RemoveRange(userRole);
            this.myContext.Users.Remove(existUser);

            var result = await this.myContext.SaveChangesAsync();
            if (result == 0)
            {
                return BadRequest("Server Error !");
            }

            return Ok("Delete Scuccess !");
        }

        // GET: /Account/Register
        [AllowAnonymous]
        [HttpGet]
        [Route("register")]
        public ActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.ValidationState);
            }
            var isExist = this.myContext.Users.Where(Q => Q.Email == model.Email).Any();
            if (isExist)
            {
                return BadRequest("Email Already Registered !");
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password, 12);
            var role = this.myContext.Roles.Where(Q => Q.Name == "SALES").FirstOrDefault();
            if (role == null)
            {
                return BadRequest("Default Role Data Not Exist !");
            }
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
            var result = await this.myContext.SaveChangesAsync();
            if (result == 0)
            {
                return BadRequest("Server Error !");
            }
            return Ok("Successfully Created");
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
            if (userRole == null)
            {
                return BadRequest("Retrieving User Role Data Failed !");
            }
            var role = this.myContext.Roles.Where(Q => userRole.Any(X => X == Q.Id)).ToList();
            if (role == null)
            {
                return BadRequest("Retrieving Role Data Failed !");
            }
            var roleName = role.Select(Q => Q.Name).ToList();

            UserViewModel user = new UserViewModel()
            {
                Id = isExist.Id,
                Email = isExist.Email,
                UserName = isExist.UserName,
                RoleName = roleName,
                EmailConfirmed = isExist.EmailConfirmed
            };
            var stringRoles = String.Join(",", user.RoleName.ToArray());

            var claims = new List<Claim> {
                new Claim("Id",user.Id),
                new Claim("Role",stringRoles),
                new Claim("UserName", user.UserName),
                new Claim("Email", user.Email),
                new Claim("IsVerified", user.EmailConfirmed.ToString())
             };

            //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));

            //var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //var token = new JwtSecurityToken(configuration["Jwt:Issuer"], configuration["Jwt:Audience"], claims, expires: DateTime.UtcNow.AddMinutes(1), signingCredentials: signIn);

            user.JWToken = this.tokenService.GenerateAccessToken(claims);

            return Ok(user);

            //return Ok(user);
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
            if (user == null)
            {
                return BadRequest("Retrieving User Data Failed !");
            }

            user.SecurityStamp = null;
            user.EmailConfirmed = true;

            var result = await this.myContext.SaveChangesAsync();
            if (result == 0)
            {
                return BadRequest("Server Error !");
            }

            return Ok("Account Verified !");
        }
    }
}