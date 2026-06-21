using StudentManagement.API.Models;

namespace StudentManagement.API.Data
{
    public interface IStudentRepository
    {
        Task<int> CreateStudent(Student student);
        Task<bool> UpdateStudent(Student student);
        Task<bool> DeleteStudent(int id);
        Task<IEnumerable<Student>> GetAllStudents();
        Task<Student?> GetStudentById(int id);
        Task<IEnumerable<Student>> GetStudentsByDepartment(int departmentId);
    }
}