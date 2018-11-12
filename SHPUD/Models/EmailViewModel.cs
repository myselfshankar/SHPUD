using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SHPUD.Models
{
    public class EmailViewModel
    {
        [Required, Display(Name = "First name")]
        public string Firstname { get; set; }
        [Required, Display(Name = "Last name")]
        public string Lastname { get; set; }

        [Required, Display(Name = "Your email"), EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Message { get; set; }
        [Display(Name = "Phone")]
        public string phone { get; set; }
    }
}