using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetManagementClient.Controllers
{
    public class EmployeeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Route("Menu/RequestItem/NewRequest")]
        public IActionResult RequestNewItem()
        {
            return View();
        }

        [Route("Menu/RequestItem/ListRequestItems")]
        public IActionResult ListRequestItems()
        {
            return View();
        }

        [Route("Settings/Profile/Employee")]
        public IActionResult YourAccount()
        {
            return View();
        }

        [Route("Settings/Security/Employee")]
        public IActionResult Security()
        {
            return View();
        }

        [Route("Settings/Employee")]
        public IActionResult Settings()
        {
            return View();
        }
    }
}
