using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Figure2.Models;

namespace Figure2.Controllers
{
    public class TimKiemController : Controller
    {
        // GET: TimKiem
        dbDataContext db = new dbDataContext();
        public ActionResult TimKiem(string strSearch)
        {
            ViewBag.Search = strSearch;
            if (!string.IsNullOrEmpty(strSearch))
            {
                //var kq = from s in data.SACHes where s.TenSach.Contains(strSearch) select s;//var kq = db.SACHes…
                var kq = from s in db.Products where s.tenSanPham.Contains(strSearch) || s.Category.tenDanhMuc.Contains(strSearch) select s;
                return View(kq);
            }
            return View();
        }
       
    }
}