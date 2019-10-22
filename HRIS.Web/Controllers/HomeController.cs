using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HRIS.Web.Models;
using Microsoft.Extensions.Configuration;
using CookieManager;

namespace HRIS.Web.Controllers
{
    public class HomeController : GlobalAccessMenuController
    {

        public HomeController(IConfiguration config, ICookie cookie) : base(config, cookie)
        {

        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
