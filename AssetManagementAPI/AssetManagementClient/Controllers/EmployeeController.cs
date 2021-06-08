using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
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
            var token = HttpContext.Session.GetString("token");
            ViewData["token"] = token;

            var jwtReader = new JwtSecurityTokenHandler();
            var jwt = jwtReader.ReadJwtToken(token);
            var id = jwt.Claims.First(c => c.Type == "Id").Value;
            ViewData["id"] = id;

            return View();
        }

        [Route("Menu/RequestItem/ListRequestItems")]
        public IActionResult ListRequestItems()
        {
            var token = HttpContext.Session.GetString("token");
            ViewData["token"]= token;

            var jwtReader = new JwtSecurityTokenHandler();
            var jwt = jwtReader.ReadJwtToken(token);
            var id = jwt.Claims.First(c => c.Type == "Id").Value;
            ViewData["id"] = id;


            return View();
        }

        [Route("Settings/Profile/Employee")]
        public IActionResult YourAccount()
        {
            var token = HttpContext.Session.GetString("token");
            ViewData["token"] = token;

            var jwtReader = new JwtSecurityTokenHandler();
            var jwt = jwtReader.ReadJwtToken(token);
            var id = jwt.Claims.First(c => c.Type == "Id").Value;
            ViewData["id"] = id;
   
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
