using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Web;

namespace PKT.Models
{
    public class Upload
    {
        public string status { get; set; }
        public string msg { get; set; }
        public string responsecode { get; set; }

        public string ProcessCodes { get; set; }

        public string FileNames { get; set; }
        public string FileDate { get; set; }

        public string uploadType { get; set; }
         
        public string FileType { get; set; }
        public string Path { get; set; }

    }
}
