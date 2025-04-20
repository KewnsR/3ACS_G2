using System;
using System.ComponentModel.DataAnnotations;

namespace HumanRepProj.Models
{
    public class Department
    {
        public int DepartmentID { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Department name cannot exceed 100 characters.")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; }

        [Range(0, 100, ErrorMessage = "Performance must be between 0 and 100.")]
        public decimal Performance { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; } = DateTime.Now;

        [Range(0, double.MaxValue, ErrorMessage = "Budget must be a positive value.")]
        public decimal Budget { get; set; }

        [Required]
        public string Status { get; set; } = "Active";

        // Navigation property to represent the relationship with Employee
        public virtual ICollection<Employee> Employees { get; set; }
    }
}