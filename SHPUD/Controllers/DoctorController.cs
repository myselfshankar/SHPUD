using SHPUD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SHPUD.Controllers
{
    public class DoctorController : Controller
    {
        private SHPUDEntities1 db = new SHPUDEntities1();
        // GET: Doctor
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListDoctors()
        {
            var list = db.TBL_DOCTOR;
            return PartialView(list);
        }
        [HttpGet]
        public ActionResult AddDoctor()
        {
            DoctorViewModel dc = new DoctorViewModel();

            return PartialView(dc);
        }
        [HttpPost]
        public ActionResult AddDoctor(HttpPostedFileBase filename,DoctorViewModel model)
        {
            if (ModelState.IsValid)
            {

                if (filename != null)
                {
                    string pic = System.IO.Path.GetExtension(filename.FileName);
                    string name = Guid.NewGuid().ToString() + pic;
                    string path = System.IO.Path.Combine(
                                           Server.MapPath("~/Contents/upload/Doctor"), name);
                    // file is uploaded
                    filename.SaveAs(path);
                    model.DOCTOR_IMAGE = name;


                }
                TBL_DOCTOR tbd = new TBL_DOCTOR();
                tbd.DOCTOR_NAME = model.DOCTOR_NAME;
                tbd.EMAIL = model.EMAIL;
                tbd.ADDRESS = model.ADDRESS;
                tbd.DOCUMENT = model.DOCUMENT;
                tbd.DOCTOR_IMAGE = model.DOCTOR_IMAGE;
                tbd.SPECIALIZATION = model.SPECIALIZATION;
                tbd.WORK_HOSPITAL = model.WORK_HOSPITAL;
                tbd.CONTACT = model.CONTACT;
                tbd.GENDER = model.GENDER;
                tbd.DOCTOR_IMAGE = model.DOCTOR_IMAGE;
                
                tbd.DOCTOR_ID = db.TBL_DOCTOR.Add(tbd).DOCTOR_ID;
                db.SaveChanges();

               // var dtl = db.TBL_DISEASE_DETAILS.Where(X => X.DISEASE_ID == model.DISEASE_ID).FirstOrDefault();
                TBL_DOCTOR_DETAILS dlt = new TBL_DOCTOR_DETAILS();
                dlt.DETAIL_ID = db.TBL_DOCTOR_DETAILS.Add(dlt).DETAIL_ID;
                dlt.DOCTOR_DESCRIPTION = model.DOCTOR_DESCRIPTION;
                dlt.DOCTOR_EDUCATION = model.DOCTOR_EDUCATION;
                dlt.DOCTOR_ID = tbd.DOCTOR_ID;
                dlt.DOCTOR_SPECIALIZATION_DETAILS = model.DOCTOR_SPECIALIZATION_DETAILS;
                
                db.SaveChanges();


                if (tbd.DOCTOR_ID > 0)
                    return Json(new { title="Success",successMessage = "Successfully Registered!" });
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.SeeOther;
                    return Json(new { title="Error",errorMessage = "Something went wrong!" });
                }
            }
            else
            {
                return PartialView(model);
            }
        }
        [HttpGet]
        public ActionResult EditDoctor(int id)
        {
            DoctorViewModel tbd = new DoctorViewModel();
            var model = (from p in db.TBL_DOCTOR
                         join q in db.TBL_DOCTOR_DETAILS on p.DOCTOR_ID equals q.DOCTOR_ID
                         where p.DOCTOR_ID.Equals(id)
                         select new { p, q }).FirstOrDefault();
            tbd.DOCTOR_ID = id;
            tbd.DOCTOR_NAME = model.p.DOCTOR_NAME;
            tbd.EMAIL = model.p.EMAIL;
            tbd.ADDRESS = model.p.ADDRESS;
            tbd.DOCUMENT = model.p.DOCUMENT;
            tbd.DOCTOR_IMAGE = model.p.DOCTOR_IMAGE;
            tbd.SPECIALIZATION = model.p.SPECIALIZATION;
            tbd.WORK_HOSPITAL = model.p.WORK_HOSPITAL;
            tbd.CONTACT = model.p.CONTACT;
            tbd.GENDER = model.p.GENDER;
            tbd.DOCTOR_IMAGE = model.p.DOCTOR_IMAGE;

            tbd.DOCTOR_DESCRIPTION = model.q.DOCTOR_DESCRIPTION;
            tbd.DOCTOR_EDUCATION = model.q.DOCTOR_EDUCATION;
            
            tbd.DOCTOR_SPECIALIZATION_DETAILS = model.q.DOCTOR_SPECIALIZATION_DETAILS;

            return PartialView(tbd);
        }
        [HttpPost]
        public ActionResult EditDoctor(HttpPostedFileBase filename, DoctorViewModel model)
        {
            if (ModelState.IsValid)
            {
                var OldModel = db.TBL_DOCTOR.Where(X => X.DOCTOR_ID == model.DOCTOR_ID).FirstOrDefault();

                try {
                    OldModel.DOCTOR_NAME = model.DOCTOR_NAME;
                    OldModel.ADDRESS = model.ADDRESS;
                    OldModel.DOCUMENT = model.DOCUMENT;
                    OldModel.DOCTOR_IMAGE = model.DOCTOR_IMAGE;
                    OldModel.SPECIALIZATION = model.SPECIALIZATION;
                    OldModel.WORK_HOSPITAL = model.WORK_HOSPITAL;
                    OldModel.EMAIL = model.EMAIL;
                    OldModel.CONTACT = model.CONTACT;
                    OldModel.GENDER = model.GENDER;
                    OldModel.DOCTOR_IMAGE = model.DOCTOR_IMAGE;
                    if (filename != null)
                    {
                        string pic = System.IO.Path.GetExtension(filename.FileName);
                        string name = Guid.NewGuid().ToString() + pic;
                        string path = System.IO.Path.Combine(
                                               Server.MapPath("~/Contents/upload/Doctor"), name);
                        // file is uploaded
                        filename.SaveAs(path);
                        OldModel.DOCTOR_IMAGE = name;


                    }
               
                    db.SaveChanges();
                    var mdl = db.TBL_DOCTOR_DETAILS.Where(X => X.DOCTOR_ID == model.DOCTOR_ID).FirstOrDefault();

                    mdl.DOCTOR_DESCRIPTION = model.DOCTOR_DESCRIPTION;
                    mdl.DOCTOR_EDUCATION = model.DOCTOR_EDUCATION;
                    mdl.DOCTOR_ID = model.DOCTOR_ID;
                    mdl.DOCTOR_SPECIALIZATION_DETAILS = model.DOCTOR_SPECIALIZATION_DETAILS;
                    db.SaveChanges();

                    return Json(new { title = "Success", successMessage = "Successfully Updated !" });
                }
                catch (Exception ex)
                {
                    return Json(new { title = "Error", errorMessage = ex.ToString() });
                }
            }
            else
                return PartialView(model);
            }

        public ActionResult DoctorDetails(int id)
        {
            
            DoctorViewModel tbd = new DoctorViewModel();
            var model = (from p in db.TBL_DOCTOR
                         join q in db.TBL_DOCTOR_DETAILS on p.DOCTOR_ID equals q.DOCTOR_ID
                         where p.DOCTOR_ID.Equals(id)
                         select new { p, q }).FirstOrDefault();
            tbd.DOCTOR_ID = id;
            tbd.DOCTOR_NAME = model.p.DOCTOR_NAME;
            tbd.EMAIL = model.p.EMAIL;
            tbd.ADDRESS = model.p.ADDRESS;
            tbd.DOCUMENT = model.p.DOCUMENT;
            tbd.DOCTOR_IMAGE = model.p.DOCTOR_IMAGE;
            tbd.SPECIALIZATION = model.p.SPECIALIZATION;
            tbd.WORK_HOSPITAL = model.p.WORK_HOSPITAL;
            tbd.CONTACT = model.p.CONTACT;
            tbd.GENDER = model.p.GENDER;
            tbd.DOCTOR_IMAGE = model.p.DOCTOR_IMAGE;
            tbd.views = Convert.ToInt32( model.p.Views);

            tbd.DOCTOR_DESCRIPTION = model.q.DOCTOR_DESCRIPTION;
            tbd.DOCTOR_EDUCATION = model.q.DOCTOR_EDUCATION;

            tbd.DOCTOR_SPECIALIZATION_DETAILS = model.q.DOCTOR_SPECIALIZATION_DETAILS;

            return PartialView(tbd);
        }

        public ActionResult DeleteDoctor(int id)
        {
            try
            {
                if (id.ToString() != null)
                {
                    var model = db.TBL_DOCTOR.Where(x => x.DOCTOR_ID == id).FirstOrDefault();
                    db.TBL_DOCTOR.Remove(model);
                    db.SaveChanges();
                    return Json(new { success = true, error = false, successMessage = "Successfully Deleted" });
                }
                else
                    return Json(new { success = false, error = true, errorMessage = "Choose User" });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = true, errorMessage = ex.ToString() });
            }
        }
        [HttpPost]
        public ActionResult Booknow(string date,string doc,string time,string message)
        {
            if(Session["userid"] == null)
            {
                var refer = Request.UrlReferrer.ToString();
                return Redirect(Url.Content("~/User/Login/refer"));
            }
            else
            {
                try
                {
                    TBL_BOOKING bk = new TBL_BOOKING();
                    bk.Date = date;
                    bk.Doctor_ID = Convert.ToInt32(doc);
                    bk.Time = time;
                    bk.Message = message;
                    bk.User_ID = Convert.ToInt32(Session["userid"].ToString());
                    var model = db.TBL_BOOKING.Where(x => x.Doctor_ID == bk.Doctor_ID && x.Date == bk.Date && x.User_ID == bk.User_ID).ToList();
                    if(model.Count>0)
                    {
                        TempData["errmessage"] = "<div class='alert alert-danger'>You have already placed a booking request for this Doctor on same date.<p> Choose another date.</p> </div>";
                    }
                    else
                    {
                        bk.Doctor_ID = db.TBL_BOOKING.Add(bk).Doctor_ID;
                        db.SaveChanges();

                        TempData["message"] = "<div class='alert alert-success'>" + "<h3>Date: " + date + "</h3><h3>Time: " + time + "</h3><p>Message: " + message + "</p><p>We'll contact you soon for confirmation!</p></div>";

                        ViewBag.status = "data";
                    }
                    
                    return Redirect(Request.UrlReferrer.ToString());
                }
                catch(Exception ex)
                {
                    return Redirect(Request.UrlReferrer.ToString());
                }
                
            }
           
        }
    }
}