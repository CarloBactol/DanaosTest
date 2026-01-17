using System.Data;
using System.Data.SqlClient;
using DanaosBackend.Models;

namespace DanaosBackend.Services
{
    /// <summary>
    /// Service responsible for efficient raw SQL data access.
    /// Handles connections and query execution for the Student and Grades tables.
    /// </summary>
    public class DatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        /// <summary>
        /// Reads student names and calculates their average grade directly in the database.
        /// This is O(N) efficiency as the database engine handles the math.
        /// </summary>
        public List<StudentGradeDto> GetStudentAverages()
        {
            var results = new List<StudentGradeDto>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                
                // OPTIMIZATION: Use JOIN and GROUP BY to let SQL Server do the heavy lifting.
                // Only fetch the final result, saving network bandwidth.
                string query = @"
                    SELECT s.Name, AVG(g.Grade) as AverageGrade
                    FROM Student s
                    JOIN Grades g ON s.Id = g.Student_Id
                    GROUP BY s.Name";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            results.Add(new StudentGradeDto
                            {
                                StudentName = reader["Name"].ToString(),
                                // Handle potential NULLs if a student has no grades yet
                                AverageGrade = reader["AverageGrade"] != DBNull.Value 
                                    ? Convert.ToDouble(reader["AverageGrade"]) 
                                    : 0.0
                            });
                        }
                    }
                }
            }
            return results;
        }

        /// <summary>
        /// Reads course data for the analytics chart.
        /// </summary>
        public List<CourseAverageDto> GetCourseAverages()
        {
            var results = new List<CourseAverageDto>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
                    SELECT Course_Name, AVG(Grade) as AverageGrade
                    FROM Grades
                    GROUP BY Course_Name";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            results.Add(new CourseAverageDto
                            {
                                CourseName = reader["Course_Name"].ToString(),
                                AverageGrade = reader["AverageGrade"] != DBNull.Value 
                                    ? Convert.ToDouble(reader["AverageGrade"]) 
                                    : 0.0
                            });
                        }
                    }
                }
            }
            return results;
        }
    }
}