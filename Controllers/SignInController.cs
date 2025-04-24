using Microsoft.AspNetCore.Mvc;
using PKT.DataAccess;
using PKT.Models;
using Newtonsoft.Json;
using System.Data;
using static PKT.Models.UserInfo;



namespace PKT.Controllers
{
    public class SignInController : Controller
    {

        private readonly DBContext _DBContext;
 

        public SignInController(DBContext dbContext )
        {
            _DBContext = dbContext;
             

        }

        public IActionResult Login()
        {
            HttpContext.Session.Clear();
            return View();
        }

        [HttpPost]
        public IActionResult Login([FromBody] Login login)
        {

            var sessionId = HttpContext.Session.Id;
            
            var result = _DBContext.Check_Login("usp_PKT_Check_Login", login.userId, login.Password,"","", sessionId);

            if (result.Tables[0].Rows.Count > 0)
            {
                //if (result.Tables[0].Rows[0]["MSG"].ToString().ToUpper().Equals("SUCCESS"))
                if (result != null &&
                        result.Tables.Count > 0 &&
                        result.Tables[0].Rows.Count > 0 &&
                        result.Tables[0].Rows[0]["MSG"] != DBNull.Value &&
                        result.Tables[0].Rows[0]["MSG"]?.ToString()?.ToUpper() == "SUCCESS")
                {

                    HttpContext.Session.SetString("userId", result.Tables[0].Rows[0]["empCode"].ToString());
                    HttpContext.Session.SetString("userName", result.Tables[0].Rows[0]["EmpName"].ToString());
                    HttpContext.Session.SetString("privilege", result.Tables[0].Rows[0]["privilege"].ToString());
                    HttpContext.Session.SetString("region", result.Tables[0].Rows[0]["region"].ToString());
                    HttpContext.Session.SetString("sessionId", sessionId);
                    HttpContext.Session.SetString("isMobileDevice", "1");
                    HttpContext.Session.SetString("DOJ", result.Tables[0].Rows[0]["DOJ"].ToString());

                    HttpContext.Session.SetString("CostName", result.Tables[0].Rows[0]["CostName"].ToString());
                    HttpContext.Session.SetString("PrivilegeName", result.Tables[0].Rows[0]["PrivilegeName"].ToString());
              
                    var userId = HttpContext.Session.GetString("userId");
                    var privilege = HttpContext.Session.GetString("privilege");

                    UserDetails _UserDetails = new UserDetails
                    {
                        userId = HttpContext.Session.GetString("userId"),
                        userName = HttpContext.Session.GetString("userName"),
                        privilege = Convert.ToInt32(HttpContext.Session.GetString("privilege")),
                        region = Convert.ToInt32(HttpContext.Session.GetString("region")),
                        isMobileDevice = HttpContext.Session.GetString("isMobileDevice") == "1",
                        sessionId = HttpContext.Session.GetString("sessionId"),
                        userType = HttpContext.Session.GetString("userType"),
                        DOJ = HttpContext.Session.GetString("DOJ"),

                        CostName = HttpContext.Session.GetString("CostName"),
                        PrivilegeName = HttpContext.Session.GetString("PrivilegeName"),

                        spl_access = Convert.ToInt32(HttpContext.Session.GetString("spl_access"))
                    };

                    string userInfoJson = JsonConvert.SerializeObject(_UserDetails);
                    HttpContext.Session.SetString("UserDetails", userInfoJson);


                    return Json(new { success = true, privilege= privilege });

                }
                else
                {
                    return Json(new { success = false , msg= result.Tables[0].Rows[0]["MSG"].ToString() });
                }
            }
            else
            {
                return Json(new { success = false });
            }

        }



        [HttpGet]
        public IActionResult Login(string UName, string UCode)
        {

            var DecUName = "";
            var DecUCode = "";

            try
            {

                if (UName != null && UCode != null)
                {
                    DecUName = Crypts.Decrypt(UName.ToString());
                    DecUCode = Crypts.Decrypt(UCode.ToString());
                }
                else
                {
                    HttpContext.Session.Clear();
                    return View("Login");
                }

                var sessionId = HttpContext.Session.Id;

                var result = _DBContext.Check_Login("usp_PKT_Check_Login", DecUName, DecUCode, "", "", sessionId);

                if (result.Tables[0].Rows.Count > 0)
                {
                    if (result.Tables[0].Rows[0]["MSG"].ToString().ToUpper().Equals("SUCCESS"))
                    {

                        HttpContext.Session.SetString("userId", result.Tables[0].Rows[0]["empCode"].ToString());
                        HttpContext.Session.SetString("userName", result.Tables[0].Rows[0]["EmpName"].ToString());
                        HttpContext.Session.SetString("privilege", result.Tables[0].Rows[0]["privilege"].ToString());
                        HttpContext.Session.SetString("region", result.Tables[0].Rows[0]["region"].ToString());
                        HttpContext.Session.SetString("sessionId", sessionId);
                        HttpContext.Session.SetString("isMobileDevice", "1");

                        HttpContext.Session.SetString("CostName", result.Tables[0].Rows[0]["CostName"].ToString());
                        HttpContext.Session.SetString("PrivilegeName", result.Tables[0].Rows[0]["PrivilegeName"].ToString());

                        var userId = HttpContext.Session.GetString("userId");
                        var privilege = HttpContext.Session.GetString("privilege");

                        UserDetails _UserDetails = new UserDetails
                        {
                            userId = HttpContext.Session.GetString("userId"),
                            userName = HttpContext.Session.GetString("userName"),
                            privilege = Convert.ToInt32(HttpContext.Session.GetString("privilege")),
                            region = Convert.ToInt32(HttpContext.Session.GetString("region")),
                            isMobileDevice = HttpContext.Session.GetString("isMobileDevice") == "1",
                            sessionId = HttpContext.Session.GetString("sessionId"),
                            userType = HttpContext.Session.GetString("userType"),

                            CostName = HttpContext.Session.GetString("CostName"),
                            PrivilegeName = HttpContext.Session.GetString("PrivilegeName"),

                            spl_access = Convert.ToInt32(HttpContext.Session.GetString("spl_access"))
                        };

                        string userInfoJson = JsonConvert.SerializeObject(_UserDetails);
                        HttpContext.Session.SetString("UserDetails", userInfoJson);

                        return RedirectToAction("Index", "Home");

                    }
                    else
                    {
                        return View("Login");
                    }
                }
                else
                {
                    return View("Login");
                }

            }
            catch
            {
                return View("Login");
            }

        }







        [HttpPost]
        public IActionResult CheckUserDetails([FromBody] Login login)
        {


            var result = _DBContext.Get_UI_Elements("PKT_CheckUserDetails", login.userId, "", "", "", "", "", "");

            if (result.Tables[0].Rows.Count > 0)
            {
                if (result.Tables[0].Rows[0]["MSG"].ToString().ToUpper().Equals("SUCCESS"))
                {


                    List<Dictionary<string, object>> rowsList = new List<Dictionary<string, object>>();
                    foreach (DataRow row in result.Tables[0].Rows)
                    {
                        var rowDict = new Dictionary<string, object>();
                        foreach (DataColumn column in result.Tables[0].Columns)
                        {
                            rowDict[column.ColumnName] = row[column];
                        }
                        rowsList.Add(rowDict);
                    }

                    // Return the list of dictionaries as JSON
                    return Json(new { success = true, rows = rowsList });

                }
                else
                {
                    return Json(new { success = false });
                }
            }
            else
            {
                return Json(new { success = false });
            }

        }






    }
}
