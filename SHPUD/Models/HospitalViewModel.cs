using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SHPUD.Models
{
    public class HospitalViewModel
    {
        [Key]
        public int HOSPITAL_ID { get; set; }
        public Nullable<System.DateTime> DateAdded { get; set; }
        public string HOSPITAL_NAME { get; set; }
        public string HOSPITAL_ADDRESS { get; set; }
        public string EMAIL { get; set; }
        public string SPECIALIZATION { get; set; }
        public string HOSPITAL_IMAGE { get; set; }
        public string CONTACT { get; set; }

        public string DESCRIPTION { get; set; }
        public string MORE_INFO { get; set; }
        public string REMARKS { get; set; }
    }
}