using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BelajarDotNetCoreAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BelajarDotNetCoreClient.Controllers
{
    public class UserProfileController : Controller
    {
        readonly HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:44321/api/")
        };

        public IActionResult Index()
        {
            if (!HttpContext.Session.IsAvailable)
            {
                return Redirect("/login");
            }
            if (HttpContext.Session.GetString("verified") == "false")
            {
                return Redirect("/verify");
            }
            if (HttpContext.Session.GetString("roles") == null)
            {
                return Redirect("/login");
            }

            return View();
        }

        public ActionResult GetUser()
        {
            var id = HttpContext.Session.GetString("id");
            UserViewModel userViewModel = null;

            var authToken = HttpContext.Session.GetString("JWToken");
            client.DefaultRequestHeaders.Add("Authorization", authToken);

            var resTask = client.GetAsync("Users/Details/" + id);
            resTask.Wait();

            var result = resTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<UserViewModel>();
                readTask.Wait();

                userViewModel = readTask.Result;
            }
            var response = Tuple.Create(userViewModel, result);

            return Json(response, new Newtonsoft.Json.JsonSerializerSettings());
        }
    }
}