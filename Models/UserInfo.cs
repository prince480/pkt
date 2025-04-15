using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PKT.Models
{
    public class UserInfo
    {
        [Serializable]
        public class UserDetails
        {
            public string userId { get; set; }
            public string userName { get; set; }
            public string userType { get; set; }
            public int privilege { get; set; }
            public string designation { get; set; }
            public int region { get; set; }
            public bool isMobileDevice { get; set; }
            public string sessionId { get; set; }
            public int? spl_access { get; set; }

            public string auth_acct_url { get; set; }
            public string acct_validate_url { get; set; }
            public string acct_query_url { get; set; }
            public string api_key { get; set; }
            public string iv { get; set; }
            public string pb_secret { get; set; }
            public string pb_userId { get; set; }
            public string pb_password { get; set; }
            public string channel { get; set; }
            public string CostName { get; set; }
            public string PrivilegeName { get; set; }

            public string DOJ { get; set; }




        }
    }
}
