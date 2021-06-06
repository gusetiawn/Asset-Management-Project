using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetManagementClient.Controllers
{
    public class ManagerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Route("Menu/RequestList")]
        public IActionResult RequestList()
        {
            return View();
        }

        [Route("Settings/Profile/Manager")]
        public IActionResult YourAccount()
        {
            return View();
        }

        [Route("Settings/Security/Manager")]
        public IActionResult Security()
        {
            return View();
        }

        [Route("Settings/Manager")]
        public IActionResult Settings()
        {
            return View();
        }
    }
}
