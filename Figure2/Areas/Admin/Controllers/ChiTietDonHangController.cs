using Figure2.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Figure2.Areas.Admin.Controllers
{
    public class ChiTietDonHangController : Controller
    {
        dbDataContext db = new dbDataContext();
       
        public ActionResult Index(int? Page)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            int iPageNum = (Page ?? 1);
            int iPageSize = 7;
            return View(db.OrderDetails.ToList().OrderBy(n => n.maCTDonHang).ToPagedList(iPageNum, iPageSize));


        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var ctdh = db.OrderDetails.SingleOrDefault(n => n.maCTDonHang == id);
            if (ctdh == null)
            {
                Response.StatusCode = 404;
                return null;

            }
            return View(ctdh);
        }
        public ActionResult Details(int id)
        {
            var ctdh = db.OrderDetails.SingleOrDefault(n => n.maCTDonHang == id);
            if (ctdh == null)
            {
                Response.StatusCode = 404;
                return null;

            }
            return View(ctdh);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int id, FormCollection f)
        {
            var ctdh = db.OrderDetails.SingleOrDefault(n => n.maCTDonHang == id);
            if (ctdh == null)
            {
                Response.StatusCode = 404;
                return null;

            }
           

            db.OrderDetails.DeleteOnSubmit(ctdh);
            db.SubmitChanges();
            return RedirectToAction("Index");

        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var ctdh = db.OrderDetails.SingleOrDefault(n => n.maCTDonHang == id);
            if (ctdh == null)
            {
                Response.StatusCode = 404;
                return null;

            }
            ViewBag.maDonHang = new SelectList(db.DonHangs.ToList().OrderBy(n => n.maDonHang), "maDonHang", "maDonHang", ctdh.maDonHang);
            ViewBag.maSanPham = new SelectList(db.Products.ToList().OrderBy(n => n.maSanPham), "maSanPham", "tenSanPham", ctdh.maSanPham);

            return View(ctdh);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(FormCollection f)
        {
            var ctdh = db.OrderDetails.SingleOrDefault(n => n.maCTDonHang == int.Parse(f["maCTDonHang"]));
            ViewBag.maDonHang = new SelectList(db.DonHangs.ToList().OrderBy(n => n.maDonHang), "maDonHang", "maDonHang", ctdh.maDonHang);
            ViewBag.maSanPham = new SelectList(db.Products.ToList().OrderBy(n => n.maSanPham), "maSanPham", "tenSanPham", ctdh.maSanPham);
            if (f["maDonHang"] == null)
            {
                ViewBag.maDonHang = new SelectList(db.DonHangs.ToList().OrderBy(n => n.maDonHang), "maDonHang", "maDonHang", int.Parse(f["maDonHang"]));
            }
            else if (f["maSanPham"] == null)
            {
                ViewBag.maSanPham = new SelectList(db.Products.ToList().OrderBy(n => n.maSanPham), "maSanPham", "tenSanPham", int.Parse(f["maSanPham"]));
            }
            else if (f["soLuong"] == null)
            {
                ViewBag.soLuong =int.Parse(f["soLuong"]);
            }

            else if (f["DonGia"] == null)
            {
                ViewBag.DonGia = f["DonGia"];
            }            
            else
            {
                if (ModelState.IsValid)
                {
                    ctdh.maDonHang = int.Parse(f["maDonHang"]);
                    ctdh.maSanPham = int.Parse(f["maSanPham"]);
                    ctdh.soLuong = int.Parse(f["soLuong"]);
                    ctdh.DonGia= int.Parse(f["DonGia"]);

                    db.SubmitChanges();
                    return RedirectToAction("Index");

                }
            }
            return View(ctdh);

        }
    }
}