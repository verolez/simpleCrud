using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebMvc.Models;

namespace WebMvc.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _FetchStudents()
        {
            List<Students> students = new List<Students>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(System.Configuration.ConfigurationManager.AppSettings["baseAddress"]);

                var responseTask = client.GetAsync("student");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<List<Students>>();
                    readTask.Wait();

                    students = readTask.Result;


                }
            }
            return PartialView(students);
        }

        [HttpPost]
        public JsonResult SaveStudent([Bind(Include = "ID, Name, Email, Address")]Students student)
        {
            using(var client = new HttpClient())
            {
                client.BaseAddress = new Uri(System.Configuration.ConfigurationManager.AppSettings["baseAddress"]);

                var postTask = client.PostAsJsonAsync<Students>("student/SaveStudent", student);
                postTask.Wait();

                var result = postTask.Result;
                if(result.IsSuccessStatusCode)
                {
                    return Json(new { message = "Student Saved." });
                }
            }
           
            return Json(new { message = "Something went wrong, please try again." });
            
        }

        public JsonResult DeleteStudent(int id)
        {
            string message = "";
            using(var client = new HttpClient())
            {
                client.BaseAddress = new Uri(System.Configuration.ConfigurationManager.AppSettings["baseAddress"]);

                var delete = client.DeleteAsync("student/DeleteStudent/" + id);
                delete.Wait();

                var result = delete.Result;
                if(result.IsSuccessStatusCode)
                {
                    message = "student removed.";
                }
            }
            return Json(new { message = message });
        }
    }
}