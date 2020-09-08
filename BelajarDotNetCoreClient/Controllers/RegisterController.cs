using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BelajarDotNetCoreAPI.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BelajarDotNetCoreClient.Controllers
{
    public class RegisterController : Controller
    {
        readonly HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:44321/api/")
        };

        public IActionResult Index()
        {
            return View();
        }

        public JsonResult Register(RegisterViewModel registerModel)
        {
            Task<HttpResponseMessage> resTask;
            string stringData = JsonConvert.SerializeObject(registerModel);
            var contentData = new StringContent(stringData, System.Text.Encoding.UTF8, "application/json");

            resTask = client.PostAsync("Users", contentData);
            resTask.Wait();
            var result = resTask.Result;

            var errorMessage = result.Content.ReadAsStringAsync().Result;

            var response = Tuple.Create(errorMessage, result);
            return Json(response, new Newtonsoft.Json.JsonSerializerSettings());
        }
    }
}