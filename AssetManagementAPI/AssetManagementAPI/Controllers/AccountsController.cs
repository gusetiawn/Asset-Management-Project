using AssetManagementAPI.Base;
using AssetManagementAPI.Context;
using AssetManagementAPI.Handler;
using AssetManagementAPI.Models;
using AssetManagementAPI.Repositories.Data;
using AssetManagementAPI.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
                    return StatusCode(200, new { Status = "200", message = "Login Berhasil!" });

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
                return StatusCode(200, new { Status = "200", message = "Change Password Successful!!" });
            }
            else
            {
                return StatusCode(404, new { Status = "404", message = "Maaf Akun Tidak Sesuai" });
            }

        }
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
                return StatusCode(result, new { Status = "200", message = "Request Password baru anda berhasil, password baru anda:"+resetPass });
            }
            else
            {
                return StatusCode(404, new { Status = "404", message = "Email Tersebut Tidak Terdaftar" });
            }

        }
    }
}
