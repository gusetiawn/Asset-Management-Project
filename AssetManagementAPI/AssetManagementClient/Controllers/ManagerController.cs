using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
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

        [Route("Menu/RequestItem/NeedsApproval")]
        public IActionResult NeedsApproval()
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

        [Route("Menu/RequestItem/RequestHistory")]
        public IActionResult RequestHistory()
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

        [Route("Settings/Profile/Manager")]
        public IActionResult YourAccount()
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

        [Route("Settings/Security/Manager")]
        public IActionResult Security()
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

        [Route("Settings/Manager")]
        public IActionResult Settings()
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
