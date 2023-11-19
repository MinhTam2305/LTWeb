using Figure2.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Figure2.Areas.Admin.Controllers
{
    public class NguoiDungController : Controller
    {
        // GET: Admin/NguoiDung
        dbDataContext db = new dbDataContext();
        // GET: Admin/DanhMuc
        public ActionResult Index(int? Page)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            int iPageNum = (Page ?? 1);
            int iPageSize = 7;
            return View(db.NguoiDungs.ToList().OrderBy(n => n.idNguoiDung).ToPagedList(iPageNum, iPageSize));


        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();

        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(NguoiDung nd, FormCollection f)
        {

            var sHoTen = f["tenNguoiDung"];
            var sTaiKhoan = f["taiKhoan"];
            var sMatKhau = f["password"];
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

                return Redirect("Index");
            }
            return View();
        }
      
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var nd = db.NguoiDungs.SingleOrDefault(n => n.idNguoiDung == id);
            if (nd == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            return View(nd);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int id, FormCollection f)
        {
            var nd = db.NguoiDungs.SingleOrDefault(n => n.idNguoiDung == id);
           
            if (nd == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            db.NguoiDungs.DeleteOnSubmit(nd);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var nd = db.NguoiDungs.SingleOrDefault(n => n.idNguoiDung == id);
            if (nd == null)
            {
                Response.StatusCode = 404;
                return null;

            }

            return View(nd);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(FormCollection f)
        {
            var nd = db.NguoiDungs.SingleOrDefault(n => n.idNguoiDung == int.Parse(f["idNguoiDung"]));


            if (ModelState.IsValid)
            {

                nd.tenNguoiDung = f["tenNguoiDung"];
                nd.email = f["email"];
                nd.diaChi = f["diaChi"];
                nd.matKhau = f["matkhau"];
                nd.taiKhoan = f["taiKhoan"];
                nd.soDienThoai = f["soDienThoai"];

                db.SubmitChanges();
                return RedirectToAction("Index");

            }

            return View(nd);

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