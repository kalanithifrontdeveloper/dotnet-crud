using Dapper;
using StudentManagement.API.Models;
using System.Data;
using Microsoft.Data.SqlClient; 

namespace StudentManagement.API.Data
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public DepartmentRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection") 
                ?? throw new ArgumentNullException("Connection string not found");
        }

        public async Task<int> CreateDepartment(Department department)
        {
            using var connection = new SqlConnection(_connectionString);
            
            var parameters = new DynamicParameters();
            parameters.Add("@Name", department.Name);
            parameters.Add("@Code", department.Code);
            parameters.Add("@Description", string.IsNullOrEmpty(department.Description) ? DBNull.Value : (object)department.Description);

            var result = await connection.ExecuteScalarAsync<int>(
                "usp_InsertDepartment",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result;
        }

        public async Task<bool> UpdateDepartment(Department department)
        {
            using var connection = new SqlConnection(_connectionString);
            
            var parameters = new DynamicParameters();
            parameters.Add("@Id", department.Id);
            parameters.Add("@Name", department.Name);
            parameters.Add("@Code", department.Code);
            parameters.Add("@Description", string.IsNullOrEmpty(department.Description) ? DBNull.Value : (object)department.Description);

            var rowsAffected = await connection.ExecuteScalarAsync<int>(
                "usp_UpdateDepartment",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return rowsAffected > 0;
        }

        public async Task<bool> DeleteDepartment(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);

            await connection.ExecuteAsync(
                "usp_DeleteDepartment",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return true;
        }

        public async Task<IEnumerable<Department>> GetAllDepartments()
        {
            using var connection = new SqlConnection(_connectionString);
            
            var departments = await connection.QueryAsync<Department>(
                "usp_GetAllDepartments",
                commandType: CommandType.StoredProcedure
            );

            return departments;
        }

        public async Task<Department?> GetDepartmentById(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);

            var department = await connection.QuerySingleOrDefaultAsync<Department>(
                "usp_GetDepartmentById",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return department;
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