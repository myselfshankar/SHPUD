using SHPUD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SHPUD.Controllers
{
    public class DiseaseController : Controller
    {
        private SHPUDEntities1 db = new SHPUDEntities1();
        // GET: Disease
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListDisease()
        {
            var list = db.TBL_DISEASE;
            return PartialView(list);
        }
        [HttpGet]
        public ActionResult AddDisease()
        {
            DiseaseViewModel dvm = new DiseaseViewModel();
            return PartialView(dvm);
        }
        [HttpPost]
        public ActionResult AddDisease(HttpPostedFileBase filename, DiseaseViewModel model)
        {
            if (ModelState.IsValid)
            {
                
                //var result = db.TBL_DISEASE.Create(model)
                TBL_DISEASE mdl = new TBL_DISEASE();
                mdl.DISEASE_ID = db.TBL_DISEASE.Add(mdl).DISEASE_ID;
                mdl.DISEASE_NAME = model.DISEASE_NAME;
                mdl.SYMPTOMS = model.SYMPTOMS;
                mdl.CATEGORY = model.CATEGORY;
                mdl.TREATMENT = model.TREATMENT;
                //db.TBL_DISEASE.Add(mdl);
                
                //db.SaveChanges();
                if(db.SaveChanges()>0)
                {
                    
                    TBL_DISEASE_DETAILS dtl = new TBL_DISEASE_DETAILS();
                    dtl.DETAIL_ID = db.TBL_DISEASE_DETAILS.Add(dtl).DISEASE_ID;
                    dtl.DISEASE_ID = mdl.DISEASE_ID;
                    dtl.DISEASE_DESCRIPTION = model.DISEASE_Description;
                    dtl.TREATMENT = model.TREATMENT_DETAILS;
                    dtl.SUGGESTION = model.SUGGESTION;
                    if (filename != null)
                    {
                        string pic = System.IO.Path.GetExtension(filename.FileName);
                        string name = Guid.NewGuid().ToString() + pic;
                        string path = System.IO.Path.Combine(
                                               Server.MapPath("~/Contents/upload/Disease"), name);
                        // file is uploaded
                        filename.SaveAs(path);
                         dtl.IMAGE= name;


                    }
                    //db.TBL_DISEASE_DETAILS.Add(dtl);
                    //dtl.TREATMENT = model.TREATMENT;
                    db.SaveChanges();
                }
                

                if (mdl.DISEASE_ID > 0)
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
        public ActionResult EditDisease(int id)
        {
            DiseaseViewModel mdl = new DiseaseViewModel();
            var model = (from p in db.TBL_DISEASE
                         join q in db.TBL_DISEASE_DETAILS on p.DISEASE_ID equals q.DISEASE_ID
                         where p.DISEASE_ID.Equals(id)
                         select new { p, q }).FirstOrDefault();
            mdl.DISEASE_ID = id;
            mdl.DISEASE_NAME = model.p.DISEASE_NAME;
            mdl.SYMPTOMS = model.p.SYMPTOMS;
            mdl.CATEGORY = model.p.CATEGORY;
            //mdl.TREATMENT = model.p.TREATMENT;

            mdl.IMAGE = model.q.IMAGE;
            mdl.DISEASE_Description = model.q.DISEASE_DESCRIPTION;
            mdl.TREATMENT_DETAILS = model.q.TREATMENT;
            mdl.SUGGESTION = model.q.SUGGESTION;


            return PartialView(mdl);
        }
        [HttpPost]
        public ActionResult EditDisease(HttpPostedFileBase filename, DiseaseViewModel model)
        {
            if (ModelState.IsValid)
            {
               var OldModel = db.TBL_DISEASE.Where(X => X.DISEASE_ID == model.DISEASE_ID).FirstOrDefault();
                OldModel.DISEASE_NAME = model.DISEASE_NAME;
               
                OldModel.SYMPTOMS = model.SYMPTOMS;
                OldModel.CATEGORY = model.CATEGORY;
                //OldModel.TREATMENT = model.TREATMENT;
                try
                {
                    db.SaveChanges();
                    if (1==1)
                    {
                        //TBL_DISEASE_DETAILS dtl = new TBL_DISEASE_DETAILS();
                        var dtl = db.TBL_DISEASE_DETAILS.Where(X => X.DISEASE_ID == model.DISEASE_ID).FirstOrDefault();
                        //dtl.DETAIL_ID = db.TBL_DISEASE_DETAILS.Add(dtl).DETAIL_ID;
                        dtl.DISEASE_ID = model.DISEASE_ID;
                        dtl.DISEASE_DESCRIPTION = model.DISEASE_Description;
                        dtl.TREATMENT = model.TREATMENT_DETAILS;
                        dtl.SUGGESTION = model.SUGGESTION;
                        dtl.IMAGE = model.IMAGE;
                        if (filename != null)
                        {
                            string pic = System.IO.Path.GetExtension(filename.FileName);
                            string name = Guid.NewGuid().ToString() + pic;
                            string path = System.IO.Path.Combine(
                                                   Server.MapPath("~/Contents/upload/Disease"), name);
                            // file is uploaded
                            filename.SaveAs(path);
                            dtl.IMAGE = name;


                        }
                        //db.TBL_DISEASE_DETAILS.Add(dtl);
                        //dtl.TREATMENT = model.TREATMENT;
                        db.SaveChanges();
                    }
                    return Json(new { successMessage = "Successfully Updated !" });
                }
                catch(Exception ex)
                {
                    return Json(new { errorMessage = ex.ToString() });
                }
                


               
            }
            else
            {
                return PartialView(model);
            }

        }
        public ActionResult DiseaseDetails(int id)
        {
           
            DiseaseViewModel mdl = new DiseaseViewModel();
            var model = (from p in db.TBL_DISEASE
                         join q in db.TBL_DISEASE_DETAILS on p.DISEASE_ID equals q.DISEASE_ID
                         where p.DISEASE_ID.Equals(id)
                         select new { p, q }).FirstOrDefault();
            mdl.DISEASE_ID = id;
            mdl.DISEASE_NAME = model.p.DISEASE_NAME;
            mdl.SYMPTOMS = model.p.SYMPTOMS;
            mdl.CATEGORY = model.p.CATEGORY;
            //mdl.TREATMENT = model.p.TREATMENT;
            mdl.IMAGE = model.q.IMAGE;
            mdl.DISEASE_Description = model.q.DISEASE_DESCRIPTION;
            mdl.TREATMENT_DETAILS = model.q.TREATMENT;
            mdl.SUGGESTION = model.q.SUGGESTION;


            return PartialView(mdl);
        }
        public ActionResult DeleteDisease(int id)
        {
            try
            {
                if (id.ToString() != null)
                {
                    var model = db.TBL_DISEASE.Where(x => x.DISEASE_ID == id).FirstOrDefault();
                    db.TBL_DISEASE.Remove(model);
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
        public ActionResult getSymptomsData()
        {
            List<string> termsList = new List<string>();
            var resultset = db.TBL_DISEASE.Select(x => x.SYMPTOMS).ToArray();
            for(int i=0;i<resultset.Length;i++)
            {
                string[] words = resultset[i].Split(',');
                foreach (var word in words)
                {
                    
                    termsList.Add(word.Trim().ToUpper());
                }

            }
            //string str = resultset.ToString();
            return Json(termsList.Distinct(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult listAll()
        {
            DiseaseViewModel model = new DiseaseViewModel();


            return View(model);
        }

        public ActionResult Details(int id)
        {
            var result = (from p in db.TBL_DISEASE
                          join q in db.TBL_DISEASE_DETAILS on p.DISEASE_ID equals q.DISEASE_ID
                          where p.DISEASE_ID.Equals(id)
                          select new { p,q
                          }
                 ).FirstOrDefault();

            DiseaseViewModel doc = new DiseaseViewModel();
           
                doc.DISEASE_NAME = result.p.DISEASE_NAME;
                doc.CATEGORY = result.p.CATEGORY;
                doc.DISEASE_Description = result.q.DISEASE_DESCRIPTION;
                doc.IMAGE = result.q.IMAGE;
                doc.SUGGESTION = result.q.SUGGESTION;
                doc.SYMPTOMS = result.p.SYMPTOMS;

                doc.TREATMENT_DETAILS = result.q.TREATMENT;
            
            return View(doc);
           

        }


    }
}