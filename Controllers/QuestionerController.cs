using Microsoft.AspNetCore.Mvc;
using PKT.DataAccess;
using PKT.Models;
using System.Data;

using Microsoft.AspNetCore.Http;
using System.Text.Json;
using Newtonsoft.Json;

namespace PKT.Controllers
{
    public class QuestionerController : Controller
    {

        private readonly ILogger<QuestionerController> _logger;
        private readonly DBContext _DBContext;

        public QuestionerController(ILogger<QuestionerController> logger, DBContext DBContext)
        {
            _DBContext = DBContext;
            _logger = logger;
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
        public IActionResult Get_Questions(string TestName, string ForType)
        {


           DataSet dt = _DBContext.Get_UI_Elements("PKT_Get_Questions", TestName, "", "", "", "", "", "");


            if (dt.Tables.Count > 0 && dt.Tables[0].Rows.Count > 0)
            {
                string Duration =  dt.Tables[0].Rows[0]["Duration"].ToString();

                List<Dictionary<string, object>> questions = new List<Dictionary<string, object>>();
                            
                foreach (DataRow row in dt.Tables[0].Rows)
                {
                    Dictionary<string, object> question = new Dictionary<string, object>
                    {
                        { "id", Convert.ToInt32(row["QuestionId"]) },
                        { "question", row["Question"].ToString() },
                        { "options", new List<Dictionary<string, object>>() },
                        { "correctAnswerId", -1 } ,
                        { "Marks",  Convert.ToInt32(row["Marks"]) },
                    };

                    foreach (DataColumn column in dt.Tables[0].Columns)
                    {
                        if (column.ColumnName.StartsWith("Id") && row[column] != DBNull.Value)
                        {
                            int id = Convert.ToInt32(row[column]);
                            string correspondingTextColumn = "Text" + column.ColumnName.Substring(2);
                            string text = row[correspondingTextColumn]?.ToString();


                            if (ForType == "QuestionEdits")
                            {
                                Dictionary<string, object> option = new Dictionary<string, object>
                                {
                                    { "id", id },
                                    { "text", text }
                                };

                                ((List<Dictionary<string, object>>)question["options"]).Add(option);

                                if ((int)question["correctAnswerId"] == -1)
                                {
                                    question["correctAnswerId"] = id;
                                }
                            }
                            else
                            {

                            if (!string.IsNullOrEmpty(text) && !text.Contains("\r"))
                            {
                                Dictionary<string, object> option = new Dictionary<string, object>
                                {
                                    { "id", id },
                                    { "text", text }
                                };

                                ((List<Dictionary<string, object>>)question["options"]).Add(option);

                                if ((int)question["correctAnswerId"] == -1)
                                {
                                    question["correctAnswerId"] = id;
                                }
                            }

                            }
                            
                        }
                    }

                    questions.Add(question);
                }

                string json = JsonConvert.SerializeObject(new { questions });

                return Json(new { status = "success", questions = json, Duration= Duration });
            }
            else
            {
                return Json(new { status = "failure" });
            }


        }

        [HttpPost]
        public IActionResult UpdateQuestions(string QuestionId, string Question, string Option1, string Option1Id,
                                             string Option2, string Option2Id, string Option3, string Option3Id, 
                                             string Option4, string Option4Id, string Option5, string Option5Id, string Status, string Marks)
        {

            string status = string.Empty, msg = string.Empty;
            UserInfo.UserDetails _UserDetails = new UserInfo.UserDetails();
            SessionHandler.ValidateSession(HttpContext, ref _UserDetails);

            DataSet dt = _DBContext.Update_Questions(QuestionId, Question, Option1, Option1Id, Option2, Option2Id, Option3, Option3Id, 
                                                        Option4, Option4Id, Option5, Option5Id, Status, Marks, _UserDetails.userId);

            if (dt.Tables[0].Rows.Count > 0)
            {
                if (dt.Tables[0].Rows[0]["MSG"].ToString().ToUpper().Equals("UPDATED"))
                {

                    status = "success";
                    msg = "Updated Successfully";

                }
            }
            else
            {
                status = "error";
                msg = "Not Updated";
               
            }

            return Json(new { status = status, msg = msg });

        }



        [HttpPost]
        public IActionResult Add_Questions_Entry(string[] selectedIds, string PKTId, string TimeTaken)
        {
            UserInfo.UserDetails _UserDetails = new UserInfo.UserDetails();
            SessionHandler.ValidateSession(HttpContext, ref _UserDetails);
            string status = string.Empty, msg = string.Empty;

            var idsString = string.Join(",", selectedIds) + ",";
            DataSet dt = _DBContext.Add_Questions_Entry(idsString, PKTId, TimeTaken, _UserDetails.userId);

            if (dt.Tables[0].Rows.Count > 0)
            {
                if (dt.Tables[0].Rows[0]["MSG"].ToString().ToUpper().Equals("ADDED"))
                {
                    status = "success";
                    msg = "Added Successfully";
                }
            }
            else
            {
                status = "error";
                msg = "Not Updated";

            }

            return Json(new { status = status, msg = msg });
        }




        [HttpPost]
        public IActionResult Get_Reports( string Criteria, string Id)
        {
            UserInfo.UserDetails _UserDetails = new UserInfo.UserDetails();
            SessionHandler.ValidateSession(HttpContext, ref _UserDetails);  

            DataSet dt = _DBContext.Get_UI_Elements(Criteria, _UserDetails.userId, Id, "", "", "", "", "");


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
                    return Json(new { status = "error", msg = "No Records Found" });
                }
            }
            else
            {
                return Json(new { status = "error", msg = "No Records Found" });
            }


        }


    }
}
