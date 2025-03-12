using System;

namespace HumanRepProj.Models
{
    public class ApplicationUser
    {
        public int LoginID { get; set; } // Primary key
        public int EmployeeID { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public DateTime? LastLogin { get; set; }
        public int FailedAttempts { get; set; }
        public bool IsLocked { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
