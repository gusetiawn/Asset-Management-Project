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
                    new Claim("Name", user.FirstName),
                    new Claim("Name", user.LastName)
                    };
                    foreach (var item in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, myContext.Roles.Where(r => r.Id == item.RoleId).FirstOrDefault().Name));
                    }
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));

                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(configuration["Jwt:Issuer"], configuration["Jwt:Audience"], claims, expires: DateTime.UtcNow.AddMinutes(5), signingCredentials: signIn);

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
        [Authorize]
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
                var body = $"Hai {user.FirstName},\nAnda baru saja melakukan perubahan password pada akun anda, Apabila Anda merasa tidak melakukannya maka segera melakukan perubahan dengan cara forgot password pada akun anda.\n Terima kasih dan Selamat Beraktifitas.";
                sendMail.SendEmail(changePasswordVM.Email, body, subject);
                return StatusCode(200, new { Status = "200", message = "Perubahan Password Berhasil" });
            }
            else
            {
                return StatusCode(404, new { Status = "404", message = "Maaf Akun Tidak Sesuai" });
            }

        }
        [Authorize]
        [HttpPost("ForgotPassword")]
        public ActionResult ForgotPassword(ForgotPasswordVM forgotPasswordVM)
        {
            var resetPass = Guid.NewGuid().ToString();
            var check = myContext.Users.Include(u => u.Account).Where(user => user.Email == forgotPasswordVM.Email).FirstOrDefault();
            Account account = myContext.Accounts.Where(accountRepository => accountRepository.Id == check.Id).FirstOrDefault();
            if (check != null)
            {
                account.Password = Hashing.HashPassword(resetPass);
                myContext.Entry(account).State = EntityState.Modified;
                var result = myContext.SaveChanges();

                var subject = "Request Reset Password";
                var body = $"Hai {check.FirstName},\nAnda baru saja melakukan  request lupa password, Gunakan password berikut untuk melakukan login, dan segera lakukan perubahan password.\nPassword Baru : {resetPass}\n Apabila Anda merasa tidak melakukannya maka segera melakukan perubahan dengan cara forgot password kembali pada akun anda.\n Terima kasih dan Selamat Beraktifitas.";
                sendMail.SendEmail(forgotPasswordVM.Email, body, subject);
                return StatusCode(result, new { Status = "200", message = "Request Password baru anda berhasil, Silahkan Cek Email Anda" });
            }
            else
            {
                return StatusCode(404, new { Status = "404", message = "Email Tersebut Tidak Terdaftar" });
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
