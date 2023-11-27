using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Figure2.Controllers
{
    public class TrangMenuController : Controller
    {
        // GET: TrangMenu
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GioiThieu()
        {
            return View();
        }
        public ActionResult HuongDan()
        {
            return View();
        }
        [HttpGet]
        public ActionResult LienHe()
        {
          
            return View();
           
        }
        [HttpPost]
        public ActionResult LienHe(FormCollection f)
        {
            if (f["email"] == null)
            {
                ViewBag.LienHe = "Email không thể để trống";
            }
            else if (f["name"] == null)
            {
                ViewBag.LienHe = "Tên không thể để trống";
            }
            else if (f["message"] == null)
            {
                ViewBag.LienHe = "Nội dung không thể để trống";
            }
            else
            {
                string toAddress = f["email"];
                string subject = "Liên hệ của khách hàng";
                string body = f["message"];
                ViewBag.LienHe = "Đã gửi thành công";
                SendConfirmationEmail(toAddress, subject, body);
            }
            return View();
        }
        public void SendConfirmationEmail(string toAddress, string subject, string body)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("tam2003hkt@gmail.com", "uliw obgp dqnw eetq"),
                EnableSsl = true
            };

            var mailMessage = new System.Net.Mail.MailMessage
            {
                From = new MailAddress("tam2003hkt@gmail.com"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(toAddress);

            smtpClient.Send(mailMessage);
        }
    }
}