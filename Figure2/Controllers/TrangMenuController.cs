using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Figure2.Controllers
{
    public class TrangMenuController : Controller
    {
        // GET: TrangMenu
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GioiThieu()
        {
            return View();
        }
        public ActionResult HuongDan()
        {
            return View();
        }
        public ActionResult LienHe()
        {
            return View();
        }
    }
}