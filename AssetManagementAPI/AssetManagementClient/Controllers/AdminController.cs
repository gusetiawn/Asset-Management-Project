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
        [Route("Menu/Admin/UserRegister")]
        public IActionResult UserRegister()
        {
            return View();
        }
        [Route("Menu/Admin/RequestItem/RequestList")]
        public IActionResult RequestItem()
        {
            return View();
        }
        [Route("Menu/Admin/RequestItem/TakeAsset")]
        public IActionResult TakeAsset()
        {
            return View();
        }
        [Route("Menu/Admin/RequestItem/ReturnAsset")]
        public IActionResult ReturnAsset()
        {
            return View();
        }
        [Route("Menu/Admin/Item")]
        public IActionResult Item()
        {
            return View();
        }
        [Route("Settings/Profile/Admin")]
        public IActionResult YourAccount()
        {
            return View();
        }

        [Route("Settings/Security/Admin")]
        public IActionResult Security()
        {
            return View();
        }

        [Route("Settings/Admin")]
        public IActionResult Settings()
        {
            return View();
        }
    }
}
