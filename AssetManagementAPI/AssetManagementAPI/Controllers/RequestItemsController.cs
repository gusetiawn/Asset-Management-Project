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
            try
            {
                var checkItem = myContext.Items.Where(i => i.Id == requestItem.ItemId).FirstOrDefault();
                if (requestItem.Quantity > checkItem.Quantity)
                {
                    return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "Request Item Gagal" });
                }
                else
                {
                    var request = new RequestItem
                    {
                        AccountId = requestItem.AccountId,
                        ItemId = requestItem.ItemId,
                        StartDate = requestItem.StartDate,
                        EndDate = requestItem.EndDate,
                        Quantity = requestItem.Quantity,
                        Notes = requestItem.Notes,
                        StatusId = 3 //Waiting for Approval"
                    };
                    myContext.RequestItems.Add(request);
                    myContext.SaveChanges();

                    var data = myContext.Items.Include(a => a.RequestItems).Where(e => e.Id == request.ItemId).FirstOrDefault();
                    data.Quantity -= requestItem.Quantity;
                    myContext.Entry(data).State = EntityState.Modified;
                    myContext.SaveChanges();

                    var user = myContext.Users.Where(u => u.Id == requestItem.AccountId).FirstOrDefault();
                    var dep = myContext.Departments.Where(d => d.Id == user.DepartmentId).FirstOrDefault();
                    var currentItem = myContext.RequestItems.Where(i => i.AccountId == user.Id).FirstOrDefault();
                    var item = myContext.Items.Where(i => i.Id == requestItem.ItemId).FirstOrDefault();
                    
                    var subject = $"Request #{request.Id} Received";
                    string mailbody = string.Empty;
                    mailbody += "<body>";
                    mailbody += "<div style='margin: 25px;'>";
                    mailbody += "<div style='background-color: #4ac09d; border-radius: 25px;'>";
                    mailbody += "<p style='font-weight: bold; font-size: 25px; padding-top: 25px; color: white; text-align: center;'>Asset Management System</p>";
                    mailbody += "<div style='background-color:#f0f0f0; border-bottom-right-radius:25px; border-bottom-left-radius:25px; margin-top:-30px;'>";
                    mailbody += "<div style='margin-left: 35px; margin-right: 35px; padding-bottom: 50px;'>";
                    mailbody += "<h4 style='padding-top: 20px; color: black;'>Hello " + user.FirstName + " ,</h4>";
                    mailbody += "<p style='color: black;'>We have received your request,</p>";
                    mailbody += "<p style='color: black;'>We will process your request after getting approval from the Manager</p>";
                    mailbody += "<p style='color: black;'>Here are the details:</p>";
                    mailbody += "<p style='color: black;'>Name        : " + user.FirstName + " " + user.LastName + "</p>";
                    mailbody += "<p style='color: black;'>Department  : " + dep.Name + " </p>";
                    mailbody += "<p style='color: black;'>Item Request: " + item.Name + " </p>";
                    mailbody += "<p style='color: black;'>Date        : " + request.StartDate.ToString().Substring(0,10) + " - " + request.EndDate.ToString().Substring(0,10) + "</p>";
                    mailbody += "<br>";
                    mailbody += "<p style='color: black;'>Best Regards,</p>";
                    mailbody += "<p style='color: black;'>Management System</p>";
                    mailbody += "</div>";

                    mailbody += "<div style='color: inherit; font - size:inherit; line - height:inherit'>";
                    mailbody += "<table width='100%' cellpadding='0' cellspacing='0' style='border-spacing:0!important;border-collapse:collapse;text-align:center;font-family:Arial,sans-serif;font-size:12px;line-height:135%;color:#23496d;margin-bottom:0;padding:0' align='center'>";
                    mailbody += "<tbody>";
                    mailbody += "<tr>";
                    mailbody += "<td align='center' valign='top' style='border-collapse:collapse;font-family:Lato,Tahoma,sans-serif;font-size:13px;color:#444444;word-break:break-word;text-align:center;margin-bottom:0;line-height:135%;padding:10px 20px'>";
                    mailbody += "<p style='font-family:Verdana,sans-serif;font-size:11px;font-weight:normal;text-decoration:none;font-style:normal;color:#444444'> APL Tower Lantai 37, Jl. Letjen S. Parman Kav. 28, RT.12/RW.6, Tj. Duren Sel., Jakarta Barat, Kota Jakarta Barat, Daerah Khusus Ibukota Jakarta 11470 </p>";
                    mailbody += "<font color='#888888'>";
                    mailbody += "<p>";
                    mailbody += "<a style='font-family:Verdana,sans-serif;font-size:10px;color:#00000f;font-weight:normal;font-style:normal' rel='noreferrer'>© PT. Mitra Integrasi Informatika</a>";
                    mailbody += "</p>";
                    mailbody += "</font>";
                    mailbody += "</td>";
                    mailbody += "</tr>";
                    mailbody += "</tbody>";
                    mailbody += "</table>";
                    mailbody += "<div>";
                    mailbody += "<div>";
                    mailbody += "<div>";

                    mailbody += "</div>";
                    mailbody += "</div>";
                    mailbody += "</div>";
                    mailbody += "</div>";
                    mailbody += "</body>";
                    var body = mailbody;
                    sendMail.SendEmail(user.Email, body, subject);

                    return StatusCode(200, new { status = HttpStatusCode.OK, message = "Request Item Berhasil" });
                    
                }

            }
            catch (Exception)
            {
                return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "Request Item Gagal" });
            }

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
                    StatusId = 4 //Already Approved
                };
                myContext.Entry(request).State = EntityState.Modified;
                myContext.SaveChanges();

                var user = myContext.Users.Where(u => u.Id == requestItem.AccountId).FirstOrDefault();
                var item = myContext.Items.Where(i => i.Id == requestItem.ItemId).FirstOrDefault();

                var subject = $"Request #{request.Id} Approved";
                string mailbody = string.Empty;
                mailbody += "<body>";
                mailbody += "<div style='margin: 25px;'>";
                mailbody += "<div style='background-color: #4ac09d; border-radius: 25px;'>";
                mailbody += "<p style='font-weight: bold; font-size: 25px; padding-top: 25px; color: white; text-align: center;'>Asset Management System</p>";
                mailbody += "<div style='background-color:#f0f0f0; border-bottom-right-radius:25px; border-bottom-left-radius:25px; margin-top:-30px;'>";
                mailbody += "<div style='margin-left: 50px; margin-right: 50px; padding-bottom: 50px;'>";
                mailbody += "<h4 style='padding-top: 20px; color: black;'>Hello " + user.FirstName + ",</h4>";
                mailbody += "<p style='color: black;'>Congratulations!, your request that you submitted has been approved by Manager.</p>";
                mailbody += "<p style='color: black;'>Here are the details:</p>";
                mailbody += "<p style='color: black;'>Name        : " + user.FirstName + " " + user.LastName + "</p>";
                mailbody += "<p style='color: black;'>Item Request: " + item.Name + " </p>";
                mailbody += "<p style='color: black;'>Date        : " + request.StartDate.ToString().Substring(0, 10) + " - " + request.EndDate.ToString().Substring(0, 10) + "</p>";
                mailbody += "<p style='color: black;'>Your request is being processed by us.</p>";
                mailbody += "<p style='color: black;'>Please come to the Admin room at APL Tower Lt. 18 for the next step.</p>";
                mailbody += "<div style='text-align: center;'>";
                mailbody += "<br>";
                mailbody += "<p style='color: black;'>Best Regards,</p>";
                mailbody += "<p style='color: black;'>Management System</p>";
                mailbody += "</div>";

                mailbody += "<div style='color: inherit; font - size:inherit; line - height:inherit'>";
                mailbody += "<table width='100%' cellpadding='0' cellspacing='0' style='border-spacing:0!important;border-collapse:collapse;text-align:center;font-family:Arial,sans-serif;font-size:12px;line-height:135%;color:#23496d;margin-bottom:0;padding:0' align='center'>";
                mailbody += "<tbody>";
                mailbody += "<tr>";
                mailbody += "<td align='center' valign='top' style='border-collapse:collapse;font-family:Lato,Tahoma,sans-serif;font-size:13px;color:#444444;word-break:break-word;text-align:center;margin-bottom:0;line-height:135%;padding:10px 20px'>";
                mailbody += "<p style='font-family:Verdana,sans-serif;font-size:11px;font-weight:normal;text-decoration:none;font-style:normal;color:#444444'> APL Tower Lantai 37, Jl. Letjen S. Parman Kav. 28, RT.12/RW.6, Tj. Duren Sel., Jakarta Barat, Kota Jakarta Barat, Daerah Khusus Ibukota Jakarta 11470 </p>";
                mailbody += "<font color='#888888'>";
                mailbody += "<p>";
                mailbody += "<a style='font-family:Verdana,sans-serif;font-size:10px;color:#00000f;font-weight:normal;font-style:normal' rel='noreferrer'>© PT. Mitra Integrasi Informatika</a>";
                mailbody += "</p>";
                mailbody += "</font>";
                mailbody += "</td>";
                mailbody += "</tr>";
                mailbody += "</tbody>";
                mailbody += "</table>";
                mailbody += "<div>";
                mailbody += "<div>";
                mailbody += "<div>";

                mailbody += "</div>";
                mailbody += "</div>";
                mailbody += "</div>";
                mailbody += "</div>";
                mailbody += "</body>";
                var body = mailbody;
                sendMail.SendEmail(user.Email, body, subject);

                var subjectforadmin = $"Request #{request.Id} Need Asset";
                string mailbodyforadmin = string.Empty;
                mailbodyforadmin += "<body>";
                mailbodyforadmin += "<div style='margin: 25px;'>";
                mailbodyforadmin += "<div style='background-color: #4ac09d; border-radius: 25px;'>";
                mailbodyforadmin += "<p style='font-weight: bold; font-size: 25px; padding-top: 25px; color: white; text-align: center;'>Asset Management System</p>";
                mailbodyforadmin += "<div style='background-color:#f0f0f0; border-bottom-right-radius:25px; border-bottom-left-radius:25px; margin-top:-30px;'>";
                mailbodyforadmin += "<div style='margin-left: 50px; margin-right: 50px; padding-bottom: 50px;'>";
                mailbodyforadmin += "<h4 style='padding-top: 20px; color: black;'>Hello Admin,</h4>";
                mailbodyforadmin += "<p style='color: black;'>There is an asset request, please prepare the item immediately</p>";
                mailbodyforadmin += "<p style='color: black;'>Here are the details:</p>";
                mailbodyforadmin += "<p style='color: black;'>Name        : " + user.FirstName + " " + user.LastName + "</p>";
                mailbodyforadmin += "<p style='color: black;'>Item Request: " + item.Name + " </p>";
                mailbodyforadmin += "<p style='color: black;'>Date        : " + request.StartDate.ToString().Substring(0, 10) + " - " + request.EndDate.ToString().Substring(0, 10) + "</p>";
                mailbodyforadmin += "<div style='text-align: center;'>";
                mailbodyforadmin += "<br>";
                mailbodyforadmin += "<p style='color: black;'>Best Regards,</p>";
                mailbodyforadmin += "<p style='color: black;'>Management System</p>";
                mailbodyforadmin += "</div>";

                mailbodyforadmin += "<div style='color: inherit; font - size:inherit; line - height:inherit'>";
                mailbodyforadmin += "<table width='100%' cellpadding='0' cellspacing='0' style='border-spacing:0!important;border-collapse:collapse;text-align:center;font-family:Arial,sans-serif;font-size:12px;line-height:135%;color:#23496d;margin-bottom:0;padding:0' align='center'>";
                mailbodyforadmin += "<tbody>";
                mailbodyforadmin += "<tr>";
                mailbodyforadmin += "<td align='center' valign='top' style='border-collapse:collapse;font-family:Lato,Tahoma,sans-serif;font-size:13px;color:#444444;word-break:break-word;text-align:center;margin-bottom:0;line-height:135%;padding:10px 20px'>";
                mailbodyforadmin += "<p style='font-family:Verdana,sans-serif;font-size:11px;font-weight:normal;text-decoration:none;font-style:normal;color:#444444'> APL Tower Lantai 37, Jl. Letjen S. Parman Kav. 28, RT.12/RW.6, Tj. Duren Sel., Jakarta Barat, Kota Jakarta Barat, Daerah Khusus Ibukota Jakarta 11470 </p>";
                mailbodyforadmin += "<font color='#888888'>";
                mailbodyforadmin += "<p>";
                mailbodyforadmin += "<a style='font-family:Verdana,sans-serif;font-size:10px;color:#00000f;font-weight:normal;font-style:normal' rel='noreferrer'>© PT. Mitra Integrasi Informatika</a>";
                mailbodyforadmin += "</p>";
                mailbodyforadmin += "</font>";
                mailbodyforadmin += "</td>";
                mailbodyforadmin += "</tr>";
                mailbodyforadmin += "</tbody>";
                mailbodyforadmin += "</table>";
                mailbodyforadmin += "<div>";
                mailbodyforadmin += "<div>";
                mailbodyforadmin += "<div>";

                mailbodyforadmin += "</div>";
                mailbodyforadmin += "</div>";
                mailbodyforadmin += "</div>";
                mailbodyforadmin += "</div>";
                mailbodyforadmin += "</body>";
                var bodyadmin = mailbodyforadmin;
                sendMail.SendEmail("gusetiawn@gmail.com", bodyadmin, subjectforadmin);

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
                    StatusId = 1 //Has Been Rejected
                };
                myContext.Entry(request).State = EntityState.Modified;
                myContext.SaveChanges();

                var recentQty = myContext.Items.Where(I => I.Id == requestItem.ItemId).FirstOrDefault();
                var data = myContext.Items.Include(a => a.RequestItems).Where(e => e.Id == request.ItemId).FirstOrDefault();
                data.Quantity = recentQty.Quantity + requestItem.Quantity;
                myContext.Entry(data).State = EntityState.Modified;
                myContext.SaveChanges();

                var user = myContext.Users.Where(u => u.Id == requestItem.AccountId).FirstOrDefault();
                var item = myContext.Items.Where(i => i.Id == requestItem.ItemId).FirstOrDefault();

                var subject = $"Request #{request.Id} Rejected";
                string mailbody = string.Empty;
                mailbody += "<body>";
                mailbody += "<div style='margin: 25px;'>";
                mailbody += "<div style='background-color: #4ac09d; border-radius: 25px;'>";
                mailbody += "<p style='font-weight: bold; font-size: 25px; padding-top: 25px; color: white; text-align: center;'>Asset Management System</p>";
                mailbody += "<div style='background-color:#f0f0f0; border-bottom-right-radius:25px; border-bottom-left-radius:25px; margin-top:-30px;'>";
                mailbody += "<div style='margin-left: 50px; margin-right: 50px; padding-bottom: 50px;'>";
                mailbody += "<h4 style='padding-top: 20px; color: black;'>Hello " + user.FirstName + ",</h4>";
                mailbody += "<p style='color: black;'>Sorry, request that you submitted was rejected.</p>";
                mailbody += "<p style='color: black;'>Here are the details:</p>";
                mailbody += "<p style='color: black;'>Name        : " + user.FirstName + " " + user.LastName + "</p>";
                mailbody += "<p style='color: black;'>Item Request: " + item.Name + " </p>";
                mailbody += "<p style='color: black;'>Date        : " + request.StartDate.ToString().Substring(0, 10) + " - " + request.EndDate.ToString().Substring(0, 10) + "</p>";
                mailbody += "<p style='color: black;'>We can't process your request.</p>";
                mailbody += "<p style='color: black;'>For more information you can contact the manager.</p>";
                mailbody += "<div style='text-align: center;'>";
                mailbody += "<br>";
                mailbody += "<p style='color: black;'>Best Regards,</p>";
                mailbody += "<p style='color: black;'>Management System</p>";
                mailbody += "</div>";

                mailbody += "<div style='color: inherit; font - size:inherit; line - height:inherit'>";
                mailbody += "<table width='100%' cellpadding='0' cellspacing='0' style='border-spacing:0!important;border-collapse:collapse;text-align:center;font-family:Arial,sans-serif;font-size:12px;line-height:135%;color:#23496d;margin-bottom:0;padding:0' align='center'>";
                mailbody += "<tbody>";
                mailbody += "<tr>";
                mailbody += "<td align='center' valign='top' style='border-collapse:collapse;font-family:Lato,Tahoma,sans-serif;font-size:13px;color:#444444;word-break:break-word;text-align:center;margin-bottom:0;line-height:135%;padding:10px 20px'>";
                mailbody += "<p style='font-family:Verdana,sans-serif;font-size:11px;font-weight:normal;text-decoration:none;font-style:normal;color:#444444'> APL Tower Lantai 37, Jl. Letjen S. Parman Kav. 28, RT.12/RW.6, Tj. Duren Sel., Jakarta Barat, Kota Jakarta Barat, Daerah Khusus Ibukota Jakarta 11470 </p>";
                mailbody += "<font color='#888888'>";
                mailbody += "<p>";
                mailbody += "<a style='font-family:Verdana,sans-serif;font-size:10px;color:#00000f;font-weight:normal;font-style:normal' rel='noreferrer'>© PT. Mitra Integrasi Informatika</a>";
                mailbody += "</p>";
                mailbody += "</font>";
                mailbody += "</td>";
                mailbody += "</tr>";
                mailbody += "</tbody>";
                mailbody += "</table>";
                mailbody += "<div>";
                mailbody += "<div>";
                mailbody += "<div>";

                mailbody += "</div>";
                mailbody += "</div>";
                mailbody += "</div>";
                mailbody += "</div>";
                mailbody += "</body>";
                var body = mailbody;
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
                    StatusId = 5 //Already Picked Up
                };
                myContext.Entry(request).State = EntityState.Modified;
                myContext.SaveChanges();

                var user = myContext.Users.Where(u => u.Id == requestItem.AccountId).FirstOrDefault();
                var item = myContext.Items.Where(i => i.Id == requestItem.ItemId).FirstOrDefault();

                var subject = $"Request #{request.Id} Picked Up";
                string mailbody = string.Empty;
                mailbody += "<body>";
                mailbody += "<div style='margin: 25px;'>";
                mailbody += "<div style='background-color: #4ac09d; border-radius: 25px;'>";
                mailbody += "<p style='font-weight: bold; font-size: 25px; padding-top: 25px; color: white; text-align: center;'>Asset Management System</p>";
                mailbody += "<div style='background-color:#f0f0f0; border-bottom-right-radius:25px; border-bottom-left-radius:25px; margin-top:-30px;'>";
                mailbody += "<div style='margin-left: 50px; margin-right: 50px; padding-bottom: 50px;'>";
                mailbody += "<h4 style='padding-top: 20px; color: black;'>Hello " + user.FirstName + ",</h4>";
                mailbody += "<p style='color: black;'>Thank you for making a request with our system,.</p>";
                mailbody += "<p style='color: black;'>Here are the details:</p>";
                mailbody += "<p style='color: black;'>Name        : " + user.FirstName + " " + user.LastName + "</p>";
                mailbody += "<p style='color: black;'>Item Request: " + item.Name + " </p>";
                mailbody += "<p style='color: black;'>Date        : " + request.StartDate.ToString().Substring(0, 10) + " - " + request.EndDate.ToString().Substring(0, 10) + "</p>";
                mailbody += "<p style='color: black;'>We hope you can keep the Assets well and return them according to the set schedule,</p>";
                mailbody += "<p style='color: black;'>if there is damage and loss then you will be charged a penalty fee according to the conditions that occur, please cooperate..</p>";
                mailbody += "<br>";
                mailbody += "<p style='color: black;'>Best Regards,</p>";
                mailbody += "<p style='color: black;'>Management System</p>";
                mailbody += "</div>";

                mailbody += "<div style='color: inherit; font - size:inherit; line - height:inherit'>";
                mailbody += "<table width='100%' cellpadding='0' cellspacing='0' style='border-spacing:0!important;border-collapse:collapse;text-align:center;font-family:Arial,sans-serif;font-size:12px;line-height:135%;color:#23496d;margin-bottom:0;padding:0' align='center'>";
                mailbody += "<tbody>";
                mailbody += "<tr>";
                mailbody += "<td align='center' valign='top' style='border-collapse:collapse;font-family:Lato,Tahoma,sans-serif;font-size:13px;color:#444444;word-break:break-word;text-align:center;margin-bottom:0;line-height:135%;padding:10px 20px'>";
                mailbody += "<p style='font-family:Verdana,sans-serif;font-size:11px;font-weight:normal;text-decoration:none;font-style:normal;color:#444444'> APL Tower Lantai 37, Jl. Letjen S. Parman Kav. 28, RT.12/RW.6, Tj. Duren Sel., Jakarta Barat, Kota Jakarta Barat, Daerah Khusus Ibukota Jakarta 11470 </p>";
                mailbody += "<font color='#888888'>";
                mailbody += "<p>";
                mailbody += "<a style='font-family:Verdana,sans-serif;font-size:10px;color:#00000f;font-weight:normal;font-style:normal' rel='noreferrer'>© PT. Mitra Integrasi Informatika</a>";
                mailbody += "</p>";
                mailbody += "</font>";
                mailbody += "</td>";
                mailbody += "</tr>";
                mailbody += "</tbody>";
                mailbody += "</table>";
                mailbody += "<div>";
                mailbody += "<div>";
                mailbody += "<div>";

                mailbody += "</div>";
                mailbody += "</div>";
                mailbody += "</div>";
                mailbody += "</div>";
                mailbody += "</body>";
                var body = mailbody;
                sendMail.SendEmail(user.Email, body, subject);

                return StatusCode(200, new { status = HttpStatusCode.OK, message = "Item telah berhasil diambil Pemohon" });

            }
            catch (Exception)
            {

                return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "Item Gagal diambil" });
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

            }
            catch (Exception)
            {

                return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "Gagal Mengambil data Request" });
            }

        }
        [HttpGet("UserRequest")]
        public ActionResult UserRequest()
        {
            var userRequest = from U in myContext.Users
                           join A in myContext.Accounts on U.Id equals A.Id
                           join R in myContext.RequestItems on A.Id equals R.AccountId
                           join I in myContext.Items on R.ItemId equals I.Id
                           join C in myContext.Categories on I.CategoryId equals C.Id
                           join S in myContext.Statuses on R.StatusId equals S.Id
                           select new
                           {
                               Id = R.Id,
                               Name = U.FirstName + " " + U.LastName,
                               Item = I.Name,
                               ItemId = R.ItemId,
                               AccountId = R.AccountId,
                               StartDate = R.StartDate,
                               EndDate = R.EndDate,
                               Notes = R.Notes,
                               Quantity = R.Quantity,
                               StatusId = S.Id,
                               Status = S.Name,
                               Category = C.Name
                           };
            return Ok(userRequest);

        }

        [HttpGet("{id}")]
        public ActionResult RequestedItems(string id)
        {
            var user = myContext.Users.Find(id);
            if (user != null)
            {
                var reqItems = from A in myContext.Accounts
                               join R in myContext.RequestItems on A.Id equals R.AccountId
                               join I in myContext.Items on R.ItemId equals I.Id
                               join C in myContext.Categories on I.CategoryId equals C.Id
                               join S in myContext.Statuses on R.StatusId equals S.Id
                               where A.Id == id
                               select new
                               {
                                   Id = R.Id,
                                   Item = I.Name,
                                   StartDate = R.StartDate,
                                   EndDate = R.EndDate,
                                   Quantity = R.Quantity,
                                   Notes = R.Notes,
                                   Status = S.Name
                               };
                return Ok(reqItems);
            }
            else
            {
                return NotFound("Id Not Registered");
            }
        }

        [HttpGet("RequestNeedsApproval")]
        public ActionResult RequestNeedsApproval()
        {
            var userRequest = from U in myContext.Users
                              join A in myContext.Accounts on U.Id equals A.Id
                              join R in myContext.RequestItems on A.Id equals R.AccountId
                              join I in myContext.Items on R.ItemId equals I.Id
                              join C in myContext.Categories on I.CategoryId equals C.Id
                              join S in myContext.Statuses on R.StatusId equals S.Id
                              where R.StatusId == 3
                              select new
                              {
                                  Id = R.Id,
                                  AccountId = R.AccountId,
                                  Name = U.FirstName + " " + U.LastName,
                                  Item = I.Name,
                                  ItemId = R.ItemId,
                                  StartDate = R.StartDate,
                                  EndDate = R.EndDate,
                                  Quantity = R.Quantity,
                                  Notes = R.Notes,
                                  Status = S.Name
                              };
            return Ok(userRequest);
        }

        [HttpGet("UserRequestTake")]
        public ActionResult UserRequestTake()
        {
            var userRequest = from A in myContext.Accounts
                              join R in myContext.RequestItems on A.Id equals R.AccountId
                              join I in myContext.Items on R.ItemId equals I.Id
                              join C in myContext.Categories on I.CategoryId equals C.Id
                              join S in myContext.Statuses on R.StatusId equals S.Id
                              join U in myContext.Users on A.Id equals U.Id
                              where R.StatusId == 4
                              select new
                              {
                                  Id = R.Id,
                                  Item = I.Name,
                                  ItemId = R.ItemId,
                                  Name = U.FirstName + " " + U.LastName,
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

        [HttpGet("UserRequestReturn")]
        public ActionResult UserRequestReturn()
        {
            var userRequest = from A in myContext.Accounts
                              join R in myContext.RequestItems on A.Id equals R.AccountId
                              join I in myContext.Items on R.ItemId equals I.Id
                              join C in myContext.Categories on I.CategoryId equals C.Id
                              join S in myContext.Statuses on R.StatusId equals S.Id
                              join U in myContext.Users on A.Id equals U.Id
                              where R.StatusId == 5
                              select new
                              {
                                  Id = R.Id,
                                  Item = I.Name,
                                  ItemId = R.ItemId,
                                  Name = U.FirstName + " " + U.LastName,
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
        [HttpGet("Id={id}")]
        public ActionResult GetRequestById(int id)
        {
            var userRequest = from U in myContext.Users
                              join A in myContext.Accounts on U.Id equals A.Id
                              join R in myContext.RequestItems on A.Id equals R.AccountId
                              join I in myContext.Items on R.ItemId equals I.Id
                              join C in myContext.Categories on I.CategoryId equals C.Id
                              join S in myContext.Statuses on R.StatusId equals S.Id
                              where R.Id == id
                              select new
                              {
                                  Id = R.Id,
                                  AccountId = R.AccountId,
                                  ItemId = R.ItemId,
                                  StartDate = R.StartDate,
                                  EndDate = R.EndDate,
                                  Quantity = R.Quantity,
                                  Notes = R.Notes
                              };
            return Ok(userRequest);
        }

        [HttpGet("ReqWaiting")]
        public ActionResult ReqWaiting()
        {
            var reqWaiting = from R in myContext.RequestItems
                            join S in myContext.Statuses on R.StatusId equals S.Id
                            where R.StatusId == 3
                            select new
                            {
                                Status = S.Name
                            };
            return Ok(reqWaiting);
        }

        [HttpGet("ReqApprove")]
        public ActionResult ReqApprove()
        {
            var reqApprove = from R in myContext.RequestItems
                             join S in myContext.Statuses on R.StatusId equals S.Id
                             where R.StatusId == 4
                             select new
                             {
                                 Status = S.Name
                             };
            return Ok(reqApprove);
        }

        [HttpGet("ReqReject")]
        public ActionResult ReqReject()
        {
            var reqReject = from R in myContext.RequestItems
                             join S in myContext.Statuses on R.StatusId equals S.Id
                             where R.StatusId == 1
                             select new
                             {
                                 Status = S.Name
                             };
            return Ok(reqReject);
        }

        [HttpGet("ReqReturn")]
        public ActionResult ReqReturn()
        {
            var reqReturn = from R in myContext.RequestItems
                             join S in myContext.Statuses on R.StatusId equals S.Id
                             where R.StatusId == 2
                             select new
                             {
                                 Status = S.Name
                             };
            return Ok(reqReturn);
        }

        [HttpGet("ReqWaiting/{id}")]
        public ActionResult ReqWaiting(string id)
        {
            var reqWaiting = from U in myContext.Users
                             join R in myContext.RequestItems on U.Id equals R.AccountId
                             join S in myContext.Statuses on R.StatusId equals S.Id
                             where U.Id == id && R.StatusId == 3 
                             select new
                             {
                                 Id = U.Id,
                                 Name = U.FirstName + " " + U.LastName,
                                 Status = S.Name
                             };
            return Ok(reqWaiting);
        }

        [HttpGet("ReqApprove/{id}")]
        public ActionResult ReqApprove(string id)
        {
            var reqApprove = from U in myContext.Users
                             join R in myContext.RequestItems on U.Id equals R.AccountId
                             join S in myContext.Statuses on R.StatusId equals S.Id
                             where U.Id == id && R.StatusId == 4
                             select new
                             {
                                 Id = U.Id,
                                 Name = U.FirstName + " " + U.LastName,
                                 Status = S.Name
                             };
            return Ok(reqApprove);
        }

        [HttpGet("ReqReject/{id}")]
        public ActionResult ReqReject(string id)
        {
            var reqReject = from U in myContext.Users
                             join R in myContext.RequestItems on U.Id equals R.AccountId
                             join S in myContext.Statuses on R.StatusId equals S.Id
                             where U.Id == id && R.StatusId == 1
                             select new
                             {
                                 Id = U.Id,
                                 Name = U.FirstName + " " + U.LastName,
                                 Status = S.Name
                             };
            return Ok(reqReject);
        }

        [HttpGet("ReqReturn/{id}")]
        public ActionResult ReqReturn(string id)
        {
            var reqReturn = from U in myContext.Users
                             join R in myContext.RequestItems on U.Id equals R.AccountId
                             join S in myContext.Statuses on R.StatusId equals S.Id
                             where U.Id == id && R.StatusId == 2
                             select new
                             {
                                 Id = U.Id,
                                 Name = U.FirstName + " " + U.LastName,
                                 Status = S.Name
                             };
            return Ok(reqReturn);
        }
    }

}
