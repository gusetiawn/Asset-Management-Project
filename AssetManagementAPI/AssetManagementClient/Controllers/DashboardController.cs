using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetManagementClient.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
 
        [Route("Dashboard/Employee")]
        public IActionResult Employee()
        {
            return View();
        }

        [Route("Dashboard/Manager")]
        public IActionResult Manager()
        {
            return View();
        }

        [Route("Dashboard/Admin")]
        public IActionResult Admin()
        {
            return View();
        }

        [Route("Dashboard/Tes")]
        public IActionResult Tes()
        {
            return View();
        }
    }
}
