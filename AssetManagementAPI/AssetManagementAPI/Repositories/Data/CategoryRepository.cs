using AssetManagementAPI.Context;
using AssetManagementAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetManagementAPI.Repositories.Data
{
    public class CategoryRepository : GeneralRepository<MyContext, Category, int>
    {
        public CategoryRepository(MyContext myContext) : base(myContext)
        {

        }
    }
}
