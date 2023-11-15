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
        public ActionResult DangNhap(FormCollection f)
        {

            var sTenDN = f["tenNguoiDung"];
            var sMatKhau = f["matKhau"];
            
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
                    return RedirectToAction("Index", "Figure");
                }
                else
                {
                    ViewBag.ThongBao = "ten dang nhap hoac mk k dung";

                }

            }
            Session["XLDangNhap"] = true;
            return View();
        }
        public ActionResult DangXuat()
        {
            Session.Clear();
            return RedirectToAction("Index", "Figure");
        }
    }
}