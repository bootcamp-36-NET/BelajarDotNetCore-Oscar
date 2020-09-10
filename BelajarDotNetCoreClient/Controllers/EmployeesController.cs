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
    public class EmployeesController : Controller
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
            var stringRole = HttpContext.Session.GetString("roles");
            var roles = stringRole.Split(',').ToList();
            var isValid = roles.Where(Q => Q == "ADMIN").Any();
            if (!isValid)
            {
                return Redirect("/Userprofile");
            }
            return View();
        }

        public IActionResult LoadEmployee()
        {
            IEnumerable<Employee> employees = null;

            var authToken = HttpContext.Session.GetString("JWToken");
            client.DefaultRequestHeaders.Add("Authorization", authToken);

            var resTask = client.GetAsync("Employees");
            resTask.Wait();

            var result = resTask.Result;

            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<IList<Employee>>();
                readTask.Wait();
                employees = readTask.Result;
            }

            return Json(employees, new Newtonsoft.Json.JsonSerializerSettings());
        }

        public IActionResult GetById(string id)
        {
            Employee employees = null;

            var authToken = HttpContext.Session.GetString("JWToken");
            client.DefaultRequestHeaders.Add("Authorization", authToken);

            var resTask = client.GetAsync("Employees/" + id);
            resTask.Wait();

            var result = resTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<Employee>();
                readTask.Wait();

                employees = readTask.Result;
            }

            return Json(employees, new Newtonsoft.Json.JsonSerializerSettings());
        }

        public JsonResult Delete(string id)
        {
            var authToken = HttpContext.Session.GetString("JWToken");
            client.DefaultRequestHeaders.Add("Authorization", authToken);

            var resTask = client.DeleteAsync("Employees/" + id);
            resTask.Wait();
            var response = resTask.Result;

            return Json(response, new Newtonsoft.Json.JsonSerializerSettings());
        }
    }
}