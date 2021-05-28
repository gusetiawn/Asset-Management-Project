using AssetManagementAPI.Context;
using AssetManagementAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetManagementAPI.Repositories.Data
{
    public class ItemRepository : GeneralRepository<MyContext, Item, int>
    {
        public ItemRepository(MyContext myContext) : base(myContext)
        {

        }
    }
}
