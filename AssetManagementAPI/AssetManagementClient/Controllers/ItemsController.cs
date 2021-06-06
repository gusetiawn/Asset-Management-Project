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
    public class ItemsController : BaseController<Item, ItemRepository, int>
    {
        private readonly ItemRepository repository;

        public ItemsController(ItemRepository repository) : base(repository)
        {
            this.repository = repository;
        }
        public IActionResult Index()
        {
            return View();
        }


    }
}
