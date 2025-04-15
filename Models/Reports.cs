using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Web;

namespace PKT.Models
{
    public class Reports
    {
        public string status { get; set; }
        public string msg { get; set; }
        public string responsecode { get; set; }

        public string ProcessCodes { get; set; }

        public string PKTName { get; set; }
        public string fromDate { get; set; }
        public string toDate { get; set; }


    }
}
