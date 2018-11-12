using SHPUD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.Core.Objects;
using System.Web.Mvc;
using System.Net;

namespace SHPUD.Controllers
{
    [HandleError()]
    public class AdminController : Controller
    {
        CommonController com = new CommonController();
        private SHPUDEntities1 db = new SHPUDEntities1();
        // GET: Admin
        public ActionResult Index()
        {
            if (Session["userid"] == null)
            {
                return Redirect(Url.Content("~/User/Login"));

            }
            else
            {
                if (Session["role"].ToString() == "admin")
                {
                    
                    return View();
                }
                else
                {
                    throw new Exception("Unauthorized");
                }
            }
                
        }
        [HandleError()]
        public ActionResult Dashboard()
        {
            if (Session["userid"] == null)
            {
                return Redirect(Url.Content("~/User/Login"));

            }
            else
            {
                int user = int.Parse(Session["userid"].ToString());
                var username = db.TBL_USER.Where(x => x.USER_ID == user).Select(x => x.NAME).FirstOrDefault();
                string[] name = username.Split(' ');
                ViewBag.name = name[0];
                ViewBag.doctor = db.TBL_DOCTOR.Count();
                ViewBag.hospital = db.TBL_HOSPITAL.Count();
                ViewBag.disease = db.TBL_DISEASE.Count();
                ViewBag.user = db.TBL_USER.Count();
                return PartialView();
            }

        }
        [HandleError()]
        public PartialViewResult addDisease()
        {
            return PartialView();
        }
        public PartialViewResult manageDisease()
        {
            var list = db.TBL_USER;
            return PartialView(list);
        }
        [HttpGet]
        [HandleError()]
        public ActionResult addUser()
        {
            var userModel = new TBL_USER();
            return PartialView(userModel);
        }
        [HttpPost]
        [HandleError()]
        public ActionResult addUser(TBL_USER model)
        {

            if (ModelState.IsValid)
            {
                //var result = db.TBL_DISEASE.Create(model)
                model.USER_ID = db.TBL_USER.Add(model).USER_ID;
                model.PASSWORD = com.EncodePass(model.PASSWORD, "shpud");
                db.SaveChanges();


                if (model.USER_ID > 0)
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
        [HandleError()]
        public PartialViewResult manageUser()
        {
            var list = db.TBL_USER.Where(x=>x.ROLE =="admin");
            return PartialView(list);
        }
        [HttpGet]
        [HandleError()]
        public PartialViewResult editUser(int id)
        {
            var model = db.TBL_USER.Where(X => X.USER_ID == id).FirstOrDefault();
            return PartialView(model);
        }
        [HandleError()]
        [HttpPost]
        public ActionResult editUser(HttpPostedFile filename, TBL_USER model)
        {

            if (ModelState.IsValid)
            {
                try { 
                var OldModel = db.TBL_USER.Where(X => X.USER_ID == model.USER_ID).FirstOrDefault();
                OldModel.NAME = model.NAME;
                OldModel.ADDRESS = model.ADDRESS;
                OldModel.GENDER = model.GENDER;
                OldModel.DOB = model.DOB;
                OldModel.CONTACT = model.CONTACT;
                OldModel.IMAGE = model.IMAGE;
                    if (filename != null)
                    {
                        string pic = System.IO.Path.GetExtension(filename.FileName);
                        string name = Guid.NewGuid().ToString() + pic;
                        string path = System.IO.Path.Combine(
                                               Server.MapPath("~/Contents/upload/Doctor"), name);
                        // file is uploaded
                        filename.SaveAs(path);
                        OldModel.IMAGE = name;


                    }
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

        [HandleError()]
        public ActionResult deleteUser(int id)
        {
            try
            {
                if(id.ToString()!=null)
                {
                    var model = db.TBL_USER.Where(x => x.USER_ID == id).FirstOrDefault();
                    db.TBL_USER.Remove(model);
                    db.SaveChanges();
                    return Json(new { success = true, error = false, successMessage = "Successfully Deleted" });
                }
                else
                    return Json(new { success = false, error = true, errorMessage = "Choose User" });

            }
            catch(Exception ex)
            {
                return Json(new { success = false, error = true, errorMessage = ex.ToString() });
            }
            
        }
        [HandleError()]
        public PartialViewResult changePassword()
        {
            return PartialView();
        }

        public ActionResult loginProcess()
        {

            return View();
        }
        
        public ActionResult Messages()
        {
            var message = (from p in db.CONTACT_EMAILS
                           select new { p }).ToList();
            List<CONTACT_EMAILS> con = new List<CONTACT_EMAILS>();
            foreach(var item in message)
            {
                CONTACT_EMAILS c = new CONTACT_EMAILS();
                c.ID = item.p.ID;
                c.Name = item.p.Name;
                c.Message = item.p.Message;
                c.Email = item.p.Email;
                c.Phone = item.p.Phone;
                con.Add(c);

            }
            return PartialView(con);
        }
    

    }
}