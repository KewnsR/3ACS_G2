// Models/Employee.cs
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HumanRepProj.Models
{
    public class Employee : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmployeeID { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format")]
        [StringLength(20)]
        public string? PhoneNumber { get; set; }  // Made explicitly nullable

        [StringLength(500)]
        public string? Address { get; set; }     // Made explicitly nullable

        [Required(ErrorMessage = "Date of birth is required")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        [StringLength(10)]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Department is required")]
        public int DepartmentID { get; set; }

        [ForeignKey("DepartmentID")]
        public Department? Department { get; set; }

        [Required(ErrorMessage = "Position is required")]
        [StringLength(100)]
        public string Position { get; set; } = string.Empty;

        [Required(ErrorMessage = "Salary is required")]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue, ErrorMessage = "Salary must be positive")]
        public decimal Salary { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateHired { get; set; }

        [StringLength(20)]
        public string EmploymentType { get; set; } = "Full-time";

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "Active";

        public int? ManagerID { get; set; } // This is correct - keep as nullable

        [ForeignKey("ManagerID")]
        [ValidateNever] // Add this attribute
        public Employee? Manager { get; set; }

        public bool IsManager { get; set; }

        [NotMapped]
        [ValidateNever]  // Add this attribute
        public string FullName => $"{FirstName} {LastName}";

        [NotMapped]
        [ValidateNever]  // Add this attribute
        public ICollection<Employee> Subordinates { get; set; } = new List<Employee>();
    }
}