using Figure2.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows.Media.Media3D;

namespace Figure2.Areas.Admin.Controllers
{
    public class SliderController : Controller
    {
        dbDataContext db = new dbDataContext();
        // GET: Admin/Slider
        public ActionResult Index(int? page)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            int iPageNum = (page ?? 1);
            int iPageSize = 7;
            return View(db.Sliders.ToList().OrderBy(n => n.maHinh).ToPagedList(iPageNum, iPageSize));
        }
        public ActionResult Details(int id)
        {
            var slider = db.Sliders.SingleOrDefault(n => n.maHinh == id);
            if (slider == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(slider);
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var slider = db.Sliders.SingleOrDefault(n => n.maHinh == id);
            if (slider == null)
            {
                Response.StatusCode = 404;
                return null;

            }
            return View(slider);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int id, FormCollection f)
        {
            var slider = db.Sliders.SingleOrDefault(n => n.maHinh == id);
            if (slider == null)
            {
                Response.StatusCode = 404;
                return null;

            }
            db.Sliders.DeleteOnSubmit(slider);
            db.SubmitChanges();
            return RedirectToAction("Index");

        }
        [HttpGet]
        public ActionResult Create()
        {
           
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(HttpPostedFileBase upload, int x, int y, int width, int height)
        {
            Slider slider = new Slider();
            try
            {
                // Check if a file was uploaded
                if (upload != null && upload.ContentLength > 0)
                {
                    /*  string croppedImageFileName = "cropped_image.png";*/
                    // Save the uploaded file to a temporary location
                    string tempImagePath = Server.MapPath("~/image.jpg");
                    upload.SaveAs(tempImagePath);
                    string sFileName = Path.GetFileName(upload.FileName);
                    // Perform cropping and other image processing here using the provided dimensions

                    string croppedImagePath = Path.Combine(Server.MapPath("~/img"), sFileName);

                    using (var originalImage = Image.FromFile(tempImagePath))
                    using (var croppedImage = new Bitmap(width, height))
                    using (var graphics = Graphics.FromImage(croppedImage))
                    {
                        graphics.DrawImage(originalImage, new Rectangle(0, 0, width, height), new Rectangle(x, y, width, height), GraphicsUnit.Pixel);
                        croppedImage.Save(croppedImagePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                    }

                    // Clean up: Delete the temporary uploaded file
                    System.IO.File.Delete(tempImagePath);
                   
                       
                        slider.anh = sFileName;
                        db.Sliders.InsertOnSubmit(slider);
                        db.SubmitChanges();
                        return RedirectToAction("Index");                  
                }
                else
                {
                    ViewBag.Message = "No file uploaded.";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"Error: {ex.Message}";
            }

            return View();
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var slider = db.Sliders.SingleOrDefault(n => n.maHinh == id);
            if (slider == null)
            {
                Response.StatusCode = 404;
                return null;
            }
          
            return View(slider);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(HttpPostedFileBase upload, int x, int y, int width, int height,FormCollection f)
        {
            var slider = db.Sliders.SingleOrDefault(n => n.maHinh == int.Parse(f["maHinh"]));
            try
            {
                // Check if a file was uploaded
                if (upload != null && upload.ContentLength > 0)
                {
                    /*  string croppedImageFileName = "cropped_image.png";*/
                    // Save the uploaded file to a temporary location
                    string tempImagePath = Server.MapPath("~/image.jpg");
                    upload.SaveAs(tempImagePath);
                    string sFileName = Path.GetFileName(upload.FileName);
                    // Perform cropping and other image processing here using the provided dimensions

                    string croppedImagePath = Path.Combine(Server.MapPath("~/img"), sFileName);

                    using (var originalImage = Image.FromFile(tempImagePath))
                    using (var croppedImage = new Bitmap(width, height))
                    using (var graphics = Graphics.FromImage(croppedImage))
                    {
                        graphics.DrawImage(originalImage, new Rectangle(0, 0, width, height), new Rectangle(x, y, width, height), GraphicsUnit.Pixel);
                        croppedImage.Save(croppedImagePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                    }

                    // Clean up: Delete the temporary uploaded file
                    System.IO.File.Delete(tempImagePath);


                    if (ModelState.IsValid)
                    {
                        if (upload != null)
                        {
                           
                          
                            if (!System.IO.File.Exists(croppedImagePath))
                            {
                                upload.SaveAs(croppedImagePath);
                            }
                            slider.anh = sFileName;

                        }

                        db.SubmitChanges();
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    ViewBag.Message = "No file uploaded.";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"Error: {ex.Message}";
            }
            
            return View(slider);
        }
    }
}