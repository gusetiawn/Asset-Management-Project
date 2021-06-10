using AssetManagementAPI.Base;
using AssetManagementAPI.Context;
using AssetManagementAPI.Handler;
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
        public UsersController(UserRepository userRepository, MyContext myContext) : base(userRepository)
        {
            this.userRepository = userRepository;
            this.myContext = myContext;
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
                    GenderId = registerVM.GenderId,
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
                    Password = Hashing.HashPassword(registerVM.Password)
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

        [HttpGet("GenderMale")]
        public ActionResult GetGenderMale()
        {
            var genderMale = from U in myContext.Users
                              join G in myContext.Genders on U.GenderId equals G.Id
                              where U.GenderId == 1
                              select new
                              {
                                  Gender = G.Name
                              };
            return Ok(genderMale);
        }

        [HttpGet("GenderFemale")]
        public ActionResult GetGenderFemale()
        {
            var genderFemale = from U in myContext.Users
                             join G in myContext.Genders on U.GenderId equals G.Id
                             where U.GenderId == 2
                             select new
                             {
                                 Gender = G.Name
                             };
            return Ok(genderFemale);
        }

        [HttpGet("DeptEngineering")]
        public ActionResult GetDeptEngineering()
        {
            var deptEngineering = from U in myContext.Users
                               join D in myContext.Departments on U.DepartmentId equals D.Id
                               where U.DepartmentId == 1
                               select new
                               {
                                   Department = D.Name
                               };
            return Ok(deptEngineering);
        }

        [HttpGet("DeptHR")]
        public ActionResult GetDeptHR()
        {
            var deptHR = from U in myContext.Users
                                  join D in myContext.Departments on U.DepartmentId equals D.Id
                                  where U.DepartmentId == 2
                                  select new
                                  {
                                      Department = D.Name
                                  };
            return Ok(deptHR);
        }

        [HttpGet("DeptFinance")]
        public ActionResult GetDeptFinance()
        {
            var deptFinance = from U in myContext.Users
                                  join D in myContext.Departments on U.DepartmentId equals D.Id
                                  where U.DepartmentId == 3
                                  select new
                                  {
                                      Department = D.Name
                                  };
            return Ok(deptFinance);
        }

        [HttpGet("DeptAdmin")]
        public ActionResult GetDeptAdmin()
        {
            var deptAdmin = from U in myContext.Users
                                  join D in myContext.Departments on U.DepartmentId equals D.Id
                                  where U.DepartmentId == 4
                                  select new
                                  {
                                      Department = D.Name
                                  };
            return Ok(deptAdmin);
        }
    }
}
