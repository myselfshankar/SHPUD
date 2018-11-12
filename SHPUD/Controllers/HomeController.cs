using SHPUD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SHPUD.Controllers
{
    public class HomeController : Controller
    {
        private SHPUDEntities1 db = new SHPUDEntities1();
        // GET: Home
        public ActionResult Index()
        {
            var model = db.TBL_DOCTOR.OrderByDescending(x=>x.Views).ToList();
            return View(model);
        }
    }
}