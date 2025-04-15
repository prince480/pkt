using Microsoft.AspNetCore.Mvc;
using PKT.DataAccess;
using PKT.Models;
using System.Diagnostics;
using System;
using static PKT.Models.UserInfo;
using Newtonsoft.Json;
using System.Data.Common;
using System.Data;
//using static PKT.Models.UserInfo;

namespace PKT.Controllers
{
    public class UploadController : Controller
    {

        private readonly ILogger<QuestionerController> _logger;
        private readonly DBContext _DBContext; 

        public UploadController(ILogger<QuestionerController> logger, DBContext DBContext )
        {
            _DBContext = DBContext;
            _logger = logger; 

        }


        public IActionResult Index()
        {

            UserInfo.UserDetails _UserDetails = new UserInfo.UserDetails();
            SessionHandler.ValidateSession(HttpContext, ref _UserDetails);
            

            Upload _mod = new Upload();

            try
            {
                if (true)
                {
                    ViewBag.TempList = _DBContext.Get_UI_Elements("PKT_ProcessList", "", "", "", _UserDetails.userId, _UserDetails.privilege.ToString(), _UserDetails.region.ToString(), _UserDetails.userType);
                    ViewBag.UserName = _UserDetails.userName;
                    ViewBag.Privilege = Convert.ToInt32(_UserDetails.privilege);

                    return View();
                }
                else
                {
                    return RedirectToAction("SignIn", "Login");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            
        }



 

        public IActionResult get_dropdown_list(string Criteria, string ID, string Condition2="")
        {
            try
            {
                UserDetails _UserDetails = new UserDetails();
                SessionHandler.ValidateSession(HttpContext, ref _UserDetails);
                var data = _DBContext.Get_UI_Elements(Criteria, ID, Condition2, "", _UserDetails.userId, _UserDetails.privilege.ToString(), _UserDetails.region.ToString(), _UserDetails.userType);
                string json = JsonConvert.SerializeObject(data);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                var error = new { error = ex.Message };
                string errorJson = JsonConvert.SerializeObject(error);
                return Content(errorJson, "application/json");
            }
        }



        [HttpPost]
        [Route("Base_Uploads")]
        public async Task<IActionResult> Base_Uploads([FromForm] IFormFile postedFile , [FromForm] string CostCode, [FromForm] string PKTName)
        {
            UserDetails _UserDetails = new UserDetails();
            SessionHandler.ValidateSession(HttpContext, ref _UserDetails);

            try
            {
                string ReqFileName = "PKT_Question_Base.csv";
                string FolderName = "Question_Base_Uploads";

                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Input", FolderName);

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                if (postedFile != null)
                {
                    string fileName = Path.GetFileName(postedFile.FileName);

                    if (!fileName.Equals(ReqFileName, StringComparison.OrdinalIgnoreCase))
                    {
                        return Json(new { status = "error", Msg= "Please choose a valid file / Check the File Name!" });
                       // return BadRequest("");
                    }

                    string dest = Path.Combine(path, ReqFileName);
                    using (var fileStream = new FileStream(dest, FileMode.Create))
                    {
                        await postedFile.CopyToAsync(fileStream);
                    }

                    DataSet dt = _DBContext.Base_Uploads(CostCode, PKTName, _UserDetails.userId );

                    if (dt.Tables[0].Rows.Count > 0)
                    {
                        if (dt.Tables[0].Rows[0]["MSG"].ToString().ToUpper() == "UPLOADED")
                        {
                            return Json(new { status = "success", Msg = "The File has been uploaded Successfully!" });
 
                        }
                        else if (dt.Tables[0].Rows[0]["MSG"].ToString().ToUpper() == "ZEROUPLOAD")
                        {
                            return Json(new { status = "error", Msg = "No Rows Uploaded, Please confirm the File / Contact Administrator!" });
 
                        }
                        else  
                        {
                            return Json(new { status = "error", Msg = dt.Tables[0].Rows[0]["MSG"].ToString() });

                        }
                    }
                    else
                    {
                        return Json(new { status = "error", Msg = "No data found" });
 
                    }
                }
                else
                {
                    return Json(new { status = "error", Msg = "Please select a file to upload" });
                 }
            }
            catch (FileNotFoundException ex)
            {
                _logger.LogError(ex, "File not found error");
                return NotFound($"Error: {ex.Message}");
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "Database error");
                return StatusCode(500, $"Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error");
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }



        [HttpPost]
        public IActionResult Get_Reports(string CostId, string TestName  )
        {
            DataSet dt = _DBContext.Get_UI_Elements("PKT_TestReport", CostId, "", "", "", "", "", "");

            if (dt.Tables[0].Rows.Count > 0)
            {
                if (true)
                {
                    var Data = dt.Tables[0].AsEnumerable()
                    .Select(row => {
                        var questionObj = new Dictionary<string, object>();

                        foreach (DataColumn col in dt.Tables[0].Columns)
                        {
                            questionObj[col.ColumnName] = row.IsNull(col) ? "" : row[col];
                        }

                        return questionObj;
                    })
                    .ToList();

                    return Json(new { status = "success", Data });
                }
                else
                {
                    return Json(new { status = "error", msg="No Data Found" });
                }
            }
            else
            {
                return Json(new { status = "error", msg = "No Data Found" });
            }
  

        }
    }
}
