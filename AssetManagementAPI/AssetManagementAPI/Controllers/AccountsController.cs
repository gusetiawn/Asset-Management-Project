using AssetManagementAPI.Base;
using AssetManagementAPI.Context;
using AssetManagementAPI.Handler;
using AssetManagementAPI.Models;
using AssetManagementAPI.Repositories.Data;
using AssetManagementAPI.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : BaseController<Account, AccountRepository, string>
    {
        private readonly AccountRepository accountRepository;
        private readonly MyContext myContext;
        private readonly IConfiguration configuration;
        private readonly SendMail sendMail = new SendMail();
        public AccountsController(AccountRepository accountRepository, MyContext myContext, IConfiguration configuration) : base(accountRepository)
        {
            this.accountRepository = accountRepository;
            this.myContext = myContext;
            this.configuration = configuration;
        }
        [HttpPost("Login")]
        public ActionResult Login(LoginVM loginVM)
        {
            var user = myContext.Users.Where(u => u.Email == loginVM.Email).FirstOrDefault();
            if (user != null)
            {
                var account = myContext.Accounts.Where(a => a.Id == user.Id).FirstOrDefault();
                if (account != null && Hashing.ValidatePassword(loginVM.Password, account.Password))
                {
                    var roles = myContext.RoleAccounts.Where(ra => ra.AccountId == account.Id).ToList();

                    var claims = new List<Claim> {
                    new Claim("Email", user.Email),
                    new Claim("FirstName", user.FirstName),
                    new Claim("LastName", user.LastName),
                    new Claim("Id", user.Id)
                    };
                    foreach (var item in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, myContext.Roles.Where(r => r.Id == item.RoleId).FirstOrDefault().Name));
                    }
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));

                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(configuration["Jwt:Issuer"], configuration["Jwt:Audience"], claims, expires: DateTime.UtcNow.AddDays(1), signingCredentials: signIn);

                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));

                }
                else
                {
                    return StatusCode(404, new { Status = "404", message = "Email atau Password Anda Salah!" });
                }
            }
            else
            {
                return StatusCode(404, new { Status = "404", message = "Email Tidak Terdaftar!" });
            }


        }
        //[Authorize]
        [HttpPut("ChangePassword")]
        public ActionResult ChangePassword(ChangePasswordVM changePasswordVM)
        {
            var user = myContext.Users.Where(u => u.Email == changePasswordVM.Email).FirstOrDefault();
            var account = myContext.Accounts.Where(a => a.Id == user.Id).FirstOrDefault();
            if (user != null && Hashing.ValidatePassword(changePasswordVM.OldPassword, account.Password))
            {
                account.Password = Hashing.HashPassword(changePasswordVM.NewPassword);
                myContext.Entry(account).State = EntityState.Modified;
                var result = myContext.SaveChanges();

                var subject = "Change Password";
                string mailbody = string.Empty;
                mailbody += "<body>";
                mailbody += "<div style='margin: 25px;'>";
                mailbody += "<div style='background-color: #4ac09d; border-radius: 25px;'>";
                mailbody += "<p style='font-weight: bold; font-size: 25px; padding-top: 25px; color: white; text-align: center;'>Asset Management System</p>";
                mailbody += "<div style='background-color:#f0f0f0; border-bottom-right-radius:25px; border-bottom-left-radius:25px; margin-top:-30px;'>";
                mailbody += "<div style='margin-left: 50px; margin-right: 50px; padding-bottom: 50px;'>";
                mailbody += "<h4 style='padding-top: 20px; color: black;'>Hello " + user.FirstName + " ,</h4>";
                mailbody += "<p style='color: black;'>You have just changed the password to your account,</p>";
                mailbody += "<p style='color: black;'>If you feel you have not done so, immediately make changes by forgetting the password on your account</p>";
                mailbody += "<br>";
                mailbody += "<p style='color: black;'>Best Regards,</p>";
                mailbody += "<p style='color: black;'>Management System</p>";
                mailbody += "</div>";

                mailbody += "<div style='color: inherit; font - size:inherit; line - height:inherit'>";
                mailbody += "<table width='100%' cellpadding='0' cellspacing='0' style='border-spacing:0!important;border-collapse:collapse;text-align:center;font-family:Arial,sans-serif;font-size:12px;line-height:135%;color:#23496d;margin-bottom:0;padding:0' align='center'>";
                mailbody += "<tbody>";
                mailbody += "<tr>";
                mailbody += "<td align='center' valign='top' style='border-collapse:collapse;font-family:Lato,Tahoma,sans-serif;font-size:13px;color:#444444;word-break:break-word;text-align:center;margin-bottom:0;line-height:135%;padding:10px 20px'>";
                mailbody += "<p style='font-family:Verdana,sans-serif;font-size:11px;font-weight:normal;text-decoration:none;font-style:normal;color:#444444'> APL Tower Lantai 37, Jl. Letjen S. Parman Kav. 28, RT.12/RW.6, Tj. Duren Sel., Jakarta Barat, Kota Jakarta Barat, Daerah Khusus Ibukota Jakarta 11470 </p>";
                mailbody += "<font color='#888888'>";
                mailbody += "<p>";
                mailbody += "<a style='font-family:Verdana,sans-serif;font-size:10px;color:#00000f;font-weight:normal;font-style:normal' rel='noreferrer'>© PT. Mitra Integrasi Informatika</a>";
                mailbody += "</p>";
                mailbody += "</font>";
                mailbody += "</td>";
                mailbody += "</tr>";
                mailbody += "</tbody>";
                mailbody += "</table>";
                mailbody += "<div>";
                mailbody += "<div>";
                mailbody += "<div>";

                mailbody += "</div>";
                mailbody += "</div>";
                mailbody += "</div>";
                mailbody += "</div>";
                mailbody += "</body>";
                var body = mailbody;
                sendMail.SendEmail(changePasswordVM.Email, body, subject);
                return StatusCode(200, new { Status = "200", message = "Perubahan Password Berhasil" });
            }
            else
            {
                return StatusCode(404, new { Status = "404", message = "Maaf Akun Tidak Sesuai" });
            }

        }
        //[Authorize]
        [HttpPost("ForgotPassword")]
        public ActionResult ForgotPassword(ForgotPasswordVM forgotPasswordVM)
        {
            var resetPass = Guid.NewGuid().ToString();
            //var check = myContext.Users.Include(u => u.Account).Where(user => user.Email == forgotPasswordVM.Email).FirstOrDefault();
            //Account account = myContext.Accounts.Where(accountRepository => accountRepository.Id == check.Id).FirstOrDefault();
            User user = myContext.Users.Where(user => user.Email == forgotPasswordVM.Email).FirstOrDefault();
            if (user != null)
            {
                Account account = myContext.Accounts.Find(user.Id);
                account.Password = Hashing.HashPassword(resetPass);
                myContext.Entry(account).State = EntityState.Modified;
                myContext.SaveChanges();

                var subject = "Request Reset Password";
                string mailbody = string.Empty;
                mailbody += "<body>";
                mailbody += "<div style='margin: 25px;'>";
                mailbody += "<div style='background-color: #4ac09d; border-radius: 25px;'>";
                mailbody += "<p style='font-weight: bold; font-size: 25px; padding-top: 25px; color: white; text-align: center;'>Asset Management System</p>";
                mailbody += "<div style='background-color:#f0f0f0; border-bottom-right-radius:25px; border-bottom-left-radius:25px; margin-top:-30px;'>";
                mailbody += "<div style='margin-left: 50px; margin-right: 50px; padding-bottom: 50px;'>";
                mailbody += "<h4 style='padding-top: 20px; color: black;'>Hello " + user.FirstName + " ,</h4>";
                mailbody += "<p style='color: black;'>We have accepted your request to reset password. Here is your new password.</p>";
                mailbody += "<h4 style='text-align: center; color: black;'>Use that new password to login to your account</h4>";
                mailbody += "<div style='background-color: white;'>";
                mailbody += "<p style='text-align: center; padding: 10px;  color: black;'> " + resetPass + " </p>";
                mailbody += "</div>";
                mailbody += "<br>";
                mailbody += "<p style='color: black;'>Best Regards,</p>";
                mailbody += "<p style='color: black;'>Management System</p>";
                mailbody += "</div>";

                mailbody += "<div style='color: inherit; font - size:inherit; line - height:inherit'>";
                mailbody += "<table width='100%' cellpadding='0' cellspacing='0' style='border-spacing:0!important;border-collapse:collapse;text-align:center;font-family:Arial,sans-serif;font-size:12px;line-height:135%;color:#23496d;margin-bottom:0;padding:0' align='center'>";
                mailbody += "<tbody>";
                mailbody += "<tr>";
                mailbody += "<td align='center' valign='top' style='border-collapse:collapse;font-family:Lato,Tahoma,sans-serif;font-size:13px;color:#444444;word-break:break-word;text-align:center;margin-bottom:0;line-height:135%;padding:10px 20px'>";
                mailbody += "<p style='font-family:Verdana,sans-serif;font-size:11px;font-weight:normal;text-decoration:none;font-style:normal;color:#444444'> APL Tower Lantai 37, Jl. Letjen S. Parman Kav. 28, RT.12/RW.6, Tj. Duren Sel., Jakarta Barat, Kota Jakarta Barat, Daerah Khusus Ibukota Jakarta 11470 </p>";
                mailbody += "<font color='#888888'>";
                mailbody += "<p>";
                mailbody += "<a style='font-family:Verdana,sans-serif;font-size:10px;color:#00000f;font-weight:normal;font-style:normal' rel='noreferrer'>© PT. Mitra Integrasi Informatika</a>";
                mailbody += "</p>";
                mailbody += "</font>";
                mailbody += "</td>";
                mailbody += "</tr>";
                mailbody += "</tbody>";
                mailbody += "</table>";
                mailbody += "<div>";
                mailbody += "<div>";
                mailbody += "<div>";

                mailbody += "</div>";
                mailbody += "</div>";
                mailbody += "</div>";
                mailbody += "</div>";
                mailbody += "</body>";
                var body = mailbody;
                sendMail.SendEmail(forgotPasswordVM.Email, body, subject);
                return StatusCode(200, new { Status = "200", message = "Request Password baru anda berhasil, Silahkan Cek Email Anda" });
            }
            else
            {
                return StatusCode(404, new { Status = "404", message = "Email Not Found" });
            }

        }

        [HttpGet("UserData")]
        public ActionResult UserData()
        {
            var userData = from u in myContext.Users
                           join d in myContext.Departments on u.DepartmentId equals d.Id
                           join g in myContext.Genders on u.GenderId equals g.Id
                           join a in myContext.Accounts on u.Id equals a.Id
                           join ra in myContext.RoleAccounts on a.Id equals ra.AccountId
                           join r in myContext.Roles on ra.RoleId equals r.Id
                           where u.IsDeleted == 0
                           select new
                           {
                               Id = u.Id,
                               FirstName = u.FirstName,
                               LastName = u.LastName,
                               Name = u.FirstName +" "+ u.LastName,
                               GenderId = u.GenderId,
                               Gender = g.Name,
                               BirthDate = u.BirthDate,
                               Address = u.Address,
                               Contact = u.Contact,
                               Email = u.Email,
                               DepartmentId = u.DepartmentId,
                               Department = d.Name,
                               RoleId = ra.RoleId,
                               Role = r.Name
                           };
            return Ok(userData);
        }

        [HttpGet("Profile/{Id}")]
        public ActionResult GetProfileById(string Id)
        {
            var user = myContext.Users.Find(Id);
            if (user != null)
            {
                var profile = from u in myContext.Users
                               join d in myContext.Departments on u.DepartmentId equals d.Id
                               join g in myContext.Genders on u.GenderId equals g.Id
                               join a in myContext.Accounts on u.Id equals a.Id
                               join ra in myContext.RoleAccounts on a.Id equals ra.AccountId
                               join r in myContext.Roles on ra.RoleId equals r.Id
                               where u.Id == Id
                               select new
                               {
                                   Id = u.Id,
                                   FirstName = u.FirstName,
                                   LastName = u.LastName,
                                   Gender = g.Name,
                                   GenderId = u.GenderId,
                                   BirthDate = u.BirthDate,
                                   Address = u.Address,
                                   Contact = u.Contact,
                                   Email = u.Email,
                                   Department = d.Name,
                                   DepartmentId = u.DepartmentId,
                                   Role = r.Name
                               };
                return Ok(profile);
            }
            else
            {
                return NotFound("Id Not Registered");
            }
        }
    }
}
