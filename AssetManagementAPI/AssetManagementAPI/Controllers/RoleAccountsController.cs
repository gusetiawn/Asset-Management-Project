using AssetManagementAPI.Base;
using AssetManagementAPI.Models;
using AssetManagementAPI.Repositories.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleAccountsController : BaseController<RoleAccount, RoleAccountRepository, string>
    {
        public RoleAccountsController(RoleAccountRepository roleAccountRepository) : base(roleAccountRepository)
        {

        }
    }
}
