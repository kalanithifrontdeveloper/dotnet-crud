using System.ComponentModel.DataAnnotations;

namespace StudentManagement.API.DTOs
{
    public class StudentDto
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [Range(1, 100)]
        public int Age { get; set; }
        
        [Required]
        public DateTime DateOfBirth { get; set; }
        
        [Required]
        public string Status { get; set; } = "Active";
        
        public int? DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public string? DepartmentCode { get; set; }
    }

    public class CreateStudentDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [Range(1, 100)]
        public int Age { get; set; }
        
        [Required]
        public DateTime DateOfBirth { get; set; }
        
        [Required]
        public string Status { get; set; } = "Active";
        
        public int? DepartmentId { get; set; }
    }

    public class UpdateStudentDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [Range(1, 100)]
        public int Age { get; set; }
        
        [Required]
        public DateTime DateOfBirth { get; set; }
        
        [Required]
        public string Status { get; set; } = "Active";
        
        public int? DepartmentId { get; set; }
    }
}