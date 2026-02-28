using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;

namespace SchoolManagementSystem2.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // record current visit time in session
            HttpContext.Session.SetString("LastVisit", DateTime.Now.ToString("g"));
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}