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
            // Kiểm tra xem chuỗi tìm kiếm có giá trị không trống
            if (string.IsNullOrEmpty(q))
            {
                // Trả về danh sách sách trống nếu không có chuỗi tìm kiếm
                return View(new List<Product>());
            }

            string qLower = q.ToLower(); // hoặc q.ToUpper() nếu bạn muốn so sánh chữ hoa

            // Thực hiện tìm kiếm không phân biệt chữ hoa và chữ thường
            List<Product> listSach = db.Products.Where(ten => ten.tenSanPham.ToLower().Contains(qLower)).ToList();

            return View(listSach);
        }

    }
}