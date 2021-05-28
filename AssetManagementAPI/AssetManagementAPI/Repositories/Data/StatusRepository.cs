using AssetManagementAPI.Context;
using AssetManagementAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetManagementAPI.Repositories.Data
{
    public class StatusRepository : GeneralRepository<MyContext, Status, int>
    {
        public StatusRepository(MyContext myContext) : base(myContext)
        {

        }
    }
}
