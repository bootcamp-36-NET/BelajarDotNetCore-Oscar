using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BelajarDotNetCoreAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OfficeOpenXml;

namespace BelajarDotNetCoreClient.Controllers
{
    public class DivisionsController : Controller
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
                return Redirect("/");
            }
            return View();
        }

        public JsonResult LoadAllData()
        {
            IEnumerable<Division> divisions = null;

            var authToken = HttpContext.Session.GetString("JWToken");
            client.DefaultRequestHeaders.Add("Authorization", authToken);

            var resTask = client.GetAsync("Divisions");
            resTask.Wait();

            var result = resTask.Result;

            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<IList<Division>>();
                readTask.Wait();
                divisions = readTask.Result;
            }

            return Json(divisions, new Newtonsoft.Json.JsonSerializerSettings());
        }

        public JsonResult GetById(int id)
        {
            Division division = null;

            var authToken = HttpContext.Session.GetString("JWToken");
            client.DefaultRequestHeaders.Add("Authorization", authToken);

            var resTask = client.GetAsync("Divisions/" + id);
            resTask.Wait();

            var result = resTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<Division>();
                readTask.Wait();

                division = readTask.Result;
            }

            return Json(division, new Newtonsoft.Json.JsonSerializerSettings());
        }

        public JsonResult AddOrUpdate(Division division)
        {
            Task<HttpResponseMessage> resTask;
            string stringData = JsonConvert.SerializeObject(division);
            var contentData = new StringContent(stringData, System.Text.Encoding.UTF8, "application/json");

            var authToken = HttpContext.Session.GetString("JWToken");
            client.DefaultRequestHeaders.Add("Authorization", authToken);

            if (division.Id == 0)
            {
                resTask = client.PostAsync("Divisions", contentData);
            }
            else
            {
                resTask = client.PutAsync("Divisions/" + division.Id, contentData);
            }
            resTask.Wait();
            var response = resTask.Result;

            return Json(response, new Newtonsoft.Json.JsonSerializerSettings());
        }

        public JsonResult Delete(int id)
        {
            var authToken = HttpContext.Session.GetString("JWToken");
            client.DefaultRequestHeaders.Add("Authorization", authToken);

            var resTask = client.DeleteAsync("Divisions/" + id);
            resTask.Wait();
            var response = resTask.Result;

            return Json(response, new Newtonsoft.Json.JsonSerializerSettings());
        }

        public ActionResult PrintPdf()
        {
            var response = "";
            var authToken = HttpContext.Session.GetString("JWToken");
            client.DefaultRequestHeaders.Add("Authorization", authToken);

            var resTask = client.GetAsync("Divisions/PrintPdf");
            resTask.Wait();
            var result = resTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var content = result.Content.ReadAsStringAsync();
                resTask.Wait();

                response = content.Result;
            }

            return Json(response, new Newtonsoft.Json.JsonSerializerSettings());
        }


        public IActionResult PrintExcel()
        {
            var comlumHeadrs = new string[]
            {
                "No.",
                "Department Name",
                "Division Name",
                "Created At"
            };

            byte[] response;

            using (var package = new ExcelPackage())
            {
                // add a new worksheet to the empty workbook

                var worksheet = package.Workbook.Worksheets.Add("Divisions"); //Worksheet name
                using (var cells = worksheet.Cells[1, 1, 1, 7]) //(1,1) (1,5)
                {
                    cells.Style.Font.Bold = true;
                }

                //First add the headers
                for (var i = 0; i < comlumHeadrs.Count(); i++)
                {
                    worksheet.Cells[1, i + 1].Value = comlumHeadrs[i];
                }

                //Add values
                var j = 2;

                IEnumerable<Division> divisions = null;

                var authToken = HttpContext.Session.GetString("JWToken");
                client.DefaultRequestHeaders.Add("Authorization", authToken);

                var resTask = client.GetAsync("Divisions");
                resTask.Wait();

                var result = resTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<Division>>();
                    readTask.Wait();
                    divisions = readTask.Result;
                }

                foreach (var division in divisions)
                {
                    worksheet.Cells["A" + j].Value = j - 1;
                    worksheet.Cells["B" + j].Value = division.Name;
                    worksheet.Cells["C" + j].Value = division.Department.Name;
                    worksheet.Cells["D" + j].Value = division.CreateDate.ToString("dd-MMMM-yyyy HH:mm");
                    j++;
                }
                response = package.GetAsByteArray();
            }

            return File(response, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Division.xlsx");
        }
    }
}