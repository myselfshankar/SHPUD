using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SHPUD.Models
{
    [MetadataType(typeof(UserMeta))]
    public partial class TBL_USER
    {
    }
    [MetadataType(typeof(TBL_HOSPITAL_META))]
    public partial class TBL_HOSPITAL
    {
       
    }
    [MetadataType(typeof(DOCTOR_DETAILS_META))]
    public partial class TBL_DOCTOR_DETAILS
    {

    }

    public partial class TBL_HOSPITAL_META
    {
        [Key]
        public int HOSPITAL_ID { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        [Display(Name ="HOSPITAL NAME")]
        public string HOSPITAL_NAME { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "HOSPITAL ADDRESS")]
        public string HOSPITAL_ADDRESS { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        public string CONTACT { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        [DataType(DataType.EmailAddress)]
        public string EMAIL { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        public string SPECIALIZATION { get; set; }
        [Display(Name = "HOSPITAL IMAGE")]
        public string HOSPITAL_IMAGE { get; set; }
        public Nullable<System.DateTime> DateAdded { get; set; }
    }
    [MetadataType(typeof(TBL_DISEASE_META))]
    public partial class TBL_DISEASE
    {
    }

    [MetadataType(typeof(TBL_DOCTOR_META))]
    public partial class TBL_DOCTOR
    {
    }
    [MetadataType(typeof(DISEASE_DETAILS_META))]
    public partial class TBL_DISEASE_DETAILS
    {
    }
    public partial class TBL_DOCTOR_META
    {
        [Key]
        public int DOCTOR_ID { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "DOCTOR NAME")]
        public string DOCTOR_NAME { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        public string ADDRESS { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        public string CONTACT { get; set; }
        [DataType(DataType.EmailAddress)]
        public string EMAIL { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        public string SPECIALIZATION { get; set; }
        [Display(Name = "IMAGE")]
        public string DOCTOR_IMAGE { get; set; }
        public string DOCUMENT { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        public string WORK_HOSPITAL { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        public string GENDER { get; set; }
        [DataType(DataType.Password)]
        public string PASSWORD { get; set; }
        public Nullable<System.DateTime> DateAdded { get; set; }
    }
    public partial class TBL_DISEASE_META
    {
        [Key]
        public int DISEASE_ID { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "DISEASE NAME")]
        public string DISEASE_NAME { get; set; }
        [Required(ErrorMessage = "{0} is required")]
       
        public string SYMPTOMS { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        public string CATEGORY { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        public Nullable<int> TREATMENT { get; set; }
    }
    public partial class UserMeta
    {
        [Key]
        public int USER_ID { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        public string NAME { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        public string ADDRESS { get; set; }
        
        [Required(ErrorMessage = "{0} is required")]
        [DataType(DataType.Password)]
        public string PASSWORD { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        [DataType(DataType.EmailAddress)]

        public string EMAIL { get; set; }
        public string GENDER { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name ="DATE OF BIRTH")]
        public System.DateTime DOB { get; set; }
        public string CONTACT { get; set; }
        public string IMAGE { get; set; }
    }

    public partial class DOCTOR_DETAILS_META
    {
        [Key]
        public int DETAIL_ID { get; set; }
        public int DOCTOR_ID { get; set; }
        [Display(Name = "DETAIL DESCRIPTION")]
        public string DOCTOR_DESCRIPTION { get; set; }
        [Display(Name = "EDUCATION DETAILS")]
        public string DOCTOR_EDUCATION { get; set; }
        [Display(Name = "SPECIALIZATION DETAILS")]
        public string DOCTOR_SPECIALIZATION_DETAILS { get; set; }
    }

    public partial class DISEASE_DETAILS_META
    {
        [Key]
        public int DETAIL_ID { get; set; }
        public int DISEASE_ID { get; set; }
        [Display(Name = "DETAIL DESCRIPTION")]
        public string DISEASE_DESCRIPTION { get; set; }
        [Display(Name = "TREATMENT PROCESS")]
        public string TREATMENT { get; set; }
        [Display(Name = "SUGGESTION")]
        public string SUGGESTION { get; set; }
        public string IMAGE { get; set; }
    }
}