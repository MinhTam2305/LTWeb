using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Figure2.Models;

namespace Figure2.Models
{
    public class GioHang
    {
        dbDataContext db = new dbDataContext();
        public int iMaFigure { get; set; }
        public string sTenFigure { get; set; }
        public string sAnh { get; set; }
        public double dDonGia { get; set; }
        public int iSoLuong { get; set; }
        public double dThanhTien
        {
            get { return iSoLuong * dDonGia; }
        }
        public GioHang(int ms)
        {
            iMaFigure = ms;
            Product p = db.Products.Single(n => n.maSanPham == iMaFigure);
            sTenFigure = p.tenSanPham;
            sAnh = p.anh;
            dDonGia = double.Parse(p.gia.ToString());
            iSoLuong = 1;

        }
    }
}