using AssetManagementAPI.Models;
using AssetManagementClient.Base;
using AssetManagementClient.Repository.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetManagementClient.Controllers
{
    public class AccountsController : BaseController<Account, AccountRepository, string>
    {
        private readonly AccountRepository repository;

        public AccountsController(AccountRepository repository) : base(repository)
        {
            this.repository = repository;
        }

        public IActionResult Index()
        {
            return View();
        }

        //public async Task<JsonResult> GetUserData()
        //{
        //    var result = await repository.GetUserData();
        //    return Json(result);
        //}
    }
}
