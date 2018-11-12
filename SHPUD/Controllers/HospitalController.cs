using SHPUD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SHPUD.Controllers
{
    public class HospitalController : Controller
    {
        private SHPUDEntities1 db = new SHPUDEntities1();
        // GET: Hospital
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListHospital()
        {
            var list = db.TBL_HOSPITAL;
            return PartialView(list);
        }
        [HttpGet]
        public ActionResult AddHospital()
        {
            HospitalViewModel hvm = new HospitalViewModel();
            return PartialView(hvm);
        }
        [HttpPost]
        public ActionResult AddHospital(HttpPostedFileBase filename,HospitalViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (filename != null)
                {
                    string pic = System.IO.Path.GetExtension(filename.FileName);
                    string name = Guid.NewGuid().ToString() + pic;
                    string path = System.IO.Path.Combine(
                                           Server.MapPath("~/Contents/upload/Hospital"), name);
                    // file is uploaded
                    filename.SaveAs(path);
                    model.HOSPITAL_IMAGE = name;
                    

                }
                else
                {
                    model.HOSPITAL_IMAGE = "default.jpg";
                }
                //var result = db.TBL_DISEASE.Create(model)
                TBL_HOSPITAL tbl = new TBL_HOSPITAL();
                tbl.HOSPITAL_NAME = model.HOSPITAL_NAME;

                tbl.HOSPITAL_ADDRESS = model.HOSPITAL_ADDRESS;
                tbl.HOSPITAL_IMAGE = model.HOSPITAL_IMAGE;
                tbl.SPECIALIZATION = model.SPECIALIZATION;
                tbl.CONTACT = model.CONTACT;
                tbl.EMAIL = model.EMAIL;
                tbl.HOSPITAL_ID = db.TBL_HOSPITAL.Add(tbl).HOSPITAL_ID;
                db.SaveChanges();

                TBL_HOSPITAL_DETAILS mdl = new TBL_HOSPITAL_DETAILS();
                mdl.DESCRIPTION = model.DESCRIPTION;
                mdl.MORE_INFO = model.MORE_INFO;
                mdl.REMARKS = model.REMARKS;
                mdl.HOSPITAL_ID = tbl.HOSPITAL_ID;
                mdl.DETAIL_ID = db.TBL_HOSPITAL_DETAILS.Add(mdl).DETAIL_ID;
                db.SaveChanges();



                if (tbl.HOSPITAL_ID > 0)
                    return Json(new { successMessage = "Successfully Registered!" });
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.SeeOther;
                    return Json(new { errorMessage = "Something went wrong!" });
                }
            }
            else
            {
                return PartialView(model);
            }
        }
        [HttpGet]
        public ActionResult EditHospital(int id)
        {
            HospitalViewModel tbl = new HospitalViewModel();
            var model = (from p in db.TBL_HOSPITAL
                         join q in db.TBL_HOSPITAL_DETAILS on p.HOSPITAL_ID equals q.HOSPITAL_ID
                         where p.HOSPITAL_ID.Equals(id)
                         select new { p, q }).FirstOrDefault();
            tbl.HOSPITAL_NAME = model.p.HOSPITAL_NAME;

            tbl.HOSPITAL_ADDRESS = model.p.HOSPITAL_ADDRESS;
            tbl.HOSPITAL_IMAGE = model.p.HOSPITAL_IMAGE;
            tbl.SPECIALIZATION = model.p.SPECIALIZATION;
            tbl.CONTACT = model.p.CONTACT;
            tbl.EMAIL = model.p.EMAIL;

            tbl.DESCRIPTION = model.q.DESCRIPTION;
            tbl.MORE_INFO = model.q.MORE_INFO;
            tbl.REMARKS = model.q.REMARKS;
            tbl.HOSPITAL_ID = id;



            return PartialView(tbl);
        }
        [HttpPost]
        public ActionResult EditHospital(HttpPostedFileBase filename, HospitalViewModel model)
        {
            if (ModelState.IsValid)
            {
                var OldModel = db.TBL_HOSPITAL.Where(X => X.HOSPITAL_ID == model.HOSPITAL_ID).FirstOrDefault();
                OldModel.HOSPITAL_NAME = model.HOSPITAL_NAME;

                OldModel.HOSPITAL_ADDRESS = model.HOSPITAL_ADDRESS;
                OldModel.HOSPITAL_IMAGE = model.HOSPITAL_IMAGE;
                OldModel.SPECIALIZATION = model.SPECIALIZATION;
                OldModel.CONTACT = model.CONTACT;
                OldModel.EMAIL = model.EMAIL;
                OldModel.HOSPITAL_IMAGE = model.HOSPITAL_IMAGE;
                if (filename != null)
                {
                    string pic = System.IO.Path.GetExtension(filename.FileName);
                    string name = Guid.NewGuid().ToString() + pic;
                    string path = System.IO.Path.Combine(
                                           Server.MapPath("~/Contents/upload/Hospital"), name);
                    // file is uploaded
                    filename.SaveAs(path);
                    OldModel.HOSPITAL_IMAGE = name;


                }
                try
                {
                    db.SaveChanges();
                    var tbl = db.TBL_HOSPITAL_DETAILS.Where(X => X.HOSPITAL_ID == model.HOSPITAL_ID).FirstOrDefault();
                    tbl.DESCRIPTION = model.DESCRIPTION;
                    tbl.MORE_INFO = model.MORE_INFO;
                    tbl.REMARKS = model.REMARKS;
                    tbl.HOSPITAL_ID = model.HOSPITAL_ID;
                    db.SaveChanges();
                    return Json(new { successMessage = "Successfully Updated !" });
                }
                catch (Exception ex)
                {
                    return Json(new { errorMessage = ex.ToString() });
                }




            }
            else
            {
                return PartialView(model);
            }
        }
        public ActionResult HospitalDetails(int id)
        {

            HospitalViewModel tbl = new HospitalViewModel();
            var model = (from p in db.TBL_HOSPITAL
                         join q in db.TBL_HOSPITAL_DETAILS on p.HOSPITAL_ID equals q.HOSPITAL_ID
                         where p.HOSPITAL_ID.Equals(id)
                         select new { p, q }).FirstOrDefault();
            tbl.HOSPITAL_NAME = model.p.HOSPITAL_NAME;

            tbl.HOSPITAL_ADDRESS = model.p.HOSPITAL_ADDRESS;
            tbl.HOSPITAL_IMAGE = model.p.HOSPITAL_IMAGE;
            tbl.SPECIALIZATION = model.p.SPECIALIZATION;
            tbl.CONTACT = model.p.CONTACT;
            tbl.EMAIL = model.p.EMAIL;

            tbl.DESCRIPTION = model.q.DESCRIPTION;
            tbl.MORE_INFO = model.q.MORE_INFO;
            tbl.REMARKS = model.q.REMARKS;
            tbl.HOSPITAL_ID = id;



            return PartialView(tbl);
        }
        public ActionResult DeleteHospital(int id)
        {
            try
            {
                if (id.ToString() != null)
                {
                    var model = db.TBL_HOSPITAL.Where(x => x.HOSPITAL_ID == id).FirstOrDefault();
                    db.TBL_HOSPITAL.Remove(model);
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

    }
}