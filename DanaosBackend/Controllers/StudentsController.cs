using Microsoft.AspNetCore.Mvc;
using DanaosBackend.Services;
using OfficeOpenXml; 

namespace DanaosBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly DatabaseService _dbService; 
        private readonly ILogger _logger;

        public StudentsController(DatabaseService dbService, ILogger<StudentsController> logger)
        {
            _dbService = dbService; 
            _logger = logger; 
        }

        /// <summary>
        /// Retrieves the calculated grade averages for all students.
        /// </summary>
        /// <returns>A 200 OK response containing a list of student grade averages.</returns>
        [HttpGet]
        public IActionResult GetStudentGrades()
        {
            try 
            {
                var data = _dbService.GetStudentAverages();
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving student grades");
                return StatusCode(500, "Internal Server Error");
            }
        }

        /// <summary>
        /// Retrieves the calculated grade averages for all courses.
        /// </summary>
        /// <returns>A 200 OK response containing a list of course averages.</returns>
        [HttpGet("courses")]
        public IActionResult GetCourseAverages()
        {
            try
            {
                var data = _dbService.GetCourseAverages();
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving course averages");
                return StatusCode(500, "Internal Server Error");
            }
        }

        /// <summary>
        /// Generates and downloads an Excel file containing all student average grades.
        /// </summary>
        /// <returns>An Excel (.xlsx) file stream for the client to download.</returns>
        [HttpGet("export")]
        public IActionResult ExportToExcel()
        {
            try
            {
                var data = _dbService.GetStudentAverages();
                ExcelPackage.License.SetNonCommercialPersonal("CarloBactol"); // Set license for EPPlus

                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("StudentGrades");
                    
                    // Add Headers
                    worksheet.Cells[1, 1].Value = "Student Name";
                    worksheet.Cells[1, 2].Value = "Average Grade";

                    // Add Data
                    for (int i = 0; i < data.Count; i++)
                    {
                        worksheet.Cells[i + 2, 1].Value = data[i].StudentName;
                        worksheet.Cells[i + 2, 2].Value = data[i].AverageGrade;
                    }

                    // Format as Table
                    worksheet.Cells[1, 1, data.Count + 1, 2].AutoFitColumns();

                    var stream = new MemoryStream();
                    package.SaveAs(stream);
                    stream.Position = 0;
                    string excelName = $"GradesReport-{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                    
                    return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting student averages to Excel");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}