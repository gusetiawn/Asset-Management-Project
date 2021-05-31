using AssetManagementAPI.Base;
using AssetManagementAPI.Context;
using AssetManagementAPI.Handler;
using AssetManagementAPI.Models;
using AssetManagementAPI.Repositories.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AssetManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReturnItemsController : BaseController<ReturnItem, ReturnItemRepository, int>
    {
        private readonly ReturnItemRepository returnItemRepository;
        private readonly MyContext myContext;
        private readonly SendMail sendMail = new SendMail();
        public ReturnItemsController(ReturnItemRepository returnItemRepository, MyContext myContext) : base(returnItemRepository)
        {
            this.returnItemRepository = returnItemRepository;
            this.myContext = myContext;
        }

        [HttpPost("NewRequest")] 
        public ActionResult ReturnItem(ReturnItem returnItem)
        {
            try
            {
                var returnItm = new ReturnItem
                {
                    RequestItemId = returnItem.RequestItemId,
                    Penalty = returnItem.Penalty,
                    Notes = returnItem.Notes
                };
                myContext.ReturnItems.Add(returnItm);
                myContext.SaveChanges();

                var reqItem = myContext.RequestItems.Where(u => u.Id == returnItem.RequestItemId).FirstOrDefault();
                var user = myContext.Users.Where(u => u.Id == reqItem.AccountId).FirstOrDefault();
                var subject = "Return Item Berhasil";
                var body = "Hi, Terima kasih Anda telah mengembalikan barang yang telah Anda pinjam. ";
                sendMail.SendEmail(user.Email, body, subject);

                return StatusCode(200, new { status = HttpStatusCode.OK, message = "Return Item Berhasil" });
            }
            catch (Exception)
            {
                return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "Return Item Gagal" });
            }
        }
    }
}
