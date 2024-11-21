using System.Data.SqlClient;
using Dapper;
using DynamicGridGeneration.Model;

namespace DynamicGridGeneration.Services
{
    public class StudentDataService
    {
        private readonly string _connectionString = "";
        private readonly IConfiguration _configuration;

        public StudentDataService(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<Student>> GetStudentsAsync()
        {
            using(var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT * FROM Students";
                var StudentData = (await connection.QueryAsync<Student>(query)).ToList();
                return StudentData;
            }
        }

        public async Task UpdateStudentAsync(Student student)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = @"
        UPDATE Students 
        SET Name = @Name, Age = @Age, DateOfBirth = @DateOfBirth, Gender = @Gender, Image = @Image 
        WHERE Id = @Id";
            await connection.ExecuteAsync(query, student);
        }



    }
}
