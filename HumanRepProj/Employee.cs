// Models/Employee.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HumanRepProj.Models
{
    public class Employee : BaseEntity  // Inherits from BaseEntity for CreatedAt/UpdatedAt
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("EmployeeID")]
        public int EmployeeID { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; }

        [Phone]
        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [StringLength(500)]  // Increased from 200 to match DB schema
        public string Address { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        [Column(TypeName = "date")]  // Explicit column type
        public DateTime DateOfBirth { get; set; }

        [Required]
        [StringLength(10)]
        public string Gender { get; set; }

        [Required]
        [Display(Name = "Department")]
        public int DepartmentID { get; set; }

        [ForeignKey("DepartmentID")]
        public Department Department { get; set; }

        [Required]
        [Column("Position")]
        [StringLength(100)]
        public string Position { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Salary { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date Hired")]
        [Column(TypeName = "date")]  // Explicit column type
        public DateTime DateHired { get; set; }

        [StringLength(20)]  // Added length constraint
        public string EmploymentType { get; set; } = "Full-time";

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "Active";

        public int? ManagerID { get; set; }

        [ForeignKey("ManagerID")]
        public Employee Manager { get; set; }

        public bool IsManager { get; set; }

        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";

        // Navigation property for employees managed by this employee (if manager)
        public ICollection<Employee> Subordinates { get; set; }

        // The following properties are inherited from BaseEntity:
        // public DateTime CreatedAt { get; set; }
        // public DateTime UpdatedAt { get; set; }
    }
}