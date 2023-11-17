using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Figure2.Models;
using log4net;
using System.Security.Cryptography;

namespace Figure2.Controllers
{
    public class GioHangController : Controller
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(GioHangController));
        // GET: GioHang
        dbDataContext data = new dbDataContext();
        public List<GioHang> LayGioHang()
        {
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang == null)
            {
                lstGioHang = new List<GioHang>();
                Session["GioHang"] = lstGioHang;
            }
            return lstGioHang;
        }
        public ActionResult ThemGioHang(int ms, string url)
        {
            List<GioHang> lstGioHang = LayGioHang();
            GioHang sp = lstGioHang.Find(n => n.iMaFigure == ms);
            if (sp == null)
            {
                sp = new GioHang(ms);
                lstGioHang.Add(sp);
            }
            else
            {
                sp.iSoLuong++;

            }
            return Redirect(url);
        }
        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                iTongSoLuong = lstGioHang.Sum(n => n.iSoLuong);
            }
            return iTongSoLuong;
        }
        private double TongTien()
        {
            double dTongTien = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                dTongTien = lstGioHang.Sum(n => n.dThanhTien);

            }
            return dTongTien;
        }
        public ActionResult GioHang()
        {
            List<GioHang> lstGioHang = LayGioHang();
            if (lstGioHang.Count == 0)
            {
                return RedirectToAction("Index", "Figure");
            }
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(lstGioHang);
        }
        public ActionResult GioHangPartial()
        {
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return PartialView();

        }
        public ActionResult XoaSPKhoiGioHang(int iMaFigure)
        {
            List<GioHang> lstGioHang = LayGioHang();
            GioHang sp = lstGioHang.SingleOrDefault(n => n.iMaFigure == iMaFigure);
            if (sp != null)
            {
                lstGioHang.RemoveAll(n => n.iMaFigure == iMaFigure);
                if (lstGioHang.Count == 0)
                {
                    return RedirectToAction("Index", "Figure");
                }
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult CapNhatGioHang(int iMaFigure, FormCollection f)
        {
            List<GioHang> lstGioHang = LayGioHang();
            GioHang sp = lstGioHang.SingleOrDefault(n => n.iMaFigure == iMaFigure);
            if (sp != null)
            {
                sp.iSoLuong = int.Parse(f["txtSoLuong"].ToString());
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult XoaGioHang()
        {
            List<GioHang> lstGioHang = LayGioHang();
            lstGioHang.Clear();
            return RedirectToAction("Index", "Figure");
        }

        [HttpGet]
        public ActionResult DatHang()
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "User");
            }
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Figure");
            }
            List<GioHang> lstGioHang = LayGioHang();
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(lstGioHang);
        }
        [HttpPost]
        public ActionResult DatHang(FormCollection f)
        {
            DonHang ddh = new DonHang();
            NguoiDung kh = (NguoiDung)Session["TaiKhoan"];
            List<GioHang> lstGioHang = LayGioHang();
            ddh.idNguoiDung = kh.idNguoiDung;
            string hoten = kh.tenNguoiDung.ToString();
            string toAddress = kh.email.ToString();
            ddh.ngayDatHang = DateTime.Now;
            var NgayGiao = string.Format("{0:MM/dd/yyyy}", f["NgayGiao"]);
            var ngaydat = string.Format("{0:MM/dd/yyyy}", ddh.ngayDatHang.ToString());

            ddh.NgayGiao = DateTime.Parse(NgayGiao);
            ddh.TrangThai = 1;
            ddh.DaThanhToan = false;


            data.DonHangs.InsertOnSubmit(ddh);
            data.SubmitChanges();

            DateTime NgayDat = DateTime.Parse(ngaydat);
            DateTime NgayGiaoss = DateTime.Parse(NgayGiao);
            string mdd = ddh.maDonHang.ToString();
            decimal tongtien = 0;


            if (NgayDat > NgayGiaoss)
            {
                return View("a");
            }

            foreach (var item in lstGioHang)
            {

                OrderDetail ctdh = new OrderDetail();
                ctdh.maDonHang = ddh.maDonHang;
                ctdh.maSanPham = item.iMaFigure;
                ctdh.soLuong = item.iSoLuong;
                ctdh.DonGia = (decimal)item.dDonGia;
                decimal dongia = (decimal)item.dDonGia;
                tongtien += dongia;
                data.OrderDetails.InsertOnSubmit(ctdh);
            }


            data.SubmitChanges();

            Session["GioHang"] = null;
            string subject = "XÁC NHẬN ĐẶT HÀNG";
            string body = a(mdd, hoten, ngaydat, NgayGiao, tongtien);
            SendConfirmationEmail(toAddress, subject, body);

            return RedirectToAction("XacNhanDonHang", "GioHang");

        }
        public ActionResult a()

        {
            return View();
        }
        public void SendConfirmationEmail(string toAddress, string subject, string body)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("tam2003hkt@gmail.com", "uliw obgp dqnw eetq"),
                EnableSsl = true
            };

            var mailMessage = new System.Net.Mail.MailMessage
            {
                From = new MailAddress("tam2003hkt@gmail.com"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(toAddress);

            smtpClient.Send(mailMessage);
        }
        public string a(string mdd, string hoten, string ngaydat, string ngaygiao, decimal tongtien)
        {

            string emailBody = @"
            <!DOCTYPE html>
            <html>
            <head>
                <title>Xác nhận đặt hàng</title>
            </head>
            <body>
                <h2>Xác nhận đặt hàng</h2>
                <p>Cảm ơn bạn đã đặt hàng! Dưới đây là thông tin đơn hàng của bạn:</p>
                <ul>
                    <li><strong>Mã đơn hàng:</strong> Ma Don Hang</li>
                    <li><strong>Họ tên khách hàng:</strong> Ho ten</li>
                    <li><strong>Ngày đặt:</strong> Ngay dat</li>
                    <li><strong>Ngày giao:</strong> Ngay giao</li>
                    <li><strong>Tổng giá tiền:</strong> Tong tien</li>
                </ul>
                <p>Chi tiết đơn hàng:</p>
               <ul>
            {OrderDetails}
                </ul>
                <p>Xin vui lòng kiểm tra thông tin đơn hàng của bạn và liên hệ với chúng tôi nếu bạn có bất kỳ câu hỏi hoặc thắc mắc nào.</p>
                <p>Cảm ơn bạn đã sử dụng dịch vụ của chúng tôi!</p>
            </body>
            </html>
            ";
            emailBody = emailBody.Replace("Ma Don Hang", mdd);
            emailBody = emailBody.Replace("Ho ten", hoten);
            emailBody = emailBody.Replace("Ngay dat", ngaydat);
            emailBody = emailBody.Replace("Ngay giao", ngaygiao);
            emailBody = emailBody.Replace("Tong tien", tongtien.ToString("C"));

            List<OrderDetail> orderDetails = LayCTDH(mdd);
            string orderDetailsHtml = "";
            foreach (var detail in orderDetails)
            {

                int ms = (int)detail.maSanPham;
                List<Product> sach = LayHINH(ms);
                foreach (var s in sach)
                {
                    orderDetailsHtml += $"<li><strong>Mã sách: </strong>{detail.maSanPham}</li><li><strong>Số lương: </strong>{detail.soLuong}</li><li><strong>Giá: </strong>{detail.DonGia}</li><br>";
                }
            }

            emailBody = emailBody.Replace("{OrderDetails}", orderDetailsHtml);

            return emailBody;
        }
        public List<OrderDetail> LayCTDH(string madonhang)
        {
            using (var context = new dbDataContext())
            {
                var orderDetails = context.OrderDetails.Where(od => od.DonHang.maDonHang == int.Parse(madonhang)).ToList();

                return orderDetails;
            }
        }
        public List<Product> LayHINH(int maSanPham)
        {
            using (var context = new dbDataContext())
            {
                var SACH = context.Products.Where(od => od.maSanPham == maSanPham).ToList();

                return SACH;
            }
        }
        public ActionResult XacNhanDonHang()
        {
            return View();
        }
        public string UrlPayment()
        {
            var lastRow = data.DonHangs.OrderByDescending(x => x.maDonHang).FirstOrDefault();
            int mdh = lastRow.maDonHang;
            DateTime ngaydat = (DateTime)lastRow.ngayDatHang;
            var ctdh = from s in data.OrderDetails where s.maDonHang == mdh select s;
          
            double tongtien = 0;
            if (ctdh != null)

            {
                tongtien = (double)ctdh.Sum(n => n.DonGia);
            }
          

            string vnp_Returnurl = ConfigurationManager.AppSettings["vnp_Returnurl"]; //URL nhan ket qua tra ve 
            string vnp_Url = ConfigurationManager.AppSettings["vnp_Url"]; //URL thanh toan cua VNPAY 
            string vnp_TmnCode = ConfigurationManager.AppSettings["vnp_TmnCode"]; //Ma định danh merchant kết nối (Terminal Id)
            string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Secret Key



            VnPayLibrary vnpay = new VnPayLibrary();

            vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", (tongtien * 100).ToString());



            vnpay.AddRequestData("vnp_CreateDate", ngaydat.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress());

            vnpay.AddRequestData("vnp_Locale", "vn");

            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang:" + mdh);
            vnpay.AddRequestData("vnp_OrderType", "other");

            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            vnpay.AddRequestData("vnp_TxnRef", mdh.ToString());

          
            string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
            log.InfoFormat("VNPAY URL: {0}", paymentUrl);

            return paymentUrl;

        }
   
        public ActionResult XacNhanThanhToan()
        {


            log.InfoFormat("Begin VNPAY Return, URL={0}", Request.RawUrl);
            if (Request.QueryString.Count > 0)
            {
                string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Chuoi bi mat
                var vnpayData = Request.QueryString;
                VnPayLibrary vnpay = new VnPayLibrary();

                foreach (string s in vnpayData)
                {
                    //get all querystring data
                    if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                    {
                        vnpay.AddResponseData(s, vnpayData[s]);
                    }
                }

                long orderId = Convert.ToInt64(vnpay.GetResponseData("vnp_TxnRef"));
                long vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
                string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
                string vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
                string vnp_SecureHash = Request.QueryString["vnp_SecureHash"];
                string TerminalID = Request.QueryString["vnp_TmnCode"];
                long vnp_Amount = Convert.ToInt64(vnpay.GetResponseData("vnp_Amount")) / 100;
                string bankCode = Request.QueryString["vnp_BankCode"];


                var donhang = data.DonHangs.FirstOrDefault(n => n.maDonHang == orderId);
                var kh = data.NguoiDungs.FirstOrDefault(n => n.idNguoiDung == donhang.idNguoiDung);
                string toAddress = kh.email;
                var NgayGiao = string.Format("{0:MM/dd/yyyy}", donhang.NgayGiao.ToString());
                var ngaydat = string.Format("{0:MM/dd/yyyy}", donhang.ngayDatHang.ToString());
                var hoten = kh.tenNguoiDung;



                bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
                if (checkSignature)
                {
                    if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                    {
                       
                        if (donhang != null)
                        {
                            donhang.DaThanhToan = true;

                            data.SubmitChanges();
                        }
                        ViewBag.Tb = "Giao dịch được thực hiện thành công. Cảm ơn quý khách đã sử dụng dịch vụ";
                        log.InfoFormat("Thanh toan thanh cong, OrderId={0}, VNPAY TranId={1}", orderId, vnpayTranId);
                        string subject = "ĐÃ THANH TOÁN THÀNH CÔNG";
                        string body = a(orderId.ToString(), hoten, ngaydat, NgayGiao, vnp_Amount);
                        SendConfirmationEmail(toAddress, subject, body);
                    }
                    else
                    {                     
                        ViewBag.Tb = "Có lỗi xảy ra trong quá trình xử lý.Mã lỗi: " + vnp_ResponseCode;
                        log.InfoFormat("Thanh toan loi, OrderId={0}, VNPAY TranId={1},ResponseCode={2}", orderId, vnpayTranId, vnp_ResponseCode);
                    }
                    ViewBag.MaWeb = "Mã Website (Terminal ID):" + TerminalID;
                    ViewBag.MaTT = "Mã giao dịch thanh toán:" + orderId.ToString();
                    ViewBag.MaGD = "Mã giao dịch tại VNPAY:" + vnpayTranId.ToString();
                    ViewBag.Tien = "Số tiền thanh toán (VND):" + vnp_Amount.ToString();
                    ViewBag.NH = "Ngân hàng thanh toán:" + bankCode;

                }
                else
                {
                    log.InfoFormat("Invalid signature, InputData={0}", Request.RawUrl);
                    ViewBag.Tb = "Có lỗi xảy ra trong quá trình xử lý";
                }
            }

            return View();
        }
        public ActionResult ThanhToanVnPay()
        {
            string paymentUrl = UrlPayment();
            return Redirect(paymentUrl);          
        }
        public ActionResult ThanhToanKhiNhaHang()
        {
            return View();
        }
    }
}
          