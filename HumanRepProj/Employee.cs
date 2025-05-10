using HumanRepProj.Controllers;
using HumanRepProj.Data;
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
        public string? PhoneNumber { get; set; }

        [StringLength(500)]
        public string? Address { get; set; }

        [Required(ErrorMessage = "Date of birth is required")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        [StringLength(10)]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Department is required")]
        public int DepartmentID { get; set; }

        [ForeignKey("DepartmentID")]
        [ValidateNever]
        public virtual Department? Department { get; set; }

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

        // Manager relationship (nullable)
        public int? ManagerID { get; set; }

        [ForeignKey("ManagerID")]
        [ValidateNever]
        public virtual Employee? Manager { get; set; }

        // Indicates if this employee is a manager of any department
        public bool IsManager { get; set; }

        // Navigation property for employees managed by this employee
        [InverseProperty("Manager")]
        [ValidateNever]
        public virtual ICollection<Employee> Subordinates { get; set; } = new List<Employee>();

        // Not mapped properties
        [NotMapped]
        [ValidateNever]
        public string FullName => $"{FirstName} {LastName}";

        [ValidateNever]
        public virtual ICollection<AttendanceRecord> AttendanceRecords { get; set; } = new List<AttendanceRecord>();

        [NotMapped]
        [ValidateNever]
        public string DepartmentName => Department?.Name ?? "Unassigned";

        // Face data relationship
        public int? FaceDataID { get; set; }

        [ForeignKey("FaceDataID")]
        [ValidateNever]
        public virtual FaceData? FaceData { get; set; }

        [ValidateNever]
        public virtual ICollection<Loans> Loans { get; set; } = new List<Loans>();

    }
}