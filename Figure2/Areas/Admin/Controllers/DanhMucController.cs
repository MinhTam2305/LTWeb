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
    public class DanhMucController : Controller
    {
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
            return View(db.Categories.ToList().OrderBy(n => n.maDanhMuc).ToPagedList(iPageNum, iPageSize));


        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();

        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Category category, FormCollection f)
        {
           
            if (f["TenDanhMuc"] == null)
            {             
                ViewBag.tenSanPham = f["TenDanhMuc"];
              
              
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {

                    category.tenDanhMuc = f["TenDanhMuc"];
                    db.Categories.InsertOnSubmit(category);
                    db.SubmitChanges();
                    return RedirectToAction("Index");
                }
            }

            return View();
        }
        public ActionResult Details(int id)
        {
            var category = db.Categories.SingleOrDefault(n => n.maDanhMuc == id);
            if (category == null)
            {
                Response.StatusCode = 404;
                return null;

            }
            return View(category);
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var category = db.Categories.SingleOrDefault(n => n.maDanhMuc == id);
            if (category == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            return View(category);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int id, FormCollection f)
        {
            var category = db.Categories.SingleOrDefault(n => n.maDanhMuc == id);
            if (category == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            db.Categories.DeleteOnSubmit(category);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var category = db.Categories.SingleOrDefault(n => n.maDanhMuc == id);
            if (category == null)
            {
                Response.StatusCode = 404;
                return null;

            }
       
            return View(category);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(FormCollection f)
        {
            var category = db.Categories.SingleOrDefault(n => n.maDanhMuc == int.Parse(f["maDanhMuc"]));

          
                if (ModelState.IsValid)
                {

                    category.tenDanhMuc = f["TenDanhMuc"];
               
                    db.SubmitChanges();
                    return RedirectToAction("Index");

                }
            
            return View(category);

        }
    }
}