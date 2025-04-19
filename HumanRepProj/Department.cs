// Models/Department.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HumanRepProj.Models
{
    public class Department : BaseEntity  // Inherits from BaseEntity for timestamps
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DepartmentID { get; set; }

        [Required]
        [StringLength(100)]  // Added length constraint
        [Column("DepartmentName")]  // Matches database column name
        [Display(Name = "Department Name")]  // For UI display
        public string Name { get; set; } = string.Empty;

        // Navigation property
        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

        // Timestamp properties inherited from BaseEntity:
        // public DateTime CreatedAt { get; set; }
        // public DateTime UpdatedAt { get; set; }
    }
}