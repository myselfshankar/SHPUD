using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SHPUD.Models;
using System.Data.Entity.Core.Objects;
using System.Net;
using PagedList;
using PagedList.Mvc;
using System.Net.Mail;
using System.Threading.Tasks;
using System.IO;

namespace SHPUD.Controllers
{
    [HandleError()]
    public class UserController : Controller
    {
        CommonController com = new CommonController();
        private SHPUDEntities1 db = new SHPUDEntities1();
        
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Login(string refer)
        {
            if (Session["userid"] != null)
            {
                if (Session["role"].ToString() == "admin")
                {
                    return Redirect(Url.Content("~/Admin"));
                }
                else
                {
                    
                        return Redirect(Url.Content("~/Home"));
                    
                    
                }


            }
            else
            {

                var userModel = new TBL_USER();
                return View(userModel);


            }

        }
        [HttpPost]
        [HandleError()]
        public ActionResult Login(TBL_USER model)
        {
            try
            {
                var user = db.TBL_USER.Where(x => x.EMAIL == model.EMAIL).FirstOrDefault();
                if (user != null)
                {
                    var enpas = com.EncodePass(model.PASSWORD, "shpud");
                    var userpass = db.TBL_USER.Where(x => x.PASSWORD == enpas && x.EMAIL == model.EMAIL).FirstOrDefault();
                    if (userpass != null)
                    {
                        if (user.STATUS == "Unverified")
                        {
                            Session["id"] = user.USER_ID;
                            return Json(new { success = true, verified = false, error = false, successMessage = "Verification Needed" });
                        }
                        else
                        {
                            
                            int userid = user.USER_ID;
                            Session["userid"] = userid;
                            Session["fullname"] = user.NAME;
                            Session["email"] = user.EMAIL;
                            Session["role"] = user.ROLE;

                            string[] name = Session["fullname"].ToString().Split(' ');
                            Session["firstname"] = name[0];
                            return Json(new { success = true, verified = true, error = false, successMessage = "Logged in Successfully", role = Session["role"].ToString() });

                        }
                        //return RedirectToAction("Dashboard", "Admin", new {userid=user });

                    }
                    else
                    {
                        //Response.StatusCode = (int)HttpStatusCode.SeeOther;
                        return Json(new { success = false, error = true, title = "Password Incorrect", errorMessage = "Please recheck your password and try again! " });
                    }

                }
                else
                {
                    //Response.StatusCode = (int)HttpStatusCode.SeeOther;
                    return Json(new { success = false, error = true, title = "Incorrect Email", errorMessage = "No user found with this email.\n Please rechek your email Address!" });
                }


            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.SeeOther;
                return Json(new { success = false, error = true, title = "Server Error", errorMessage = ex.ToString() });

            }


        }
        [HandleError()]
        public ActionResult UserRegister()
        {
            var userModel = new TBL_USER();
            return View(userModel);
        }
        [HttpPost]
        public async Task<ActionResult> UserRegister(HttpPostedFileBase filename, TBL_USER model)
        {
            if (ModelState.IsValid)
            {
                if (filename != null)
                {
                    string pic = System.IO.Path.GetExtension(filename.FileName);
                    string name = Guid.NewGuid().ToString() + pic;
                    string path = System.IO.Path.Combine(
                                           Server.MapPath("~/Contents/upload/user"), name);
                    // file is uploaded
                    filename.SaveAs(path);
                    model.IMAGE = name;


                }
                ObjectParameter output = new ObjectParameter("V_OUT", typeof(int));

                var enpas = com.EncodePass(model.PASSWORD, "shpud");
                if (model.IMAGE == null)
                    model.IMAGE = "default.jpg";
                model.PASSWORD = enpas;
                //db.P_SET_USER(model.NAME, model.ADDRESS, enpas, model.EMAIL, model.GENDER, model.DOB, model.CONTACT, model.IMAGE, output);

                model.STATUS = "Unverified";
                Random r = new Random();
                int num = r.Next(10000, 999999);
                string link = com.EncodePass(num.ToString(), "token");
                model.TOKEN = num.ToString();
                var email = (from p in db.TBL_USER
                             where p.EMAIL.Equals(model.EMAIL)
                             select new { p.EMAIL }
                             ).FirstOrDefault();  //db.TBL_USER.Where(X => X.EMAIL == model.EMAIL).FirstOrDefault();

                if (email != null)
                {
                    Response.StatusCode = (int)HttpStatusCode.SeeOther;
                    return Json(new { errorMessage = email.EMAIL }); //"User Email already Registered"
                }
                else
                {
                    //email agr
                    link = "Http://shpud.com/User/Verification/" + link;

                    //var body = "<p>Dear: {0} ({1})</p><p>Please Verify Your Email Address:</p><p>Enter this verification Code: {2}</p><p>Or goto this link: {3}</p><br/><p>Regards,</p><p>Smart Health Prediction. SHPUD</p>";
                    //ContactMail(e);
                    await SendEmail(model.EMAIL, link, model.TOKEN, model.NAME);
                    model.USER_ID = db.TBL_USER.Add(model).USER_ID;
                    if (db.SaveChanges() > 0)
                        return Json(new { successMessage = "Successfully Registered!" });

                    else
                    {
                        Response.StatusCode = (int)HttpStatusCode.SeeOther;
                        return Json(new { errorMessage = "Something went wrong!" });
                    }
                }

            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.SeeOther;
                return View(model);
            }
        }

