using Microsoft.AspNetCore.Mvc;
using StudentManagement.API.Data;
using StudentManagement.API.DTOs;
using StudentManagement.API.Models;

namespace StudentManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentRepository _repository;
        private readonly IStudentRepository _studentRepository;

        public DepartmentsController(IDepartmentRepository repository, IStudentRepository studentRepository)
        {
            _repository = repository;
            _studentRepository = studentRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetDepartments()
        {
            try
            {
                var departments = await _repository.GetAllDepartments();
                var departmentDtos = departments.Select(d => new DepartmentDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    Code = d.Code,
                    Description = d.Description,
                    StudentCount = d.StudentCount
                });
                
                return Ok(departmentDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DepartmentDto>> GetDepartment(int id)
        {
            try
            {
                var department = await _repository.GetDepartmentById(id);
                
                if (department == null)
                    return NotFound($"Department with ID {id} not found");

                var departmentDto = new DepartmentDto
                {
                    Id = department.Id,
                    Name = department.Name,
                    Code = department.Code,
                    Description = department.Description,
                    StudentCount = department.StudentCount
                };

                return Ok(departmentDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}/students")]
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetDepartmentStudents(int id)
        {
            try
            {
                var department = await _repository.GetDepartmentById(id);
                if (department == null)
                    return NotFound($"Department with ID {id} not found");

                var students = await _studentRepository.GetStudentsByDepartment(id);
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
        public async Task<ActionResult<int>> CreateDepartment([FromBody] CreateDepartmentDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var department = new Department
                {
                    Name = createDto.Name,
                    Code = createDto.Code,
                    Description = createDto.Description
                };

                var id = await _repository.CreateDepartment(department);
                return CreatedAtAction(nameof(GetDepartment), new { id }, id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepartment(int id, [FromBody] UpdateDepartmentDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var existingDepartment = await _repository.GetDepartmentById(id);
                if (existingDepartment == null)
                    return NotFound($"Department with ID {id} not found");

                existingDepartment.Name = updateDto.Name;
                existingDepartment.Code = updateDto.Code;
                existingDepartment.Description = updateDto.Description;

                var result = await _repository.UpdateDepartment(existingDepartment);
                
                if (!result)
                    return StatusCode(500, "Failed to update department");

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            try
            {
                var existingDepartment = await _repository.GetDepartmentById(id);
                if (existingDepartment == null)
                    return NotFound($"Department with ID {id} not found");

                // Check if department has students
                var students = await _studentRepository.GetStudentsByDepartment(id);
                if (students.Any())
                {
                    return BadRequest($"Cannot delete department with {students.Count()} existing students. Please reassign or remove students first.");
                }

                var result = await _repository.DeleteDepartment(id);
                
                if (!result)
                    return StatusCode(500, "Failed to delete department");

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}