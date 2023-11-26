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
        public ActionResult TimKiem(string q)
        {
           
            if (string.IsNullOrEmpty(q))
            {
                
                return View(new List<Product>());
            }

            string qLower = q.ToLower(); 

           
            List<Product> listSach = db.Products.Where(ten => ten.tenSanPham.ToLower().Contains(qLower) ).ToList();

            return View(listSach);
        }

    }
}