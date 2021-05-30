using AssetManagementAPI.Context;
using AssetManagementAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetManagementAPI.Repositories.Data
{
    public class RoleAccountRepository : GeneralRepository<MyContext, RoleAccount, string>
    {
        public RoleAccountRepository(MyContext myContext) : base(myContext)
        {

        }
    }
}
