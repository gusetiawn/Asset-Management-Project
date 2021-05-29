using AssetManagementAPI.Context;
using AssetManagementAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetManagementAPI.Repositories.Data
{
    public class UserRepository : GeneralRepository<MyContext, User, string>
    {
        public UserRepository(MyContext myContext) : base(myContext)
        {

        }
    }
}
