using SHPUD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SHPUD.Controllers
{
    public class PredictionController : Controller
    {
        private SHPUDEntities1 db = new SHPUDEntities1();
        // GET: Prediction
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Start()
        {
            return View();
        }

        public ActionResult DiseasePredict(string symp)
        {
            List<string> termsList = new List<string>();
            List<string> templist = symp.Split(',').ToList();
            foreach (var v in templist)
            {
                termsList.Add(v);
            }
            List<TBL_DISEASE> result = new List<TBL_DISEASE>() ;
           foreach(var item in termsList)
            {
               List<TBL_DISEASE> tbl = db.TBL_DISEASE.Where(c=>c.SYMPTOMS.Contains(item)).ToList();
                
                foreach (TBL_DISEASE pak in tbl)
                {
                    result.Add(pak);
                }
            }
            var groups = result;
            
            List<DiseaseCount> final = new List<DiseaseCount>();
            List<TBL_DISEASE> temp = new List<TBL_DISEASE>();
           
            foreach (var item in result)
            {
                DiseaseCount mdl = new DiseaseCount();
                mdl.disease = item;
                var itemm = final.Where(x => x.disease.Equals(item)).ToList();
                if(itemm.Count>0)
                {
                    foreach (var num in itemm)
                    {
                        mdl.count = num.count;
                    }
                    mdl.count = mdl.count + 1;                     
                    final.Add(mdl);
                }
                
                else
                {
                    final.Add(mdl);
                }
            }
           
            var data = final.OrderByDescending(x => x.count).Distinct().ToList();
            List<DiseaseCount> temptext = new List<DiseaseCount>();
            List<DiseaseCount> finaltext = new List<DiseaseCount>();
            
            foreach (var text in data)
            {
                temptext.Add(text);
                var check = temptext.Where(x => x.disease == text.disease).ToList();
                if(check.Count>1)
                {

                }
                else
                {
                    finaltext.Add(text);
                }
            }
            
            List<TBL_DISEASE> list = new List<TBL_DISEASE>();
            foreach (var item in finaltext)
            {
                TBL_DISEASE doc = new TBL_DISEASE();
               
                doc.DISEASE_ID = item.disease.DISEASE_ID;
                
                doc.DISEASE_NAME = item.disease.DISEASE_NAME;
                if(item.count>=4)
                {
                    item.match = "Strong Match";
                }
                else if (item.count == 3)
                {
                    item.match = "Moderate Match";
                }
                else if (item.count <= 2)
                {
                    item.match = "Fair Match";
                }
                else
                {
                    item.match = "Low Match";
                }
                doc.CATEGORY = item.match;
                
               
                list.Add(doc);
            }
            ViewBag.data = list;
            return PartialView();
        }

        public ActionResult details(string did)
        {
            int id = Convert.ToInt32(did);
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
            //return PartialView();
        }

        public ActionResult treatment(string did)
        {
            int id = Convert.ToInt32(did);
            var model = db.TBL_DISEASE_DETAILS.Where(x => x.DISEASE_ID == id).FirstOrDefault();
            return PartialView(model);
        }

    }
}