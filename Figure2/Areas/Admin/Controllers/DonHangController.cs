using Figure2.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Figure2.Areas.Admin.Controllers
{
    public class DonHangController : Controller
    {
        dbDataContext db = new dbDataContext();
        // GET: Admin/DonHang
        public ActionResult Index(int? Page)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            int iPageNum = (Page ?? 1);
            int iPageSize = 7;
         
            return View(db.DonHangs.ToList().OrderBy(n => n.maDonHang).ToPagedList(iPageNum, iPageSize));
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var dh = db.DonHangs.SingleOrDefault(n => n.maDonHang == id);
            if (dh == null)
            {
                Response.StatusCode = 404;
                return null;

            }

            ViewBag.idNguoiDung = new SelectList(db.NguoiDungs.ToList().OrderBy(n => n.tenNguoiDung), "idNguoiDung", "tenNguoiDung", dh.maDonHang);
            return View(dh);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(FormCollection f)
        {
            var dh = db.DonHangs.SingleOrDefault(n => n.maDonHang == int.Parse(f["maDonHang"]));
            ViewBag.idNguoiDung = new SelectList(db.NguoiDungs.ToList().OrderBy(n => n.tenNguoiDung), "idNguoiDung", "tenNguoiDung", dh.maDonHang);

            if (f["idNguoiDung"] == null)
            {
                ViewBag.idNguoiDung = new SelectList(db.NguoiDungs.ToList().OrderBy(n => n.tenNguoiDung), "idNguoiDung", "tenNguoiDung", int.Parse(f["idNguoiDung"]));
            }
            else if (f["ngayDatHang"] == null)
            {
                ViewBag.ngayDatHang = Convert.ToDateTime(f["ngayDatHang"]);
            }
            else if (f["NgayGiao"] == null)
            {
                ViewBag.NgayGiao = Convert.ToDateTime(f["NgayGiao"]);
            }

            else if (f["TrangThai"] == null)
            {
                ViewBag.TrangThai = f["TrangThai"];
            }

            else if (f["DaThanhToan"] == null)
            {
                ViewBag.DaThanhToan = f["DaThanhToan"];
            }
            else
            {
                if (ModelState.IsValid)
                {

                    dh.idNguoiDung = int.Parse(f["idNguoiDung"]);
                    dh.ngayDatHang = Convert.ToDateTime(f["ngayDatHang"]);
                    dh.NgayGiao= Convert.ToDateTime(f["NgayGiao"]);
                    dh.TrangThai = int.Parse(f["TrangThai"]);                  
                    dh.DaThanhToan = bool.Parse(f["DaThanhToan"]);
                    db.SubmitChanges();
                    return RedirectToAction("Index");

                }
            }
            return View(dh);

        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var dh = db.DonHangs.SingleOrDefault(n => n.maDonHang == id);
            if (dh == null)
            {
                Response.StatusCode = 404;
                return null;

            }
            return View(dh);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int id, FormCollection f)
        {
            var dh = db.DonHangs.SingleOrDefault(n => n.maDonHang == id);
            if (dh == null)
            {
                Response.StatusCode = 404;
                return null;

            }
            var ctdh = db.OrderDetails.Where(ct => ct.maDonHang == id);


            if (ctdh.Count() > 0)
            {
                ViewBag.ThongBao = "San pham nay co trong bang chi tiet dat hang neu muon xoa thi phai xoa het ma san pham nay trong bang chi tiet dat hang";
                return View(dh);
            }

            db.DonHangs.DeleteOnSubmit(dh);
            db.SubmitChanges();
            return RedirectToAction("Index");

        }
    }
}