using AccessoriesWebsite.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AccessoriesWebsite.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }


    }
}
