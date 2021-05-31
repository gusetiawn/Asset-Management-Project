using AssetManagementAPI.Base;
using AssetManagementAPI.Context;
using AssetManagementAPI.Handler;
using AssetManagementAPI.Models;
using AssetManagementAPI.Repositories.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        [HttpPut("Approval")]
        public ActionResult ApproveRequest(RequestItem requestItem)
        {
            try
            {
                var request = new RequestItem
                {
                    Id = requestItem.Id,
                    AccountId = requestItem.AccountId,
                    ItemId = requestItem.ItemId,
                    StartDate = requestItem.StartDate,
                    EndDate = requestItem.EndDate,
                    Quantity = requestItem.Quantity,
                    Notes = requestItem.Notes,
                    StatusId = 2 //Id 2 di table status aku buat "Approved"
                };
                myContext.Entry(request).State = EntityState.Modified;
                myContext.SaveChanges();

                var user = myContext.Users.Where(u => u.Id == requestItem.AccountId).FirstOrDefault();
                var subject = "Approval Request";
                var body = "Hi, Request anda telah disetujui oleh Manager. Silahkan Anda mendatangi admin untuk proses selanjutnya. Terima kasih. ";
                sendMail.SendEmail(user.Email, body, subject);

                return StatusCode(200, new { status = HttpStatusCode.OK, message = "Request Item Berhasil di Approve" });

            }
            catch (Exception)
            {

                return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "Request Item Gagal" });
            }

        }
        [HttpPut("Reject")]
        public ActionResult Reject(RequestItem requestItem)
        {
            try
            {
                var request = new RequestItem
                {
                    Id = requestItem.Id,
                    AccountId = requestItem.AccountId,
                    ItemId = requestItem.ItemId,
                    StartDate = requestItem.StartDate,
                    EndDate = requestItem.EndDate,
                    Quantity = requestItem.Quantity,
                    Notes = requestItem.Notes,
                    StatusId = 3 //Id 2 di table status aku buat "Approved"
                };
                myContext.Entry(request).State = EntityState.Modified;
                myContext.SaveChanges();

                var user = myContext.Users.Where(u => u.Id == requestItem.AccountId).FirstOrDefault();
                var subject = "Request for An Asset";
                var body = "Hi, mohon maaf request anda  ditolak";
                sendMail.SendEmail(user.Email, body, subject);

                return StatusCode(200, new { status = HttpStatusCode.OK, message = "Request Item Anda Ditolak " });

            }
            catch (Exception)
            {

                return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "Gagal Penolakan" });
            }

        }
        [HttpPut("TakeAnAsset")]
        public ActionResult TakeAnAsset(RequestItem requestItem)
        {
            try
            {
                var request = new RequestItem
                {
                    Id = requestItem.Id,
                    AccountId = requestItem.AccountId,
                    ItemId = requestItem.ItemId,
                    StartDate = requestItem.StartDate,
                    EndDate = requestItem.EndDate,
                    Quantity = requestItem.Quantity,
                    Notes = requestItem.Notes,
                    StatusId = 4 //Id 2 di table status aku buat "Approved"
                };
                myContext.Entry(request).State = EntityState.Modified;
                myContext.SaveChanges();

                var user = myContext.Users.Where(u => u.Id == requestItem.AccountId).FirstOrDefault();
                var subject = "Asset Telah anda terima";
                var body = "Hi, pulangkan asset sesuai dengan waktunya ";
                sendMail.SendEmail(user.Email, body, subject);

                return StatusCode(200, new { status = HttpStatusCode.OK, message = "Request Item Berhasil di ambil" });

            }
            catch (Exception)
            {

                return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "Batal diambil" });
            }

        }

    }
}
