using AssetManagementAPI.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagementClient.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Route("Login")]
        public IActionResult Login()
        {
            return View();
        }

        [Route("ForgotPassword")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public HttpStatusCode Register(RegisterVM registerVM)
        {
            var httpClient = new HttpClient();
            StringContent content = new StringContent(JsonConvert.SerializeObject(registerVM), Encoding.UTF8, "application/json");
            var result = httpClient.PostAsync("https://localhost:44395/API/Account/Register", content).Result;
            return result.StatusCode;
        }

        [HttpPost]
        public HttpStatusCode ForgotPassword(ForgotPasswordVM forgotPasswordVM)
        {
            var httpClient = new HttpClient();
            StringContent content = new StringContent(JsonConvert.SerializeObject(forgotPasswordVM), Encoding.UTF8, "application/json");
            var result = httpClient.PostAsync("https://localhost:44395/API/Accounts/ForgotPassword", content).Result;
            return result.StatusCode;
        }

        [HttpPost]
        public string Login(LoginVM loginVM)
        {
            var httpClient = new HttpClient();
            StringContent content = new StringContent(JsonConvert.SerializeObject(loginVM), Encoding.UTF8, "application/json");
            var result = httpClient.PostAsync("https://localhost:44395/API/Accounts/Login", content).Result;

            
            var token = result.Content.ReadAsStringAsync().Result;
            HttpContext.Session.SetString("token", token);

            if (result.IsSuccessStatusCode)
            {
                var jwtReader = new JwtSecurityTokenHandler();
                var jwt = jwtReader.ReadJwtToken(token);
                var id = jwt.Claims.First(c => c.Type == "Id").Value;
                HttpContext.Session.SetString("id", id);
                ViewData["id"] = HttpContext.Session.GetString("id");
                var firstName = jwt.Claims.First(c => c.Type == "FirstName").Value;
                HttpContext.Session.SetString("firstName", firstName);
                ViewData["firstName"] = HttpContext.Session.GetString("firstName");
                var lastName = jwt.Claims.First(c => c.Type == "LastName").Value;
                HttpContext.Session.SetString("lastName", lastName);
                ViewData["lastName"] = HttpContext.Session.GetString("lastName");
                var role = jwt.Claims.First(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role").Value;
                if (role == "Manager")
                {
                    return Url.Action("Manager", "Dashboard");
                }
                else if (role == "Admin")
                {
                    return Url.Action("Admin", "Dashboard");
                }
                else if (role == "Employee")
                {
                    return Url.Action("Employee", "Dashboard");
                }
                else
                {
                    return Url.Action("Index", "Home");
                }
            }
            else
            {
                return Url.Action("Error", "Home");
            }
        }
    }

}
