// Models/Department.cs
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HumanRepProj.Models
{
    public class Department
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DepartmentID { get; set; }

        // Match these property names to your actual database columns
        [Required]
        [Column("DepartmentName")] // Use the actual column name from your database
        public string Name { get; set; } = string.Empty;


        // Navigation property
        public virtual ICollection<Employee>? Employees { get; set; }
    }
}