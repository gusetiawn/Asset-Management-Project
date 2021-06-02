using AssetManagementAPI.Base;
using AssetManagementAPI.Context;
using AssetManagementAPI.Handler;
using AssetManagementAPI.Models;
using AssetManagementAPI.Repositories.Data;
using AssetManagementAPI.ViewModel;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
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
        private readonly CommandType commandType = CommandType.StoredProcedure;
        public IConfiguration Configuration { get; }
        public RequestItemsController(RequestItemRepository requestItemRepository, MyContext myContext, IConfiguration Configuration) : base(requestItemRepository)
        {
            this.requestItemRepository = requestItemRepository;
            this.myContext = myContext;
            this.Configuration = Configuration;
        }

        [HttpPost("NewRequest")]
        public ActionResult RequestItem(RequestItem requestItem)
        {
            //try
            //{
                var request = new RequestItem
                {
                    AccountId = requestItem.AccountId,
                    ItemId = requestItem.ItemId,
                    StartDate = requestItem.StartDate,
                    EndDate = requestItem.EndDate,
                    Quantity = requestItem.Quantity,
                    Notes = requestItem.Notes,
                    StatusId = 1 //Waiting for Approval"
                };
                myContext.RequestItems.Add(request);
                myContext.SaveChanges();

                var recentQty = myContext.Items.Where(I => I.Id == requestItem.ItemId).AsNoTracking().FirstOrDefault();
                var QtyNow = recentQty.Quantity - requestItem.Quantity;
                var item = new Item
                {
                    Id = recentQty.Id,
                    Name = recentQty.Name,
                    Quantity = QtyNow,
                    CategoryId = recentQty.CategoryId
                };
                myContext.Entry(item).State = EntityState.Modified;
                myContext.SaveChanges();

                var user = myContext.Users.Where(u => u.Id == requestItem.AccountId).FirstOrDefault();
                var subject = "Request for An Asset";
                var body = $"Hai {user.FirstName},\nRequest anda telah kami terima, Request Anda akan kami Proses Setelah mendapatkan Persetujuan dari Manager. Kami akan informasikan kembali mengenai hal tersebut.\n Terima kasih dan Selamat Bekerja.";
                sendMail.SendEmail(user.Email, body, subject);

                return StatusCode(200, new { status = HttpStatusCode.OK, message = "Request Item Berhasil" });

            //}
            //catch (Exception)
            //{

            //    return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "Request Item Gagal" });
            //}

        }

        [HttpPut("Approve")]
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
                    StatusId = 2 //Already Approved
                };
                myContext.Entry(request).State = EntityState.Modified;
                myContext.SaveChanges();

                var user = myContext.Users.Where(u => u.Id == requestItem.AccountId).FirstOrDefault();
                var subject = "Request for An Asset";
                var body = $"Hai {user.FirstName},\nRequest anda telah disetujui oleh Manager, Request Anda sedang kami Proses. Silahkan anda datang ke ruangan Admin untuk tahap selanjutnya.\n Terima kasih dan Selamat Bekerja.";
                sendMail.SendEmail(user.Email, body, subject);

                return StatusCode(200, new { status = HttpStatusCode.OK, message = "Request Item Berhasil di Setujui Manager" });

            }
            catch (Exception)
            {

                return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "Proses Persetujuan Gagal" });
            }

        }
        [HttpPut("Reject")]
        public ActionResult RejectRequest(RequestItem requestItem)
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
                    StatusId = 3 //Has Been Rejected
                };
                myContext.Entry(request).State = EntityState.Modified;
                myContext.SaveChanges();

                var user = myContext.Users.Where(u => u.Id == requestItem.AccountId).FirstOrDefault();
                var subject = "Request for An Asset";
                var body = $"Hai {user.FirstName},\nMohon maaf Request anda tidak disetujui oleh Manager, Request Anda tidak bisa kami Proses. Untuk Info lebih lanjut anda dapat menghubungi pihak manager.\n Terima kasih dan Selamat Bekerja.";
                sendMail.SendEmail(user.Email, body, subject);

                return StatusCode(200, new { status = HttpStatusCode.OK, message = "Request Item anda tidak disetujui Manager" });

            }
            catch (Exception)
            {

                return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "Proses Penolakan Gagal" });
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
                    StatusId = 4 //Already Picked Up
                };
                myContext.Entry(request).State = EntityState.Modified;
                myContext.SaveChanges();

                var user = myContext.Users.Where(u => u.Id == requestItem.AccountId).FirstOrDefault();
                var subject = "Request An Asset";
                var body = $"Hai {user.FirstName},\nTerima kasih telah melakukan request denngan sistem kami, Kami harap Anda dapat menjaga Asset dengan baik dan mengembalikan sesuai dengan jadwal yang ditetapkan, apabila terjadi kerusakan dan kehilangan maka anda akan dikenakan biaya pinalty sesuai dengan kondisi yang terjadi, Mohon Kerjasamanya.\n Terima kasih dan Selamat Bekerja.";
                sendMail.SendEmail(user.Email, body, subject);

                return StatusCode(200, new { status = HttpStatusCode.OK, message = "Item telah berhasil diambil Pemohon" });

            }
            catch (Exception)
            {

                return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "Item Gagal diambil" });
            }

        }
        [HttpPut("ReturnAsset")]
        public ActionResult ReturnAsset(RequestItem requestItem)
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
                    StatusId = 5 //Has been Returned
                };
                myContext.Entry(request).State = EntityState.Modified;
                myContext.SaveChanges();

                var user = myContext.Users.Where(u => u.Id == requestItem.AccountId).FirstOrDefault();
                var subject = "Return An Asset";
                var body = $"Hai {user.FirstName},\nTerima kasih telah ikut serta dalam menjaga Asset kami, Status request Anda telah selesai dan anda terbebas dari tanggungjawab terhadap Asset yang telah anda pinjam sebelumnya.\n Terima kasih dan Selamat Bekerja.";
                sendMail.SendEmail(user.Email, body, subject);

                return StatusCode(200, new { status = HttpStatusCode.OK, message = "Asset telah Berhasil dikembalikan" });

            }
            catch (Exception)
            {

                return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "Gagal Proses Pengembalian Asset" });
            }

        }

        [HttpGet("GetRequest/{id}")] // Aku Coba Pakai SP
        public ActionResult ReturnAsset(string id)
        {
            try
            {
                string connectStr = Configuration.GetConnectionString("MyConnection");
                SqlConnection db = new SqlConnection(connectStr);
                var parameter = new DynamicParameters();
                string readSp = "SP_GetRequestId";
                parameter.Add("id_", id);
                var result = db.Query<GetRequestIdVM>(readSp, parameter, commandType: commandType).ToList();
                if (result.Count() >= 1)
                {
                    return Ok(result);
                }
                return StatusCode(404, "Id Tidak ditemukan");

                //return StatusCode(200, new { status = HttpStatusCode.OK, message = "Request Item Berhasil di ambil" });

            }
            catch (Exception)
            {

                return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "Gagal Mengambil data Request" });
            }

        }
        [HttpGet("UserRequest")]
        public ActionResult UserRequest()
        {
            var userRequest = from A in myContext.Accounts
                           join R in myContext.RequestItems on A.Id equals R.AccountId
                           join I in myContext.Items on R.ItemId equals I.Id
                           join C in myContext.Categories on I.CategoryId equals C.Id
                           join S in myContext.Statuses on R.StatusId equals S.Id
                           select new
                           {
                               Id = R.Id,
                               Item = I.Name,
                               AccountId = R.AccountId,
                               StartDate = R.StartDate,
                               EndDate = R.EndDate,
                               Notes = R.Notes,
                               Quantity = R.Quantity,
                               Status = S.Name,
                               Category = C.Name
                           };
            return Ok(userRequest);

        }
    }
}
