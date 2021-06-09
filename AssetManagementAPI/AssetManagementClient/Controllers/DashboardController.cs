using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
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
            var token = HttpContext.Session.GetString("token");
            ViewData["token"] = token;

            var jwtReader = new JwtSecurityTokenHandler();
            var jwt = jwtReader.ReadJwtToken(token);
            var id = jwt.Claims.First(c => c.Type == "Id").Value;
            var firstName = jwt.Claims.First(c => c.Type == "FirstName").Value;
            var lastName = jwt.Claims.First(c => c.Type == "LastName").Value;
            ViewData["id"] = id;
            ViewData["firstName"] = firstName;
            ViewData["lastName"] = lastName;
            return View();
        }

        [Route("Dashboard/Manager")]
        public IActionResult Manager()
        {
            var token = HttpContext.Session.GetString("token");
            ViewData["token"] = token;

            var jwtReader = new JwtSecurityTokenHandler();
            var jwt = jwtReader.ReadJwtToken(token);
            var id = jwt.Claims.First(c => c.Type == "Id").Value;
            var firstName = jwt.Claims.First(c => c.Type == "FirstName").Value;
            var lastName = jwt.Claims.First(c => c.Type == "LastName").Value;
            ViewData["id"] = id;
            ViewData["firstName"] = firstName;
            ViewData["lastName"] = lastName;
            return View();
        }

        [Route("Dashboard/Admin")]
        public IActionResult Admin()
        {
            var token = HttpContext.Session.GetString("token");
            ViewData["token"] = token;

            var jwtReader = new JwtSecurityTokenHandler();
            var jwt = jwtReader.ReadJwtToken(token);
            var id = jwt.Claims.First(c => c.Type == "Id").Value;
            var firstName = jwt.Claims.First(c => c.Type == "FirstName").Value;
            var lastName = jwt.Claims.First(c => c.Type == "LastName").Value;
            ViewData["id"] = id;
            ViewData["firstName"] = firstName;
            ViewData["lastName"] = lastName;
            return View();
        }
    }
}
