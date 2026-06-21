using System.ComponentModel.DataAnnotations;

namespace StudentManagement.API.Models
{
    public class Student
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
        
        // Navigation properties (from SQL joins)
        public string? DepartmentName { get; set; }
        public string? DepartmentCode { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        // Remove this line if it exists:
        // public string? ImagePath { get; set; }
    }
}