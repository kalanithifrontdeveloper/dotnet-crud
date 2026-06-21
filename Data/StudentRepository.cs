using Dapper;
using StudentManagement.API.Models;
using System.Data;
using Microsoft.Data.SqlClient; 

namespace StudentManagement.API.Data
{
    
    public class StudentRepository : IStudentRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public StudentRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection") 
                ?? throw new ArgumentNullException("Connection string not found");
        }

        public async Task<int> CreateStudent(Student student)
        {
            using var connection = new SqlConnection(_connectionString);
            
            var parameters = new DynamicParameters();
            parameters.Add("@Name", student.Name);
            parameters.Add("@Age", student.Age);
            parameters.Add("@DateOfBirth", student.DateOfBirth);
            parameters.Add("@Status", student.Status);
            parameters.Add("@DepartmentId", student.DepartmentId.HasValue ? (object)student.DepartmentId.Value : DBNull.Value);

            var result = await connection.ExecuteScalarAsync<int>(
                "usp_InsertStudent",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result;
        }

        public async Task<bool> UpdateStudent(Student student)
        {
            using var connection = new SqlConnection(_connectionString);
            
            var parameters = new DynamicParameters();
            parameters.Add("@Id", student.Id);
            parameters.Add("@Name", student.Name);
            parameters.Add("@Age", student.Age);
            parameters.Add("@DateOfBirth", student.DateOfBirth);
            parameters.Add("@Status", student.Status);
            parameters.Add("@DepartmentId", student.DepartmentId.HasValue ? (object)student.DepartmentId.Value : DBNull.Value);

            var rowsAffected = await connection.ExecuteScalarAsync<int>(
                "usp_UpdateStudent",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return rowsAffected > 0;
        }

        public async Task<bool> DeleteStudent(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);

            await connection.ExecuteAsync(
                "usp_DeleteStudent",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return true;
        }

        public async Task<IEnumerable<Student>> GetAllStudents()
        {
            using var connection = new SqlConnection(_connectionString);
            
            var students = await connection.QueryAsync<Student>(
                "usp_GetAllStudents",
                commandType: CommandType.StoredProcedure
            );

            return students;
        }

        public async Task<Student?> GetStudentById(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);

            var student = await connection.QuerySingleOrDefaultAsync<Student>(
                "usp_GetStudentById",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return student;
        }

        public async Task<IEnumerable<Student>> GetStudentsByDepartment(int departmentId)
        {
            using var connection = new SqlConnection(_connectionString);
            
            var parameters = new DynamicParameters();
            parameters.Add("@DepartmentId", departmentId);

            var students = await connection.QueryAsync<Student>(
                "usp_GetStudentsByDepartment",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return students;
        }
    }
}