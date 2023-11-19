using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Figure2.Models;

namespace Figure2.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        dbDataContext db = new dbDataContext();
        [HttpGet]
        public ActionResult DangNhap()
        {
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
                    Session["HoTen"] = kh.tenNguoiDung;
                    Session["id"] = kh.idNguoiDung;

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
            Session.Clear();
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
    }
}