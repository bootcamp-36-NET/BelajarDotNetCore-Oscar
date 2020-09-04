using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BelajarDotNetCoreAPI.Models;
using BelajarDotNetCoreAPI.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BelajarDotNetCoreClient.Controllers
{
    public class LoginController : Controller
    {
        readonly HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:44321/api/")
        };

        // GET: Login
        public ActionResult Index()
        {
            //if (HttpContext.Session.IsAvailable)
            //{
            //    return ;
            //}

            //if (HttpContext.Session.GetString("roles") != null)
            //{
            //    return ;
            //}
            return View();
        }

        public IActionResult Validate(LoginViewModel loginViewModel)
        {
            UserViewModel account = null;
            string stringData = JsonConvert.SerializeObject(loginViewModel);
            var contentData = new StringContent(stringData, System.Text.Encoding.UTF8, "application/json");

            var resTask = client.PostAsync("Users/Login", contentData);

            var result = resTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var data = result.Content.ReadAsStringAsync().Result;
                var json = JsonConvert.DeserializeObject(data).ToString();
                account = JsonConvert.DeserializeObject<UserViewModel>(json);

                var stringRoles = String.Join(",", account.RoleName.ToArray());
                var isVerified = account.EmailConfirmed.ToString().ToLower();

                HttpContext.Session.SetString("id", account.Id);
                HttpContext.Session.SetString("uname", account.UserName);
                HttpContext.Session.SetString("email", account.Email);
                HttpContext.Session.SetString("roles", stringRoles);
                HttpContext.Session.SetString("verified", isVerified);
            }
            var response = Tuple.Create(account, result);
            return Json(response, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [HttpGet]
        //[Route("edit")]
        // GET: Edit
        public ActionResult Edit()
        {
            return View("Edit");
        }

        public ActionResult GetEdit()
        {
            var id = HttpContext.Session.GetString("id");
            UserEditViewModel userEditViewModel = null;
            
            var resTask = client.GetAsync("Users/Edit/" + id);
            resTask.Wait();

            var result = resTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<UserEditViewModel>();
                readTask.Wait();

                userEditViewModel = readTask.Result;
            }
            var response = Tuple.Create(userEditViewModel, result);

            return Json(response, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [HttpPost]
        public IActionResult Edit(UserEditViewModel userEditViewModel)
        {
            var id = HttpContext.Session.GetString("id");

            string stringData = JsonConvert.SerializeObject(userEditViewModel);
            var contentData = new StringContent(stringData, System.Text.Encoding.UTF8, "application/json");
            var resTask = client.PutAsync("Users/Edit/"+ id, contentData);

            var result = resTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var data = result.Content.ReadAsStringAsync().Result;
                var json = JsonConvert.DeserializeObject(data).ToString();
                var account = JsonConvert.DeserializeObject<UserViewModel>(json);

                //var stringRoles = String.Join(",", account.RoleName.ToArray());

                HttpContext.Session.SetString("uname", account.UserName);
                //HttpContext.Session.SetString("email", account.Email);
                //HttpContext.Session.SetString("role", stringRoles);

            }

            return Json(result, new Newtonsoft.Json.JsonSerializerSettings());
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/login");
        }
    }
}