        [HandleError()]
        public async Task SendEmail(string email, string link, string token, string name)
        {
            string FilePath = Server.MapPath("/Contents/template.html");
            StreamReader str = new StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();

            //Repalce [newusername] = signup user name   
            MailText = MailText.Replace("[verifycode]", token);
            //var body = "<p>Dear: {0} ({1})</p><p>Please Verify Your Email Address:</p><p>Enter this verification Code: {2}</p><p>Or goto this link: {3}</p><br/><p>Regards,</p><p>Smart Health Prediction. SHPUD</p>";
            var message = new MailMessage();
            message.To.Add(new MailAddress(email));  // replace with valid value 
            message.From = new MailAddress("noreply@shpud.com");  // replace with valid value
            message.Subject = "Verify Your Emailaddress";
            // message.Body = string.Format(body, name, email, token, link);
            message.Body = MailText;
            message.IsBodyHtml = true;
            int res = 0;
            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = "shpud@tilottama.edu.np",  // replace with valid value
                    Password = "2e@oN;g.tvAA"  // replace with valid value
                };
                smtp.Credentials = credential;
                smtp.Host = "mail.tilottama.edu.np";
                smtp.Port = 587;
                smtp.EnableSsl = false;
                try
                {

                    await smtp.SendMailAsync(message);
                    res = 1;
                    //ViewBag.success = "<div class='alert-success alert'>Your Message Sent Successfully</div>";
                }
                catch (Exception ex)
                {
                    ViewBag.error = "< div class='alert alert-danger'>ex.ToString()</div>";

                }

            }
        }
        [HandleError()]
        public ActionResult GetUserInfo(int id)
        {
            var model = db.TBL_USER.Where(X => X.USER_ID == id).FirstOrDefault();
            return PartialView(model);
        }
        [HandleError()]
        public ActionResult Logout()
        {
            try
            {
                Session.Abandon();
                return Redirect(Url.Content("~/user/login"));
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        [HandleError()]

        public ActionResult listDoctors(int? page, string type, string location, string query)
        {
            List<DoctorViewModel> dc = new List<DoctorViewModel>();
            int pageIndex = 1;
            int pageSize = 4;
            pageIndex = page.HasValue ? Convert.ToInt16(page) : 1;
            if (!string.IsNullOrWhiteSpace(type))
            {
                return View(db.TBL_DOCTOR.Where(X => X.SPECIALIZATION.Contains(type)).ToList().ToPagedList(pageIndex, pageSize));
            }
            else if (location != null)
            {

                return View(db.TBL_DOCTOR.Where(X => X.ADDRESS.Contains(location)).ToList().ToPagedList(pageIndex, pageSize));
            }
            else if (query != null)
            {
                //(from p in db.TBL_DOCTOR
                // join q in db.TBL_DISEASE on p.SPECIALIZATION equals q.CATEGORY
                // where p.EMAIL.Equals("something") || p.DOCUMENT != ""
                // select new {
                //     p, q.DISEASE_ID,
                //     ram = q.DISEASE_NAME
                // }
                // );
                var result = (from p in db.TBL_DOCTOR
                              where p.ADDRESS.Contains(query) || p.DOCTOR_NAME.Contains(query) || p.EMAIL.Contains(query) || p.SPECIALIZATION.Contains(query) || p.WORK_HOSPITAL.Contains(query)
                              select new { p }
                 ).ToList();

                List<TBL_DOCTOR> list = new List<TBL_DOCTOR>();
                foreach (var item in result)
                {
                    TBL_DOCTOR doc = new TBL_DOCTOR();
                    doc.ADDRESS = item.p.ADDRESS;
                    doc.DOCTOR_ID = item.p.DOCTOR_ID;
                    doc.DOCTOR_NAME = item.p.DOCTOR_NAME;
                    doc.DOCTOR_IMAGE = item.p.DOCTOR_IMAGE;
                    doc.DOCUMENT = item.p.DOCUMENT;
                    doc.WORK_HOSPITAL = item.p.WORK_HOSPITAL;
                    doc.CONTACT = item.p.CONTACT;
                    doc.EMAIL = item.p.EMAIL;
                    doc.GENDER = item.p.GENDER;
                    list.Add(doc);
                }
                return View(list.ToPagedList(pageIndex, pageSize));


                // return View(db.TBL_DOCTOR.Where(X => X.ADDRESS.Contains(query) || ).ToList().ToPagedList(pageIndex, pageSize));
            }
            else
            {
                return View(db.TBL_DOCTOR.ToList().ToPagedList(pageIndex, pageSize));
            }

        }
        [HandleError()]
        public ActionResult searchList(int? page, string searchKey, string searchfor)
        {
            int pageIndex = 1;
            int pageSize = 4;
            pageIndex = page.HasValue ? Convert.ToInt16(page) : 1;
            if (searchfor == "doctor")
            {
                var result = (from p in db.TBL_DOCTOR
                              where p.ADDRESS.Contains(searchKey) || p.DOCTOR_NAME.Contains(searchKey) || p.EMAIL.Contains(searchKey) || p.SPECIALIZATION.Contains(searchKey) || p.WORK_HOSPITAL.Contains(searchKey)
                              select new { p }
                 ).ToList();

                List<TBL_DOCTOR> list = new List<TBL_DOCTOR>();
                foreach (var item in result)
                {
                    TBL_DOCTOR doc = new TBL_DOCTOR();
                    doc.ADDRESS = item.p.ADDRESS;
                    doc.DOCTOR_ID = item.p.DOCTOR_ID;
                    doc.SPECIALIZATION = item.p.SPECIALIZATION;
                    doc.DOCTOR_NAME = item.p.DOCTOR_NAME;
                    doc.DOCTOR_IMAGE = item.p.DOCTOR_IMAGE;
                    doc.DOCUMENT = item.p.DOCUMENT;
                    doc.WORK_HOSPITAL = item.p.WORK_HOSPITAL;
                    doc.CONTACT = item.p.CONTACT;
                    doc.EMAIL = item.p.EMAIL;
                    doc.GENDER = item.p.GENDER;
                    list.Add(doc);
                }
                return View("~/Views/User/listDoctors.cshtml", list.ToPagedList(pageIndex, pageSize));

            }
            else if (searchfor == "Disease")
            {
                var result = (from p in db.TBL_DISEASE
                              join q in db.TBL_DISEASE_DETAILS on p.DISEASE_ID equals q.DISEASE_ID
                              where p.DISEASE_NAME.Contains(searchKey) || p.CATEGORY.Contains(searchKey) || p.SYMPTOMS.Contains(searchKey)
                              select new
                              {
                                  p,
                                  q
                              }
                 ).ToList();

                List<DiseaseViewModel> list = new List<DiseaseViewModel>();
                foreach (var item in result)
                {
                    DiseaseViewModel doc = new DiseaseViewModel();
                    doc.DISEASE_ID = item.p.DISEASE_ID;
                    doc.DISEASE_NAME = item.p.DISEASE_NAME;
                    doc.CATEGORY = item.p.CATEGORY;

                    doc.SYMPTOMS = item.p.SYMPTOMS;
                    //doc.WORK_HOSPITAL = item.p.WORK_HOSPITAL;
                    //doc.CONTACT = item.p.CONTACT;
                    //doc.EMAIL = item.p.EMAIL;
                    //doc.GENDER = item.p.GENDER;
                    list.Add(doc);
                }
                return View("~/Views/DIsease/listAll.cshtml", list.ToPagedList(pageIndex, pageSize));

            }
            else if (searchfor == "hospital")
            {
                var result = (from p in db.TBL_HOSPITAL
                              where p.HOSPITAL_ADDRESS.Contains(searchKey) || p.HOSPITAL_NAME.Contains(searchKey) || p.SPECIALIZATION.Contains(searchKey)
                              select new { p }
                 ).ToList();

                List<TBL_HOSPITAL> list = new List<TBL_HOSPITAL>();
                foreach (var item in result)
                {
                    TBL_HOSPITAL doc = new TBL_HOSPITAL();
                    doc.HOSPITAL_NAME = item.p.HOSPITAL_NAME;
                    doc.HOSPITAL_ADDRESS = item.p.HOSPITAL_ADDRESS;
                    doc.SPECIALIZATION = item.p.SPECIALIZATION;
                    doc.HOSPITAL_ID = item.p.HOSPITAL_ID;
                    doc.HOSPITAL_IMAGE = item.p.HOSPITAL_IMAGE;

                    list.Add(doc);
                }
                return View("~/Views/User/listHospital.cshtml", list.ToPagedList(pageIndex, pageSize));

            }
            return View();
        }
        [HandleError()]
        public ActionResult faq()
        {
            return View();
        }
        [HandleError()]
        public ActionResult Contact()
        {
            return View();
        }
        [HandleError()]
        public ActionResult listHospital(int? page, string query)
        {

            //List<DoctorViewModel> dc = new List<DoctorViewModel>();
            int pageIndex = 1;
            int pageSize = 5;
            pageIndex = page.HasValue ? Convert.ToInt16(page) : 1;

            if (query != null)
            {

                var result = (from p in db.TBL_HOSPITAL
                              where p.HOSPITAL_ADDRESS.Contains(query) || p.HOSPITAL_NAME.Contains(query) || p.EMAIL.Contains(query) || p.SPECIALIZATION.Contains(query)
                              select new { p }
                 ).ToList();

                List<TBL_HOSPITAL> list = new List<TBL_HOSPITAL>();
                foreach (var item in result)
                {
                    TBL_HOSPITAL doc = new TBL_HOSPITAL();
                    doc.HOSPITAL_NAME = item.p.HOSPITAL_NAME;
                    doc.HOSPITAL_ADDRESS = item.p.HOSPITAL_ADDRESS;
                    doc.HOSPITAL_IMAGE = item.p.HOSPITAL_IMAGE;
                    doc.HOSPITAL_ID = item.p.HOSPITAL_ID;
                    doc.CONTACT = item.p.CONTACT;
                    doc.SPECIALIZATION = item.p.SPECIALIZATION;

                    doc.EMAIL = item.p.EMAIL;

                    list.Add(doc);
                }
                return View(list.ToPagedList(pageIndex, pageSize));


                // return View(db.TBL_DOCTOR.Where(X => X.ADDRESS.Contains(query) || ).ToList().ToPagedList(pageIndex, pageSize));
            }
            else
            {
                return View(db.TBL_HOSPITAL.ToList().ToPagedList(pageIndex, pageSize));
            }
        }
        [HandleError()]

        public ActionResult doctorDetails(int id)
        {
            var md = db.TBL_DOCTOR.Find(id);
            int temp = Convert.ToInt32( md.Views);
            md.Views = temp + 1;
            db.SaveChanges();
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
            tbd.views = Convert.ToInt32(model.p.Views);

            tbd.DOCTOR_DESCRIPTION = model.q.DOCTOR_DESCRIPTION;
            tbd.DOCTOR_EDUCATION = model.q.DOCTOR_EDUCATION;

            tbd.DOCTOR_SPECIALIZATION_DETAILS = model.q.DOCTOR_SPECIALIZATION_DETAILS;
            //var model = db.TBL_DOCTOR.Where(X => X.DOCTOR_ID == id).FirstOrDefault();
            return View(tbd);
        }
        [HandleError()]
        public ActionResult UserProfile()
        {
            int userid = Convert.ToInt32(Session["userid"].ToString());
            var model = db.TBL_USER.Where(X => X.USER_ID == userid).FirstOrDefault();

            return View(model);
        }
        [HandleError()]
        public ActionResult DetailHospital(int id)
        {
            var result = (from p in db.TBL_HOSPITAL
                          join q in db.TBL_HOSPITAL_DETAILS on p.HOSPITAL_ID equals q.HOSPITAL_ID
                          where p.HOSPITAL_ID == id
                          select new
                          {
                              p,
                              q
                          }).FirstOrDefault();

            HospitalViewModel doc = new HospitalViewModel();



            doc.HOSPITAL_NAME = result.p.HOSPITAL_NAME;
            doc.HOSPITAL_ADDRESS = result.p.HOSPITAL_ADDRESS;
            doc.HOSPITAL_IMAGE = result.p.HOSPITAL_IMAGE;
            doc.HOSPITAL_ID = result.p.HOSPITAL_ID;
            doc.CONTACT = result.p.CONTACT;
            doc.SPECIALIZATION = result.p.SPECIALIZATION;
            doc.DESCRIPTION = result.q.DESCRIPTION;
            doc.MORE_INFO = result.q.MORE_INFO;
            doc.REMARKS = result.q.REMARKS;


            doc.EMAIL = result.p.EMAIL;



            return View(doc);
        }
        [HttpPost]
        [HandleError()]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ContactMail(EmailViewModel model)
        {
            var name = model.Firstname + " " + model.Lastname;
            var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
            var message = new MailMessage();
            message.To.Add(new MailAddress("shankarbymail@gmail.com"));  // replace with valid value 
            message.From = new MailAddress("meetmejohn2015@gmail.com");  // replace with valid value
            message.Subject = "New Feedback";
            message.Body = string.Format(body, name, model.Email, model.Message);
            message.IsBodyHtml = true;

            if (model.Firstname == null || model.Email == null)
            {
                ViewBag.error = "<div class='alert alert-danger'>Fill all fields first !</div>";
            }
            else
            {
                CONTACT_EMAILS em = new CONTACT_EMAILS();
                em.Name = name;
                em.Email = model.Email;
                em.Phone = model.phone;
                em.Message = model.Message;
                em.ID = db.CONTACT_EMAILS.Add(em).ID;
                db.SaveChanges();

                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = "shpud@tilottama.edu.np",  // replace with valid value
                        Password = "2e@oN;g.tvAA"  // replace with valid value
                    };
                    smtp.Credentials = credential;
                    smtp.Host = "mail.tilottama.edu.np";
                    smtp.Port = 587;
                    smtp.EnableSsl = false;
                    try
                    {
                        ViewBag.success = "<div class='alert-success alert'>Your Message Sent Successfully. We'll contact you with provided Email address soon. </div>";
                        await smtp.SendMailAsync(message);
                    }
                    catch (Exception ex)
                    {
                        ViewBag.error = "< div class='alert alert-danger'>ex.ToString()</div>";

                    }

                }

            }
            EmailViewModel mv = new EmailViewModel();
            return View("~/Views/User/Contact.cshtml", mv);




        }
        [HandleError()]
        public ActionResult Verify()
        {
            var id = Session["id"].ToString();
            TBL_USER tu = new TBL_USER();
            return View(tu);
        }
        [HttpPost]
        [HandleError()]
        [ValidateAntiForgeryToken]
        public ActionResult Verify(TBL_USER val)
        {
            var id = Convert.ToInt32(Session["id"].ToString());

            var mdl = db.TBL_USER.Where(X => X.USER_ID == id).FirstOrDefault();
            if (val.TOKEN == mdl.TOKEN)
            {
                //verified
                var test = db.TBL_USER.Find(id);
                if (test != null)
                {
                    test.STATUS = "Verified";
                    db.SaveChanges();
                }

                var user = db.TBL_USER.Where(X => X.USER_ID == id).FirstOrDefault();
                int userid = user.USER_ID;
                Session["userid"] = userid;
                Session["fullname"] = user.NAME;
                Session["email"] = user.EMAIL;
                Session["role"] = user.ROLE;

                string[] name = Session["fullname"].ToString().Split(' ');
                Session["firstname"] = name[0];
                if (user.ROLE == "user")
                {
                    return View("~/Views/Home/index.cshtml");
                }
                else
                {
                    return View("~/Views/Admin/index.cshtml");
                }


            }
            else
            {
                //unverified
                ViewBag.error = "<div class='alert alert-danger alert-sm' style='width:300px'>Incorrect Verification Code</div>";
                return View("~/Views/User/Verify.cshtml");
            }

        }
        [HandleError()]
        public async Task<ActionResult> Resend()
        {
            try
            {
                var id = Convert.ToInt32(Session["id"].ToString());
                var user = db.TBL_USER.Where(X => X.USER_ID == id).FirstOrDefault();
                string link = com.EncodePass(user.TOKEN, "token");

                link = "Http://shpud.com/User/Verification/" + link;
                await SendEmail(user.EMAIL, link, user.TOKEN, user.NAME);
                ViewBag.success = "<div class='alert alert-success alert-md' style='width:400px'>Success! Code send to " + user.EMAIL + " </div>";
                return View("~/Views/User/Verify.cshtml");
            }
            catch (Exception e)
            {
                ViewBag.error = "<script>alert('" + e.ToString() + "'</script>";
                return View("~/Views/User/Login.cshtml");
            }

        }
        public ActionResult editProfile()
        {
            var id = Convert.ToInt32(Session["userid"].ToString());
            var mdl = db.TBL_USER.Where(x => x.USER_ID == id).FirstOrDefault();
            return PartialView(mdl);
        }

        [HttpPost]
        public ActionResult editProfile(HttpPostedFileBase filename, TBL_USER model)
        {
            if (ModelState.IsValid)
            {
                var test = db.TBL_USER.Find(Convert.ToInt32( Session["userid"].ToString()));
                if (test != null)
                {
                    test.NAME = model.NAME;
                    test.GENDER = model.GENDER;
                    test.CONTACT = model.CONTACT;
                    test.ADDRESS = model.ADDRESS;
                    test.DOB = model.DOB;
                    test.IMAGE = model.IMAGE;
                    if (filename != null)
                    {
                        string pic = System.IO.Path.GetExtension(filename.FileName);
                        string name = Guid.NewGuid().ToString() + pic;
                        string path = System.IO.Path.Combine(
                        Server.MapPath("~/Contents/upload/user"), name);
                        
                        filename.SaveAs(path);
                        test.IMAGE = name; 
                    }

                    try
                    {
                        db.SaveChanges();
                        return Json(new { successMessage = "Successfully Updated !" });

                    }
                    catch(Exception e)
                    {
                        return Json(new { errorMessage = e.ToString() });
                    }
                    
                }
                else
                {
                    return Json(new { errorMessage = "Session out !" });
                }
               
            }
            else
            {
                return Json(new { errorMessage = "Fill all required fields !" });
            }
            
        }

        public ActionResult changepassword()
        {
            
            return PartialView();
        }
        [HttpPost]
        public ActionResult changepassword(string currentpass,string newpass,string cpass)
        {
            var test = db.TBL_USER.Find(Convert.ToInt32(Session["userid"].ToString()));
            if (test != null)
            {
                if(currentpass!="" || newpass!="" || cpass!="")
                {
                    currentpass = com.EncodePass(currentpass, "shpud");
                    if(test.PASSWORD==currentpass)
                    {
                        test.PASSWORD = currentpass;
                        db.SaveChanges();
                        return Json(new { successMessage = "Successfully Updated" });
                    }
                    else
                    {
                        return Json(new { errorMessage = "Incorrect current password" });
                    }
                    
                }
                else
                {
                    return Json(new { errorMessage = "Fill all required fields !" });
                }
            }
            else
            {
                return Json(new { errorMessage = "Session out !" });
            }
                
        }



    }
}