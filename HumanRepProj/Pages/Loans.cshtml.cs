using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;

namespace HumanRepProj.Pages
{
    public class LoansModel : PageModel
    {
        public int TotalLoans { get; set; } = 1_248;
        public int ActiveLoans { get; set; } = 892;
        public int PendingLoans { get; set; } = 56;
        public decimal DefaultRate { get; set; } = 0.045m;

        public List<Loan> Loans { get; set; } = new List<Loan>();

        public void OnGet()
        {
            // In a real app, this would come from a database or service
            Loans = new List<Loan>
            {
                new Loan
                {
                    Id = 10045,
                    BorrowerName = "John Smith",
                    BorrowerId = "CUS-2045",
                    BorrowerAvatar = "https://randomuser.me/api/portraits/men/32.jpg",
                    Amount = 25000m,
                    Type = "Mortgage",
                    TypePillClass = "loan-pill-primary",
                    Status = "Active",
                    StatusClass = "status-active",
                    IssuedDate = DateTime.Now.AddDays(-120),
                    DueDate = DateTime.Now.AddDays(30)
                },
                new Loan
                {
                    Id = 10046,
                    BorrowerName = "Sarah Johnson",
                    BorrowerId = "CUS-3156",
                    BorrowerAvatar = "https://randomuser.me/api/portraits/women/44.jpg",
                    Amount = 15000m,
                    Type = "Personal",
                    TypePillClass = "loan-pill-success",
                    Status = "Pending",
                    StatusClass = "status-pending",
                    IssuedDate = DateTime.Now.AddDays(-5),
                    DueDate = DateTime.Now.AddDays(45)
                },
                new Loan
                {
                    Id = 10047,
                    BorrowerName = "Michael Brown",
                    BorrowerId = "CUS-1023",
                    BorrowerAvatar = "https://randomuser.me/api/portraits/men/75.jpg",
                    Amount = 35000m,
                    Type = "Auto",
                    TypePillClass = "loan-pill-warning",
                    Status = "Active",
                    StatusClass = "status-active",
                    IssuedDate = DateTime.Now.AddDays(-90),
                    DueDate = DateTime.Now.AddDays(-5)
                },
                new Loan
                {
                    Id = 10048,
                    BorrowerName = "Emily Davis",
                    BorrowerId = "CUS-4589",
                    BorrowerAvatar = "https://randomuser.me/api/portraits/women/68.jpg",
                    Amount = 12000m,
                    Type = "Student",
                    TypePillClass = "loan-pill-info",
                    Status = "Approved",
                    StatusClass = "status-approved",
                    IssuedDate = DateTime.Now.AddDays(-15),
                    DueDate = DateTime.Now.AddDays(365)
                },
                new Loan
                {
                    Id = 10049,
                    BorrowerName = "Robert Wilson",
                    BorrowerId = "CUS-7891",
                    BorrowerAvatar = "https://randomuser.me/api/portraits/men/81.jpg",
                    Amount = 500m,
                    Type = "Business Loan",
                    TypePillClass = "loan-pill-danger",
                    Status = "Rejected",
                    StatusClass = "status-rejected",
                    IssuedDate = DateTime.Now.AddDays(-30),
                    DueDate = DateTime.Now.AddDays(-10)
                }
            };
        }
    }

    // Assuming the Loan class is defined here for completeness
    public class Loan
    {
        public int Id { get; set; }
        public string BorrowerName { get; set; }
        public string BorrowerId { get; set; }
        public string BorrowerAvatar { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; }
        public string TypePillClass { get; set; }
        public string Status { get; set; }
        public string StatusClass { get; set; }
        public DateTime IssuedDate { get; set; }
        public DateTime DueDate { get; set; }
    }
}