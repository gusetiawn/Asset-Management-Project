using AssetManagementAPI.Context;
using AssetManagementAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetManagementAPI.Repositories.Data
{
    public class GenderRepository : GeneralRepository<MyContext, Gender, int>
    {
        public GenderRepository(MyContext myContext) : base(myContext)
        {

        }
    }
}
