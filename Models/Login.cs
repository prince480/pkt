using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace PKT.Models
{
    public class Login
    {
        public string userId { get; set; }
        public string Password { get; set; }

        public string Status { get; set; }

        public string Message { get; set; }
    }
}
