using StudentManagement.API.Models;

namespace StudentManagement.API.Data
{
    public interface IDepartmentRepository
    {
        Task<int> CreateDepartment(Department department);
        Task<bool> UpdateDepartment(Department department);
        Task<bool> DeleteDepartment(int id);
        Task<IEnumerable<Department>> GetAllDepartments();
        Task<Department?> GetDepartmentById(int id);
        Task<IEnumerable<Student>> GetStudentsByDepartment(int departmentId);
    }
}