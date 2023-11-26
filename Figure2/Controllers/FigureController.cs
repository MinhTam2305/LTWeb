using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Figure2.Models;
using PagedList;

namespace Figure2.Controllers
{
    public class FigureController : Controller
    {
        // GET: Figure
        dbDataContext db = new dbDataContext();
        public ActionResult Index()
        {
            var product = from s in db.Products select s;
            return View(product);
        }
        public ActionResult ItemSanPhamCoSan(int? page)
        {
            int iPageNum = (page ?? 1);
            int iPageSize = 8;
            var product = from s in db.Products where s.danhMuc == 1 select s;
            return View(product.ToPagedList(iPageNum, iPageSize));
            /*  var products = from s in db.Products
                             where s.danhMuc == 1
                             orderby s.Gia descending
                             select s;*/
            /*  sắp xếp từ cao đến thấp*/
            /*  var products = from s in db.Products
                            where s.danhMuc == 1
                            orderby s.Gia ascending 
                            select s;*/

        }
        public ActionResult ItemSanPhamOrder(int? page)
        {
            int iPageNum = (page ?? 1);
            int iPageSize = 8;
            var product = from s in db.Products where s.danhMuc == 2 select s;
            return View(product.ToPagedList(iPageNum, iPageSize));
        }
        public ActionResult ItemNendoroid(int? page)
        {
            int iPageNum = (page ?? 1);
            int iPageSize = 8;
            var product = from s in db.Products where s.danhMuc == 3 select s;
            return View(product.ToPagedList(iPageNum, iPageSize));
        }
        public ActionResult HeaderPartial()
        {
            return View();
        }
        public ActionResult SliderPartial()
        {
            return View();
        }

        public ActionResult HinhSlider()
        {
            var item = from c in db.Sliders select c;
            return PartialView(item);
        }

        public ActionResult HangCoSan(int id)
        {
            var product = from s in db.Products where s.danhMuc == id select s;
            return View(product);

        }
     
        public ActionResult ChiTietFigure(int id)
        {
            var products = from s in db.Products
                       where s.maSanPham == id
                       select s;
            return View(products.Single());
        }
        public ActionResult danhMucHinhPatrial(int id)
        {
            var item = from c in db.danhMucHinhs where c.maProduct==id select c;
            return PartialView(item);
        }
        public ActionResult ItemPatrial()
        {
            var item = from c in db.Categories select c;
            return PartialView(item);
        }
        public ActionResult FeekBacK(int id, int? Page)
        {
            var item = from c in db.Feedbacks where c.maSanPham == id select c;
            if (item.Any())
            {
                ViewBag.FeekBack = "Chưa có đánh giá";
            }
            int iPageNum = (Page ?? 1);
            int iPageSize = 3;
            ViewBag.MaSanPham = id;
            return PartialView(item.ToPagedList(iPageNum, iPageSize));
        }


        public ActionResult AddFeekback(int id,string url)
        {
            Feedback fb = new Feedback();
            string inputValue = Request.Cookies["nd"]?.Value;
          
            int idND = 0;
            if (Session["id"] == null)
                {
                return RedirectToAction("DangNhap", "User");
            }
            else
            {
                idND= (int)Session["id"];
            }
            if (ModelState.IsValid)
            {

                fb.noiDung = inputValue;
                fb.maSanPham = id;
                fb.idNguoiDung = int.Parse(Session["id"].ToString());
                fb.thoiGian=DateTime.Now;

                db.Feedbacks.InsertOnSubmit(fb);
                db.SubmitChanges();
                return Redirect(url);
            }

            return Redirect(url);
        }
    }
}