using System;
using System.Collections.Generic;
using System.Linq;
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

                NguoiDung kh = db.NguoiDungs.SingleOrDefault(n => n.tenNguoiDung == sTenDN && n.matKhau == sMatKhau);
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
        public ActionResult DangXuat(string url)
        {
            Session.Clear();
            if (url == null)
                return RedirectToAction("Index", "Figure");
            else
                return Redirect(url);
        }
    }
}