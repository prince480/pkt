using Microsoft.Extensions.Configuration;
using PKT.Models;
using NuGet.Protocol.Plugins;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace PKT.DataAccess
{
    public class DBContext
    {
        private readonly SqlHelper _SqlHelper;
        public DBContext(SqlHelper sqlHelper)
        {
            _SqlHelper = sqlHelper;
        }

        public DataSet Check_Login(string SPName, string userId, string password, string ipAddress, string isMobileDevice, string sessionid)
        {

            var result = _SqlHelper.ReturnDataset("usp_PKT_Check_Login", new Dictionary<string, object>
            {
                { "userId", userId },
                 { "password", password },
                 { "ipAddress", ipAddress },
                 { "isMobileDevice", isMobileDevice },
                 { "sessionid", sessionid}
             });
            return result;

        }


        public List<object> Get_UI_Elements_List(string criteria, string condition1, string condition2, 
                                                    string condition3, string userId, string privilege, 
                                                    string region, string userType)
        {
            List<object> ListItems;

            var parameters = new Dictionary<string, object>
            {
                { "criteria", criteria },
                { "condition1", condition1 },
                { "condition2", condition2 },
                { "condition3", condition3 },
                { "userId",     userId },
                { "privilege",  privilege },
                { "region",     region },
                { "userType",   userType }
            };

            var dataSet = _SqlHelper.ReturnDataset("usp_PKT_GET_UI_ELEMENTS", parameters);
           

            if (criteria == "PKT_NavBar")
                {
                ListItems = CreateNavItems_DataSet(dataSet).Cast<object>().ToList();

            }
                else
                {

                ListItems = CreateCodeItems_DataSet(dataSet).Cast<object>().ToList();

            }
            return ListItems;
        }

        public DataSet Get_UI_Elements(string criteria, string condition1, string condition2,
                                                    string condition3, string userId, string privilege,
                                                    string region, string userType)
        {
            

            var parameters = new Dictionary<string, object>
            {
                { "criteria", criteria },
                { "condition1", condition1 },
                { "condition2", condition2 },
                { "condition3", condition3 },
                { "userId",     userId },
                { "privilege",  privilege },
                { "region",     region },
                { "userType",   userType }
            };

            var dataSet = _SqlHelper.ReturnDataset("usp_PKT_GET_UI_ELEMENTS", parameters);


            return dataSet;
        }


        public DataSet Update_Questions(string QuestionId, string Question, string Option1, string Option1Id,
                                             string Option2, string Option2Id, string Option3, string Option3Id,
                                             string Option4, string Option4Id, string Option5, string Option5Id, 
                                             string Status, string Marks, string UserId)
        {

            var parameters = new Dictionary<string, object>
            {
                { "QuestionId", QuestionId },
                { "Question", Question },
                { "Option1", Option1 },
                { "Option1Id", Option1Id },
                { "Option2", Option2 },
                { "Option2Id", Option2Id },
                { "Option3", Option3 },
                { "Option3Id", Option3Id },
                { "Option4", Option4 },
                { "Option4Id", Option4Id },
                { "Option5", Option5 },
                { "Option5Id", Option5Id },
                { "Status", Status },
                { "Marks", Marks },
                { "UserId", UserId }
            };

            var dataSet = _SqlHelper.ReturnDataset("usp_PKT_Question_Update", parameters);


            return dataSet;
        }



        public List<NavBar> CreateNavItems_DataSet(DataSet dataSet)
        {
            List<NavBar> navigationItems = new List<NavBar>();

            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                DataTable table = dataSet.Tables[0];

                foreach (DataRow row in table.Rows)
                {
                    var navigationItem = new NavBar
                    {
                        Id = Convert.ToInt32(row["Id"]),
                        LinkText = row["LinkText"].ToString(),
                        ControllerName = row["ControllerName"].ToString(),
                        ActionName = row["ActionName"].ToString(),
                        IsActive = Convert.ToBoolean(row["isActive"]),
                        MenuId = Convert.ToInt32(row["MenuId"]),
                        Access = row["Access"].ToString(),
                        SubMenus = new List<NavBar>() 
                    };

                    var parentId = navigationItem.Id;
                
                    navigationItems.Add(navigationItem);
                }
            }

            return navigationItems;
        }

        public List<General> CreateCodeItems_DataSet(DataSet dataSet)
        {
            List<General> CodeNames = new List<General>();

            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                DataTable table = dataSet.Tables[0];

                foreach (DataRow row in table.Rows)
                {
                    var CodeName = new General
                    {
                        Code = row["Code"].ToString(),
                        Name = row["Name"].ToString(),
                    
                    };

                    CodeNames.Add(CodeName);
                }
            }

            return CodeNames;
        }



        public DataSet Base_Uploads(string CostId, string TestName, string UserId  )
        {


            var parameters = new Dictionary<string, object>
            {
                { "CostId", CostId   },
                { "TestName", TestName},
                { "UserId", UserId } 
            };

            var dataSet = _SqlHelper.ReturnDataset("usp_PKT_Question_Upload", parameters);


            return dataSet;
        }


        public DataSet Add_Questions_Entry(string SelectedIds,string PKTId, string TimeTaken,string UserId)
        {

            var parameters = new Dictionary<string, object>
            {
                { "SelectedIds", SelectedIds },
                { "ProductKnowledgeTestNamesId", PKTId },
                { "TimeTaken", TimeTaken },
                { "UserId", UserId }
            };

            var dataSet = _SqlHelper.ReturnDataset("usp_PKT_Question_Entry", parameters);


            return dataSet;
        }


        public DataSet Get_Reports(string SPName, string ReportId, string MonthCode,
                                                  string CostId, string PKTName, string userId )
        {


            var parameters = new Dictionary<string, object>
            {
                { "ReportId", ReportId },
                { "MonthCode", MonthCode },
                { "CostId",   CostId },
                { "PKTName",  PKTName },
                { "userId",   userId }
            };

            var dataSet = _SqlHelper.ReturnDataset(SPName, parameters);


            return dataSet;
        }

    }
}