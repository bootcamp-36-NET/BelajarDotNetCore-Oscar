using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BelajarDotNetCoreClient.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using BelajarDotNetCoreAPI.ViewModel.ChartViewModel;
using System.Net.Http;

namespace BelajarDotNetCoreClient.Controllers
{
    public class HomeController : Controller
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
            foreach (var role in roles)
            {
                if (role == "ADMIN" || role == "SALES")
                {
                    return View();
                }
            }
            return Redirect("/login");

        }



        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult LoadPie()
        {
            IEnumerable<PieChartVM> pie = null;
            //var authToken = HttpContext.Session.GetString("JWToken");
            //client.DefaultRequestHeaders.Add("Authorization", authToken);
            var resTask = client.GetAsync("charts/pie");
            resTask.Wait();

            var result = resTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<List<PieChartVM>>();
                readTask.Wait();
                pie = readTask.Result;
            }
            else
            {
                pie = Enumerable.Empty<PieChartVM>();
                ModelState.AddModelError(string.Empty, "Server Error try after sometimes.");
            }
            return Json(pie);
        }

        public IActionResult LoadBar()
        {
            IEnumerable<PieChartVM> bar = null;
            //var authToken = HttpContext.Session.GetString("JWToken");
            //client.DefaultRequestHeaders.Add("Authorization", authToken);
            var resTask = client.GetAsync("charts/pie");
            resTask.Wait();

            var result = resTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<List<PieChartVM>>();
                readTask.Wait();
                bar = readTask.Result;
            }
            else
            {
                bar = Enumerable.Empty<PieChartVM>();
                ModelState.AddModelError(string.Empty, "Server Error try after sometimes.");
            }
            return Json(bar);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
