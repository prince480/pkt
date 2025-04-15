using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using PKT.DataAccess;
using PKT.Models;
using System.Configuration;
using System.Data;
using static PKT.Models.UserInfo;

namespace PKT.Controllers
{
    public class ReportsController : Controller
    {

        private readonly ILogger<QuestionerController> _logger;
        private readonly DBContext _DBContext;
        private readonly IExcelService _excelService;
        private IConfiguration _configuration;
        private IWebHostEnvironment _webHostEnvironment;

        public ReportsController(ILogger<QuestionerController> logger, DBContext DBContext, IExcelService excelService, IWebHostEnvironment webHostEnvironment)
        {
            _DBContext = DBContext;
            _logger = logger;
            _excelService = excelService;
            _webHostEnvironment = webHostEnvironment;

        }

        public IActionResult Index()
        {
            UserInfo.UserDetails _UserDetails = new UserInfo.UserDetails();
            SessionHandler.ValidateSession(HttpContext, ref _UserDetails);
            ViewBag.UserName = _UserDetails.userName;
            ViewBag.Privilege = Convert.ToInt32(_UserDetails.privilege);
            return View();
        }



        [HttpPost]
        public IActionResult Get_Reports(string ReportId, string MonthCode, string CostId, string PKTName)
            {


            UserDetails _UserDetails = new UserDetails();
            SessionHandler.ValidateSession(HttpContext, ref _UserDetails);

            DataSet dt = _DBContext.Get_Reports("usp_PKT_Get_Reports", ReportId, MonthCode, CostId, PKTName, _UserDetails.userId);


            if (dt.Tables[0].Rows.Count > 0)
            {
                if (true)
                {
                    object DataValues;
                    List<Dictionary<string, object>> Data1;
                    List<Dictionary<string, object>> Data2;

                    if (ReportId != "6")
                    {
                         Data1 = dt.Tables[0].AsEnumerable()
                            .Select(row =>
                            {
                                var questionObj = new Dictionary<string, object>();

                                foreach (DataColumn col in dt.Tables[0].Columns)
                                {
                                    questionObj[col.ColumnName] = row.IsNull(col) ? "" : row[col];
                                }

                                return questionObj;
                            })
                            .ToList();

                           DataValues = new { Data1  };
                    }
                    else
                    {
                         Data1 = dt.Tables[0].AsEnumerable()
                            .Select(row =>
                            {
                                var questionObj = new Dictionary<string, object>();

                                foreach (DataColumn col in dt.Tables[0].Columns)
                                {
                                    questionObj[col.ColumnName] = row.IsNull(col) ? "" : row[col];
                                }

                                return questionObj;
                            })
                            .ToList();

                         Data2 = dt.Tables[1].AsEnumerable()
                            .Select(row =>
                            {
                                var questionObj = new Dictionary<string, object>();

                                foreach (DataColumn col in dt.Tables[1].Columns)
                                {
                                    questionObj[col.ColumnName] = row.IsNull(col) ? "" : row[col];
                                }

                                return questionObj;
                            })
                            .ToList();

                          DataValues = new { Data1, Data2 };
                    }

                    return Json(new { status = "success", Data= DataValues });


                }
                else
                {
                    return Json(new { status = "error", msg = "No Data Found" });
                }
            }
            else
            {
                return Json(new { status = "error", msg = "No Data Found" });
            }


        }


        [HttpPost]
        public IActionResult Download_Reports(string ReportId, string MonthCode, string CostId, string PKTName)
        {

            UserDetails _UserDetails = new UserDetails();
            SessionHandler.ValidateSession(HttpContext, ref _UserDetails);

            DataSet dt = _DBContext.Get_Reports("usp_PKT_Get_Reports", ReportId, MonthCode, CostId, PKTName, _UserDetails.userId);

            if (dt.Tables[0].Rows.Count > 0)
            {

                string fileName = $"Reports_{DateTime.Now:yyyyMMddHHmmss}.xlsx"; 
                var excelService = new ExcelService(_configuration, _webHostEnvironment);
                var excelBytes = excelService.GenerateExcelFile(dt, fileName);
 
                return Json(new { status = "success", msg = fileName });
            }
            else
            {
                return Json(new { status = "error", msg = "No Data Found" });
            }
        }

        [HttpGet]
        public IActionResult DownloadFile(string fileName)
        {
            try
            {
                var fileNames = fileName; //"Reports_20231123150051.xlsx";
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Resource", "Download", fileNames);

                if (!System.IO.File.Exists(filePath))
                {
                    return NotFound(); 
                }

                var fileStream = System.IO.File.OpenRead(filePath);
                return File(fileStream, "application/octet-stream", fileNames);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }



    }
}
