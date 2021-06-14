using AssetManagementAPI.Base;
using AssetManagementAPI.Context;
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
    public class ItemsController : BaseController<Item, ItemRepository, int>
    {
        private readonly ItemRepository itemRepository;
        private readonly MyContext myContext;
        public ItemsController(ItemRepository itemRepository, MyContext myContext) : base(itemRepository)
        {
            this.itemRepository = itemRepository;
            this.myContext = myContext;
        }
        [HttpGet("DataItem")]
        public ActionResult DataItem()
        {
            var dataItem = from I in myContext.Items
                              join C in myContext.Categories on I.CategoryId equals C.Id

                              select new
                              {
                                  Id = I.Id,
                                  Name = I.Name,
                                  Quantity = I.Quantity,
                                  CategoryId = I.CategoryId,
                                  Category = C.Name
                              };
            return Ok(dataItem);

        }
    }
}
