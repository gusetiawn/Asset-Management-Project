using AssetManagementAPI.Base;
using AssetManagementAPI.Context;
using AssetManagementAPI.Handler;
using AssetManagementAPI.Models;
using AssetManagementAPI.Repositories.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AssetManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestItemsController : BaseController<RequestItem, RequestItemRepository, int>
    {
        private readonly RequestItemRepository requestItemRepository;
        private readonly MyContext myContext;
        private readonly SendMail sendMail = new SendMail();
        public RequestItemsController(RequestItemRepository requestItemRepository, MyContext myContext) : base(requestItemRepository)
        {
            this.requestItemRepository = requestItemRepository;
            this.myContext = myContext;
        }
        
        [HttpPost("NewRequest")]
        public ActionResult RequestItem(RequestItem requestItem)
        {
            try
            {
                var request = new RequestItem
                {
                    AccountId = requestItem.AccountId,
                    ItemId = requestItem.ItemId,
                    StartDate = requestItem.StartDate,
                    EndDate = requestItem.EndDate,
                    Quantity = requestItem.Quantity,
                    Notes = requestItem.Notes,
                    StatusId = 1 //Id 1 di table status aku buat "Waiting for Approval"
                };
                myContext.RequestItems.Add(request);
                myContext.SaveChanges();

                var user = myContext.Users.Where(u => u.Id == requestItem.AccountId).FirstOrDefault();
                var subject = "Request Item Berhasil";
                var body = "Hi, Request anda berhasil. Status request Anda saat ini adalah waiting for approval. Silakan menunggu beberapa saat sampai disetujui oleh manager. Terima kasih. ";
                sendMail.SendEmail(user.Email, body, subject);

                return StatusCode(200, new { status = HttpStatusCode.OK, message = "Request Item Berhasil"});

            }
            catch (Exception)
            {

                return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "Request Item Gagal" });
            }

        }
        
        
    }
}
