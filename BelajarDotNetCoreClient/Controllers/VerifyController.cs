using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BelajarDotNetCoreAPI.Context;
using BelajarDotNetCoreAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BelajarDotNetCoreClient.Controllers
{
    public class VerifyController : Controller
    {
        readonly HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:44321/api/")
        };

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Verify(string code)
        {
            var id = HttpContext.Session.GetString("id");

            var contentData = new StringContent(code, System.Text.Encoding.UTF8, "application/json");


            var authToken = HttpContext.Session.GetString("JWToken");
            client.DefaultRequestHeaders.Add("Authorization", authToken);

            var resTask = client.PostAsync("Users/Verify/" + id, contentData);
            resTask.Wait();

            var result = resTask.Result;
            if (result.IsSuccessStatusCode)
            {
                HttpContext.Session.SetString("verified", "true");
            }

            return Json(result, new Newtonsoft.Json.JsonSerializerSettings());
        }
    }
}