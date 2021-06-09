using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
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
        [Route("Menu/Admin/RequestItem/RequestList")]
        public IActionResult RequestItem()
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
        [Route("Menu/Admin/RequestItem/TakeAsset")]
        public IActionResult TakeAsset()
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
        [Route("Menu/Admin/RequestItem/ReturnAsset")]
        public IActionResult ReturnAsset()
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
        [Route("Menu/Admin/Item")]
        public IActionResult Item()
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
        [Route("Settings/Profile/Admin")]
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

        [Route("Settings/Security/Admin")]
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

        [Route("Settings/Admin")]
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
