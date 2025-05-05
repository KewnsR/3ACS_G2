using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HumanRepProj.Models
{
    public class AttendanceRecord
    {
        [Key]
        public int AttendanceID { get; set; }

        [Required]
        public int EmployeeID { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime AttendanceDate { get; set; }

        [DataType(DataType.Time)]
        public DateTime? TimeIn { get; set; }

        [DataType(DataType.Time)]
        public DateTime? TimeOut { get; set; }

        [StringLength(20)]
        public string Status { get; set; } // Present, Absent, Late, etc.

        [DataType(DataType.Time)]
        public DateTime? LunchStartTime { get; set; }

        [DataType(DataType.Time)]
        public DateTime? LunchEndTime { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Navigation property
        [ForeignKey("EmployeeID")]
        public virtual Employee Employee { get; set; }
    }
}