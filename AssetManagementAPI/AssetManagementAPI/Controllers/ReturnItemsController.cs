using AssetManagementAPI.Base;
using AssetManagementAPI.Context;
using AssetManagementAPI.Handler;
using AssetManagementAPI.Models;
using AssetManagementAPI.Repositories.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
     
                var returnItm = new ReturnItem
                {
                    RequestItemId = returnItem.RequestItemId,
                    Penalty = returnItem.Penalty,
                    Notes = returnItem.Notes
                };
                myContext.ReturnItems.Add(returnItm);
                myContext.SaveChanges();

                var dataRequest = myContext.RequestItems.Where(R => R.Id == returnItem.RequestItemId).FirstOrDefault();
                var data = myContext.Items.Include(I => I.RequestItems).Where(I => I.Id == dataRequest.ItemId).FirstOrDefault();
                data.Quantity += dataRequest.Quantity;
                myContext.Entry(data).State = EntityState.Modified;
                myContext.SaveChanges();

                dataRequest.StatusId = 2; // Returned
                myContext.Entry(dataRequest).State = EntityState.Modified;
                myContext.SaveChanges();

                var reqItem = myContext.RequestItems.Where(u => u.Id == returnItem.RequestItemId).FirstOrDefault();
                var user = myContext.Users.Where(u => u.Id == reqItem.AccountId).FirstOrDefault();

                var subject = $"Return Asset#{returnItem.RequestItemId} Successful";
                string mailbody = string.Empty;
                mailbody += "<body>";
                mailbody += "<div style='margin: 25px;'>";
                mailbody += "<div style='background-color: #4ac09d; border-radius: 25px;'>";
                mailbody += "<p style='font-weight: bold; font-size: 25px; padding-top: 25px; color: white; text-align: center;'>Asset Management System</p>";
                mailbody += "<div style='background-color:#f0f0f0; border-bottom-right-radius:25px; border-bottom-left-radius:25px; margin-top:-30px;'>";
                mailbody += "<div style='margin-left: 50px; margin-right: 50px; padding-bottom: 50px;'>";
                mailbody += "<h4 style='padding-top: 20px; color: black;'>Hello " + user.FirstName + ",</h4>";
                mailbody += "<p style='color: black;'>Thank you for your confirmation.</p>";
                mailbody += "<p style='color: black;'>Here are the details about your request:</p>";
                mailbody += "<p style='color: black;'>Name        : " + user.FirstName + " " + user.LastName + "</p>";
                mailbody += "<p style='color: black;'>Item Request: " + data.Name + " </p>";
                mailbody += "<p style='color: black;'>Date        : " + dataRequest.StartDate.ToString().Substring(0, 10) + " - " + dataRequest.EndDate.ToString().Substring(0, 10) + "</p>";
                mailbody += "<p style='color: black;'>Fee Penalty : " + returnItm.Penalty + "</p>";
                mailbody += "<p style='color: black;'>Notes       : " + returnItm.Notes + "</p>";
                mailbody += "<p style='color: black;'>If your penalty fee is Rp. 0,- you are free from responsibility for asset requests.</p>";
                mailbody += "<p style='color: black;'>But if you have a penalty charge, you have to pay the penalty immediately.</p>";
                mailbody += "<p style='color: black;'></p>";
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

                return StatusCode(200, new { status = HttpStatusCode.OK, message = "Return Item Berhasil" });
           
        }
    }
}
