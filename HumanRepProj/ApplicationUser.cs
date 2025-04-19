// Models/ApplicationUser.cs
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HumanRepProj.Models
{
    public class ApplicationUser : BaseEntity  // Inherits from BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LoginID { get; set; }

        [Required]
        [ForeignKey("Employee")]
        public int EmployeeID { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [StringLength(255)]
        public string Password { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? LastLogin { get; set; }

        [DefaultValue(0)]  // Default value for new users
        public int FailedAttempts { get; set; } = 0;

        [DefaultValue(false)]  // Default value for new users
        public bool IsLocked { get; set; } = false;

        // Navigation property to Employee
        public virtual Employee Employee { get; set; }

        // The following properties are inherited from BaseEntity:
        // public DateTime CreatedAt { get; set; }
        // public DateTime UpdatedAt { get; set; }
    }
}