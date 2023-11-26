using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Facebook;
using Figure2.Models;
using GoogleAuthentication.Services;
using Newtonsoft.Json;

namespace Figure2.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        dbDataContext db = new dbDataContext();
        [HttpGet]
        public ActionResult DangNhap()
        {
            var clientId = "3743770010-38l1rrvst136k33lg704nhjs203gci9p.apps.googleusercontent.com";
            var url = "https://localhost:44321/User/LoginGoogle";
            var response = GoogleAuth.GetAuthUrl(clientId, url);

            ViewBag.response = response;

            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = "676598567944788",
                redirect_uri = "https://localhost:44321/User/LoginFacebook",
                scope = "public_profile,email"



            });

            ViewBag.Urll = loginUrl;
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection f,string url)
        {

            var sTenDN = f["Username"];
            var sMatKhau = f["Password"];
          
            
            if (String.IsNullOrEmpty(sTenDN))
            {
                ViewData["Err1"] = "Bạn chưa nhập tên đăng nhập (;-;)";

            }
            else if (String.IsNullOrEmpty(sMatKhau))
            {
                ViewData["Err2"] = "Phải nhập mật khẩu ";
            }
            else
            {

                NguoiDung kh = db.NguoiDungs.SingleOrDefault(n => n.taiKhoan == sTenDN && n.matKhau == GetMD5(sMatKhau));
                if (kh != null)
                {
                    ViewBag.ThongBao = "dang nhap thanh cong";
                    Session["TaiKhoan"] = kh;
                    Session["ten"] = kh.tenNguoiDung;
                    Session["id"] = kh.idNguoiDung;
                    Session["email"] = kh.email;
                    Session["MatKhau"] = sMatKhau;
                    if (f["remember"].Contains("true"))
                    {
                        Response.Cookies["Username"].Value = sTenDN;
                        Response.Cookies["Password"].Value = sMatKhau;
                        Response.Cookies["Username"].Expires = DateTime.Now.AddDays(1);
                        Response.Cookies["Password"].Expires = DateTime.Now.AddDays(1);
                    }
                    else
                    {
                        Response.Cookies["Username"].Expires = DateTime.Now.AddDays(-1);
                        Response.Cookies["Password"].Expires = DateTime.Now.AddDays(-1);
                    }
                    if(url==null)
                    return RedirectToAction("Index", "Figure");
                    else
                        return Redirect(url);
                }
                else
                {
                    ViewBag.ThongBao = "ten dang nhap hoac mk k dung";

                }

            }
            Session["XLDangNhap"] = true;
            return View();
        }
        public async Task<ActionResult> LoginGoogle(string code)
        {
            var clientId = "3743770010-38l1rrvst136k33lg704nhjs203gci9p.apps.googleusercontent.com";
            var url = "https://localhost:44321/User/LoginGoogle";
            var clientSecret = "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";
            var token = await GoogleAuth.GetAuthAccessToken(code, clientId, clientSecret, url);
            var userProfile = await GoogleAuth.GetProfileResponseAsync(token.AccessToken.ToString());

            var googleUser = JsonConvert.DeserializeObject<GoogleProfile>(userProfile);
            if (userProfile != null)
            {
                Session["LoginGF"] = userProfile;
                Session["HoTenGF"] = googleUser.Name;


                return RedirectToAction("Index", "Figure");
            }
            return View("DangNhap");
        }
        public ActionResult LoginFacebook(string code)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Get("/oauth/access_token", new
            {
                client_id = "676598567944788",
                client_secret = "c591d7d5537e33229234fe971d9414a2",
                redirect_uri = "https://localhost:44321/User/LoginFacebook",
                code = code

            });
            fb.AccessToken = result.access_token;
            dynamic me = fb.Get("/me?fields=name,email");
            string name = me.name;

            Session["LoginGF"] = me;
            Session["HoTenGF"] = name;


            return RedirectToAction("Index", "Figure");

        }
        [HttpGet]
        public ActionResult DangKy()
        {

            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult DangKy(FormCollection f, NguoiDung nd)
        {
            var sHoTen = f["tenNguoiDung"];
            var sTaiKhoan = f["taiKhoan"];
            var sMatKhau = f["password"];
            var sMatKhauNhapLai = f["nlPassword"];
            var sDiaChi = f["diaChi"];
            var sEmail = f["email"];
            var sDienThoai = f["sdt"];
          
           
            if (String.IsNullOrEmpty(sHoTen))
            {
                ViewData["err1"] = "Họ tên không được rỗng";
            }
            else if (String.IsNullOrEmpty(sTaiKhoan))
            {
                ViewData["err2"] = "Ten dang nhap không được rỗng";
            }
            else if (String.IsNullOrEmpty(sMatKhau))
            {
                ViewData["err3"] = "Phai nhap mat khau";
            }
            else if (string.IsNullOrEmpty(sMatKhauNhapLai))
            {
                ViewData["err4"] = "Phải nhập lại nhập khẩu";
            }
            else if (sMatKhau != sMatKhauNhapLai)
            {
                ViewData["err4"] = "Nhập lại mật khẩu không khớp";
            }
            else if (String.IsNullOrEmpty(sEmail))
            {
                ViewData["err5"] = "Email không được rỗng";
            }
            else if (String.IsNullOrEmpty(sDienThoai))
            {
                ViewData["err6"] = "So Dien Thoai không được rỗng";
            }

            else if (db.NguoiDungs.SingleOrDefault(n => n.taiKhoan == sTaiKhoan) != null)
            {
                ViewBag.ThongBao = "Tên đăng nhập đã tồn tại.";
            }
            else if (db.NguoiDungs.SingleOrDefault(n => n.email == sEmail) != null)
            {
                ViewBag.ThongBao = "Email đã được sử dụng.";
            }
            else
            {
                nd.tenNguoiDung = sHoTen;
                nd.taiKhoan = sTaiKhoan;
                nd.matKhau = GetMD5(sMatKhau);
                nd.email = sEmail;
                nd.diaChi = sDiaChi;
                nd.soDienThoai = sDienThoai;              
                db.NguoiDungs.InsertOnSubmit(nd);
                db.SubmitChanges();
                
                return Redirect("DangNhap");
            }
            return View();
        }
        public ActionResult DangXuat(string url)
        {
            Session["abc"] = null;
            Session["TaiKhoan"]=null;
            Session["LoginGF"] = null;
            if (url == null)
                return RedirectToAction("Index", "Figure");
            else
                return Redirect(url);
        }
        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;
            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");
            }
            return byte2String;
        }
        public ActionResult TTUser()
        {
          
            var nd = db.NguoiDungs.SingleOrDefault(n => n.idNguoiDung == (int)Session["id"]);
            if (nd == null)
            {
               
              
                return RedirectToAction("DangNhap");
            }
            return View(nd);
        }
        [HttpGet]
        public ActionResult Edit()
        {
            var nd = db.NguoiDungs.SingleOrDefault(n => n.idNguoiDung == (int)Session["id"]);
            if (nd != null)
            {
                return View(nd);

            }


            return RedirectToAction("DangNhap");
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(FormCollection f)
        {
            var nd = db.NguoiDungs.SingleOrDefault(n => n.idNguoiDung == (int)Session["id"]);
            if (nd != null)
            {

                if (ModelState.IsValid)
                {

                    nd.tenNguoiDung = f["tenNguoiDung"];
                    nd.email = f["email"];
                    nd.diaChi = f["diaChi"];
                    nd.matKhau = GetMD5(f["matkhau"]);
                    nd.taiKhoan = f["taiKhoan"];
                    nd.soDienThoai = f["soDienThoai"];
                    Session["MatKhau"] = f["matkhau"];
                    db.SubmitChanges();
                    return RedirectToAction("TTUser");

                }

                return View(nd);
            }
            return RedirectToAction("DangNhap");

        }


        public void SendConfirmationEmail(string toAddress, string resestCode)
        {
            var verifyUrl = "/User/" + "ResetPassword" + "/" + resestCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);
            string subject = "Đặt lại mật khẩu";
            string body = "Bấm vài link bên dưới để đặt lại mật khẩu" + "<br/><br><a href=" + link + ">Đặt lại mật khẩu</a> ";
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

        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]

        public ActionResult ForgotPassword(string EmailID)
        {
            string maessage = "";

            using (dbDataContext db = new dbDataContext())
            {
                var account = db.NguoiDungs.Where(a => a.email == EmailID).FirstOrDefault();
                if (account != null)
                {
                    string resestCode = Guid.NewGuid().ToString();
                    SendConfirmationEmail(account.email, resestCode);

                    account.ResetCode = resestCode;
                    db.SubmitChanges();
                    return RedirectToAction("GuiEmailThanhCong");
                }
                else
                {
                    maessage = "Khong tim thay tai khoan";
                }

            }
            ViewBag.Message = maessage;
            return View();
        }
        public ActionResult ResetPassword(string id)
        {

            var user = db.NguoiDungs.Where(a => a.ResetCode == id).FirstOrDefault();
            if (user != null)
            {
                ResetPassword model = new ResetPassword();

                model.ResetCode = id;
                return View(model);
            }
            else
            {
                return HttpNotFound();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPassword model, FormCollection f)
        {
            var mk = f["NewPassword"];
            var XNmk = f["ConfirmPassword"];
            var message = "";
            if (ModelState.IsValid)
            {
                var user = db.NguoiDungs.Where(a => a.ResetCode == model.ResetCode).FirstOrDefault();
                if (user != null)
                {
                    user.matKhau = GetMD5(f["NewPassword"]);
                    user.ResetCode = "";
                    db.SubmitChanges();
                    message = "Doi mat khau thanh cong";
                    return RedirectToAction("DoiMKThanhCong");
                }
                else
                {
                    message = "Khong ton tai";
                }
            }
            ViewBag.Message = message;
            return View(model);
        }
        public ActionResult DoiMKThanhCong()
        {
            return View();
        }
        public ActionResult GuiEmailThanhCong()
        {
            return View();
        }
    }
}