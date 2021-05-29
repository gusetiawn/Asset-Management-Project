using AssetManagementAPI.Base;
using AssetManagementAPI.Context;
using AssetManagementAPI.Models;
using AssetManagementAPI.Repositories.Data;
using AssetManagementAPI.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : BaseController<User, UserRepository, string>
    {
        private readonly UserRepository userRepository;
        private readonly MyContext myContext;
        private readonly IConfiguration configuration;
        public UsersController(UserRepository userRepository, MyContext myContext, IConfiguration configuration) : base(userRepository)
        {
            this.userRepository = userRepository;
            this.myContext = myContext;
            this.configuration = configuration;
        }
        [HttpPost("Register")]
        public ActionResult Register(RegisterVM registerVM)
        {
            try
            {
                var user = new User()
                {
                    Id = registerVM.Id,
                    FirstName = registerVM.FirstName,
                    LastName = registerVM.LastName,
                    Gender = registerVM.Gender,
                    BirthDate = registerVM.BirthDate,
                    Address = registerVM.Address,
                    Contact = registerVM.Contact,
                    Email = registerVM.Email,
                    DepartmentId = registerVM.DepartmentId
                };
                myContext.Users.Add(user);
                myContext.SaveChanges();

                var account = new Account()
                {
                    Id = registerVM.Id,
                    Password = registerVM.Password
                };
                myContext.Accounts.Add(account);
                myContext.SaveChanges();
                
                var roleAccount = new RoleAccount()
                {
                    AccountId = registerVM.Id,
                    RoleId = registerVM.RoleId
                };
                myContext.RoleAccounts.Add(roleAccount);
                myContext.SaveChanges();

                return StatusCode(200, new { Status = "200", message = "Registrasi Berhasil!" });
            }
            catch (Exception)
            {
                return StatusCode(400, new { Status = "400", message = "Id Sudah Digunakan!!!" });
            }


        }
    }
}
