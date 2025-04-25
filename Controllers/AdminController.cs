using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PKT.DataAccess;
using PKT.Models;
using System.Data;

namespace PKT.Controllers
{
    public class AdminController : Controller
    {
        private readonly DBContext _DBContext;
        
        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();

        public AdminController( DBContext DBContext)
        {
            _DBContext = DBContext;
        }

        public IActionResult Navbar()
        {

            UserInfo.UserDetails _UserDetails = new UserInfo.UserDetails();
            SessionHandler.ValidateSession(HttpContext, ref _UserDetails);

            var AccessList = _DBContext.Get_UI_Elements_List("PKT_PrivilegeList", "", "", "", "", "", "", "");
            ViewBag.AccessList = AccessList.Cast<General>().ToList();
            ViewBag.UserName = _UserDetails.userName;
            ViewBag.Privilege = Convert.ToInt32(_UserDetails.privilege);
            return View();
 
        }


        public IActionResult Users()
        {

            UserInfo.UserDetails _UserDetails = new UserInfo.UserDetails();
            SessionHandler.ValidateSession(HttpContext, ref _UserDetails);
            ViewBag.UserName = _UserDetails.userName;
            ViewBag.Privilege = Convert.ToInt32(_UserDetails.privilege);
            return View();
        }   


        [HttpPost("Get_NavbarReport")]
        public IActionResult Get_NavbarReport()
        {
            var result = _DBContext.Get_UI_Elements("PKT_NavBarReport", "", "", "", "", "", "", "");

           

            if (result.Tables[0].Rows.Count > 0)
            {
                var response = new
                {
                    rows = ConvertDataTableToList(result.Tables[0]),
                    mod = new { Status = "success", Message = "Data Found!" }
                };
                return Ok(response);
            }
            else
            {
                var response = new
                {
                    mod = new { Status = "error", Message = "No data found" }
                };
                return NotFound(response);
            }
        }


        [HttpPost("Update_Active")]
        public IActionResult Update_Active(string Criteria, string Id,  string condition1)
        {
            UserInfo.UserDetails _UserDetails = new UserInfo.UserDetails();
            SessionHandler.ValidateSession(HttpContext, ref _UserDetails);

            var result = _DBContext.Get_UI_Elements(Criteria, Id, condition1, "", _UserDetails.userId, "", "", "");

            if (result.Tables[0].Rows.Count > 0)
            {

                var response = new
                {
                    mod = new { Status = "", Message = "" }
                };

                if (result.Tables[0].Rows[0]["MSG"].ToString().ToUpper().Equals("UPDATED"))
                {
                      response = new
                    {
                        mod = new { Status = "success", Message = "Updated Successfully!" }
                    };
                }
                else
                {
                    response = new
                    {
                        mod = new { Status = "error", Message = result.Tables[0].Rows[0]["MSG"].ToString()??"" }
                    };

                }
                
                return Ok(response);
            }
            else
            {
                var response = new
                {
                    mod = new { Status = "error", Message = "No data found" }
                };
                return NotFound(response);
            }
        }

        [HttpPost("Update_AccessList")]
        public IActionResult Update_AccessList(string Criteria, string AccessId,string selectedValues)
        {
            UserInfo.UserDetails _UserDetails = new UserInfo.UserDetails();
            SessionHandler.ValidateSession(HttpContext, ref _UserDetails);

            var result = _DBContext.Get_UI_Elements(Criteria, AccessId, selectedValues, "", _UserDetails.userId, "", "", "");

            if (result.Tables[0].Rows.Count > 0)
            {
                var response = new
                {
                    mod = new { Status = "success", Message = "Updated Successfuly!" }
                };
                    return Ok(response);
            }
            else
            {
                var response = new
                {
                    mod = new { Status = "error", Message = "No Data Updated" }
                };
                return NotFound(response);
            }
        }

        private List<Dictionary<string, object>> ConvertDataTableToList(DataTable table)
        {
            var rows = new List<Dictionary<string, object>>();
            foreach (DataRow dr in table.Rows)
            {
                var row = new Dictionary<string, object>();
                foreach (DataColumn col in table.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }
            return rows;
        }



    }
}
