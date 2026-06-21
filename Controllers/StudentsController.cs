using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using StudentManagement.API.Data;
using StudentManagement.API.DTOs;
using StudentManagement.API.Models;

namespace StudentManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentRepository _repository;
        private readonly IDepartmentRepository _departmentRepository;

        public StudentsController(
            IStudentRepository repository, 
            IDepartmentRepository departmentRepository)
        {
            _repository = repository;
            _departmentRepository = departmentRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetStudents()
        {
            try
            {
                var students = await _repository.GetAllStudents();
                var studentDtos = students.Select(s => new StudentDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Age = s.Age,
                    DateOfBirth = s.DateOfBirth,
                    Status = s.Status,
                    DepartmentId = s.DepartmentId,
                    DepartmentName = s.DepartmentName,
                    DepartmentCode = s.DepartmentCode
                });
                
                return Ok(studentDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StudentDto>> GetStudent(int id)
        {
            try
            {
                var student = await _repository.GetStudentById(id);
                
                if (student == null)
                    return NotFound($"Student with ID {id} not found");

                var studentDto = new StudentDto
                {
                    Id = student.Id,
                    Name = student.Name,
                    Age = student.Age,
                    DateOfBirth = student.DateOfBirth,
                    Status = student.Status,
                    DepartmentId = student.DepartmentId,
                    DepartmentName = student.DepartmentName,
                    DepartmentCode = student.DepartmentCode
                };

                return Ok(studentDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("department/{departmentId}")]
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetStudentsByDepartment(int departmentId)
        {
            try
            {
                var department = await _departmentRepository.GetDepartmentById(departmentId);
                if (department == null)
                    return NotFound($"Department with ID {departmentId} not found");

                var students = await _repository.GetStudentsByDepartment(departmentId);
                var studentDtos = students.Select(s => new StudentDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Age = s.Age,
                    DateOfBirth = s.DateOfBirth,
                    Status = s.Status,
                    DepartmentId = s.DepartmentId,
                    DepartmentName = s.DepartmentName,
                    DepartmentCode = s.DepartmentCode
                });

                return Ok(studentDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateStudent([FromBody] CreateStudentDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (createDto.DepartmentId.HasValue)
                {
                    var department = await _departmentRepository.GetDepartmentById(createDto.DepartmentId.Value);
                    if (department == null)
                        return BadRequest($"Department with ID {createDto.DepartmentId} not found");
                }

                var student = new Student
                {
                    Name = createDto.Name,
                    Age = createDto.Age,
                    DateOfBirth = createDto.DateOfBirth,
                    Status = createDto.Status,
                    DepartmentId = createDto.DepartmentId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var id = await _repository.CreateStudent(student);
                return CreatedAtAction(nameof(GetStudent), new { id }, id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(int id, [FromBody] UpdateStudentDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var existingStudent = await _repository.GetStudentById(id);
                if (existingStudent == null)
                    return NotFound($"Student with ID {id} not found");

                if (updateDto.DepartmentId.HasValue)
                {
                    var department = await _departmentRepository.GetDepartmentById(updateDto.DepartmentId.Value);
                    if (department == null)
                        return BadRequest($"Department with ID {updateDto.DepartmentId} not found");
                }

                existingStudent.Name = updateDto.Name;
                existingStudent.Age = updateDto.Age;
                existingStudent.DateOfBirth = updateDto.DateOfBirth;
                existingStudent.Status = updateDto.Status;
                existingStudent.DepartmentId = updateDto.DepartmentId;
                existingStudent.UpdatedAt = DateTime.UtcNow;

                var result = await _repository.UpdateStudent(existingStudent);
                
                if (!result)
                    return StatusCode(500, "Failed to update student");

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            try
            {
                var existingStudent = await _repository.GetStudentById(id);
                if (existingStudent == null)
                    return NotFound($"Student with ID {id} not found");

                var result = await _repository.DeleteStudent(id);
                
                if (!result)
                    return StatusCode(500, "Failed to delete student");

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}