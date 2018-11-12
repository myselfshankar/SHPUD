using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SHPUD.Models
{
    public class DoctorViewModel
    {
        public int DOCTOR_ID { get; set; }
        public Nullable<System.DateTime> DateAdded { get; set; }
        public string DOCTOR_NAME { get; set; }
        public string ADDRESS { get; set; }
        public string CONTACT { get; set; }
        public string EMAIL { get; set; }
        public string SPECIALIZATION { get; set; }
        public string DOCTOR_IMAGE { get; set; }
        public string DOCUMENT { get; set; }
        public string WORK_HOSPITAL { get; set; }
        public string GENDER { get; set; }
        public string PASSWORD { get; set; }
        public string DOCTOR_DESCRIPTION { get; set; }
        public string DOCTOR_EDUCATION { get; set; }
        public string DOCTOR_SPECIALIZATION_DETAILS { get; set; }
        public int views { get; set; }
    }
}