using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetManagementClient.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [Route("Menu/Admin/RequestItem")]
        public IActionResult RequestItem()
        {
            return View();
        }
        [Route("Menu/Admin/Item")]
        public IActionResult Item()
        {
            return View();
        }
        [Route("Menu/Admin/UserRegister")]
        public IActionResult UserRegister()
        {
            return View();
        }
    }
}
