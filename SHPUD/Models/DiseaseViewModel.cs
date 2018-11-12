using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SHPUD.Models
{
    public class DiseaseViewModel
    {
        [Key]
        public int DISEASE_ID { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "DISEASE NAME")]
        public string DISEASE_NAME { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "TREATMENT")]
       
        public int TREATMENT { get; set; }
        [Display(Name = "SUGGESTION")]
        public string SUGGESTION { get; set; }
        [Display(Name = "DESCRIPTION")]
        public string DISEASE_Description { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        
        public string SYMPTOMS { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        public string CATEGORY { get; set; }
        
        [Display(Name = "IMAGE")]
        public string IMAGE { get; set; }
        public string TREATMENT_DETAILS { get; set; }
    }
}