using Figure2.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Figure2.Areas.Admin.Controllers
{
    public class DanhGiaController : Controller
    {
        dbDataContext db = new dbDataContext();
        
        public ActionResult Index(int? Page)
        {
            /* if (Session["Admin"] == null)
             {
                 return RedirectToAction("Login", "Home");
             }*/
            int iPageNum = (Page ?? 1);
            int iPageSize = 7;
            return View(db.Feedbacks.ToList().OrderBy(n => n.maSanPham).ToPagedList(iPageNum, iPageSize));


        }
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.idNguoiDung = new SelectList(db.NguoiDungs.ToList().OrderBy(n => n.tenNguoiDung), "idNguoiDung", "tenNguoiDung");
            ViewBag.maSanPham = new SelectList(db.Products.ToList().OrderBy(n => n.tenSanPham), "maSanPham", "tenSanPham");
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Feedback feedback, FormCollection f)
        {
            ViewBag.idNguoiDung = new SelectList(db.NguoiDungs.ToList().OrderBy(n => n.tenNguoiDung), "idNguoiDung", "tenNguoiDung");
            ViewBag.maSanPham = new SelectList(db.Products.ToList().OrderBy(n => n.tenSanPham), "maSanPham", "tenSanPham");
            if (f["noiDung"] == null)
            {
               
                ViewBag.noiDung = f["noiDung"];
                ViewBag.thoiGian = f["thoiGian"];              
                ViewBag.idNguoiDung = new SelectList(db.NguoiDungs.ToList().OrderBy(n => n.tenNguoiDung), "idNguoiDung", "tenNguoiDung", int.Parse(f["idNguoiDung"]));
                ViewBag.maSanPham = new SelectList(db.Products.ToList().OrderBy(n => n.tenSanPham), "maSanPham", "tenSanPham", int.Parse(f["maSanPham"]));
               
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {

                    feedback.noiDung = f["noiDung"];
                    feedback.thoiGian = Convert.ToDateTime(f["thoiGian"]);
                    feedback.idNguoiDung = int.Parse(f["idNguoiDung"]);
                    feedback.maSanPham = int.Parse(f["maSanPham"]);
                    db.Feedbacks.InsertOnSubmit(feedback);
                    db.SubmitChanges();
                    return RedirectToAction("Index");
                }
            }

            return View();
        }
        public ActionResult Details(int id)
        {
            var feekback = db.Feedbacks.SingleOrDefault(n => n.maFeedback == id);
            if (feekback == null)
            {
                Response.StatusCode = 404;
                return null;

            }
            return View(feekback);
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var feekback = db.Feedbacks.SingleOrDefault(n => n.maFeedback == id);
            if (feekback == null)
            {
                Response.StatusCode = 404;
                return null;

            }
            return View(feekback);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int id, FormCollection f)
        {
            var feekback = db.Feedbacks.SingleOrDefault(n => n.maFeedback == id);
            if (feekback == null)
            {
                Response.StatusCode = 404;
                return null;

            }
          


          
            db.Feedbacks.DeleteOnSubmit(feekback);
            db.SubmitChanges();
            return RedirectToAction("Index");

        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var product = db.Products.SingleOrDefault(n => n.maSanPham == id);
            if (product == null)
            {
                Response.StatusCode = 404;
                return null;

            }
            ViewBag.maDanhMuc = new SelectList(db.Categories.ToList().OrderBy(n => n.tenDanhMuc), "maDanhMuc", "tenDanhMuc", product.maSanPham);

            return View(product);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(FormCollection f, HttpPostedFileBase fFileUpload)
        {
            var product = db.Products.SingleOrDefault(n => n.maSanPham == int.Parse(f["maSanPham"]));
            ViewBag.maDanhMuc = new SelectList(db.Categories.ToList().OrderBy(n => n.tenDanhMuc), "maDanhMuc", "tenDanhMuc", product.maSanPham);
            if (fFileUpload == null)
            {
                ViewBag.ThongBao = "Hay chon anh bia";
                ViewBag.tenSanPham = f["tenSanPham"];
                ViewBag.moTa = f["moTa"];
                ViewBag.soLuong = int.Parse(f["soLuong"]);
                ViewBag.gia = decimal.Parse(f["gia"]);
                ViewBag.maDanhMuc = new SelectList(db.Categories.ToList().OrderBy(n => n.tenDanhMuc), "maDanhMuc", "tenDanhMuc", int.Parse(f["maDanhMuc"]));


            }
            else
            {
                if (ModelState.IsValid)
                {
                    var sFileName = Path.GetFileName(fFileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/img/products"), sFileName);
                    if (!System.IO.File.Exists(path))
                    {
                        fFileUpload.SaveAs(path);
                    }
                    product.tenSanPham = f["tenSanPham"];
                    product.moTa = f["moTa"];
                    product.anh = sFileName;

                    product.soLuong = int.Parse(f["soLuong"]);
                    product.gia = int.Parse(f["gia"]);
                    product.danhMuc = int.Parse(f["maDanhMuc"]);
                    db.SubmitChanges();
                    return RedirectToAction("Index");

                }
            }
            return View(product);

        }
    }
}