using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SHPUD.Models
{
    public class DiseaseCount
    {
        public TBL_DISEASE disease { get; set; }
        public int count { get; set; }

        public string match { get; set; }
        public DiseaseCount()
        {
            this.count = 1;
        }
    }
    
}