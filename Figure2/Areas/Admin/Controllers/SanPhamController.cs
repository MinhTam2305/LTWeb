using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Figure2.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;
namespace Figure2.Areas.Admin.Controllers
{
    public class SanPhamController : Controller
    {
        dbDataContext db = new dbDataContext();
        // GET: Admin/SanPham
        public ActionResult Index(int? Page)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            int iPageNum = (Page ?? 1);
            int iPageSize = 7;
            return View(db.Products.ToList().OrderBy(n => n.maSanPham).ToPagedList(iPageNum, iPageSize));


        }
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.maDanhMuc = new SelectList(db.Categories.ToList().OrderBy(n => n.tenDanhMuc), "maDanhMuc", "tenDanhMuc");
            return View();

        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Product product, FormCollection f, HttpPostedFileBase fFileUpload)
        {
            ViewBag.maDanhMuc = new SelectList(db.Categories.ToList().OrderBy(n => n.tenDanhMuc), "maDanhMuc", "tenDanhMuc");
            if (fFileUpload == null)
            {
                ViewBag.ThongBao = "Hay chon anh bia";
                ViewBag.tenSanPham = f["tenSanPham"];
                ViewBag.MoTa = f["sMoTa"];
                ViewBag.SoLuong = int.Parse(f["SoLuong"]);
                ViewBag.gia = int.Parse(f["gia"]);
                ViewBag.maDanhMuc = new SelectList(db.Categories.ToList().OrderBy(n => n.tenDanhMuc), "maDanhMuc", "tenDanhMuc",int .Parse(f["maDanhMuc"]));
                return View();
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
                    product.moTa = f["sMoTa"];
                    product.anh= sFileName;                  
                    product.soLuong = int.Parse(f["soLuong"]);
                    product.gia = int.Parse(f["gia"]);
                    product.danhMuc = int.Parse(f["maDanhMuc"]);                   
                    db.Products.InsertOnSubmit(product);
                    db.SubmitChanges();
                    return RedirectToAction("Index");
                }
            }

            return View();
        }
        public ActionResult Details(int id)
        {
            var product = db.Products.SingleOrDefault(n => n.maSanPham == id);
            if (product == null)
            {
                Response.StatusCode = 404;
                return null;

            }
            return View(product);
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var product = db.Products.SingleOrDefault(n => n.maSanPham == id);
            if (product == null)
            {
                Response.StatusCode = 404;
                return null;

            }
            return View(product);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int id, FormCollection f)
        {
            var sach = db.Products.SingleOrDefault(n => n.maSanPham == id);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;

            }
            var ctdh = db.OrderDetails.Where(ct => ct.maSanPham == id);


            if (ctdh.Count() > 0)
            {
                ViewBag.ThongBao = "San pham nay co trong bang chi tiet dat hang <br>" + "neu muon xoa thi phai xoa het ma san pham nay trong bang chi tiet dat hang";
                return View(sach);
            }
           
            db.Products.DeleteOnSubmit(sach);
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
            ViewBag.maDanhMuc = new SelectList(db.Categories.ToList().OrderBy(n => n.tenDanhMuc), "maDanhMuc", "tenDanhMuc", product.maSanPham );
           
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