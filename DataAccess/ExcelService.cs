using ClosedXML.Excel;
using System;
using System.Data;
using System.IO;
using Microsoft.AspNetCore.Mvc;

using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;

namespace PKT.DataAccess
{
    public interface IExcelService
    {
        byte[] GenerateExcelFile(DataSet dt, string fileName);
    }
            
    public class ExcelService : IExcelService
    {

            private readonly IConfiguration _configuration;
            private readonly IWebHostEnvironment _webHostEnvironment;

            public ExcelService(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
            {

                _configuration = configuration;
                _webHostEnvironment = webHostEnvironment;
            }

 


        public byte[] GenerateExcelFile(DataSet dt, string fileName)
        {

            var serverPath = Path.Combine(_webHostEnvironment.WebRootPath, "Resource", "Download", fileName);

            //string serverPath = Path.Combine("Resource/Download/", fileName );

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Reports");

                // Add column headers
                for (int i = 0; i < dt.Tables[0].Columns.Count; i++)
                {
                    worksheet.Cell(1, i + 1).Value = dt.Tables[0].Columns[i].ColumnName;
                }

                // Add data to the cells
                for (int row = 0; row < dt.Tables[0].Rows.Count; row++)
                {
                    for (int column = 0; column < dt.Tables[0].Columns.Count; column++)
                    {
                        worksheet.Cell(row + 2, column + 1).Value = dt.Tables[0].Rows[row][column].ToString();
                    }
                }

                using (var memoryStream = new MemoryStream())
                {
                    workbook.SaveAs(serverPath);
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    return memoryStream.ToArray();
                }
            }
        }
 
    }

}
