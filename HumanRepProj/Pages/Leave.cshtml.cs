using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HumanRepProj.Pages
{
    public class LeaveModel : PageModel
    {
        [BindProperty]
        public LeaveInput Input { get; set; }

        public string Message { get; set; }

        // Static list to store leave requests (replace with database in production)
        public static List<LeaveRequest> LeaveRequests { get; } = new List<LeaveRequest>();

        // Property to expose all leave requests to the view
        public List<LeaveRequest> AllLeaveRequests => LeaveRequests;

        public void OnGet()
        {
            // Initial form state - leave requests are already available through AllLeaveRequests
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page(); // Show validation errors
            }

            // Convert LeaveInput to LeaveRequest and store it
            LeaveRequests.Add(new LeaveRequest
            {
                EmployeeName = Input.EmployeeName,
                LeaveType = Input.LeaveType,
                StartDate = Input.StartDate,
                EndDate = Input.EndDate,
                Reason = Input.Reason
            });

            Message = $"Leave request submitted for {Input.EmployeeName} from {Input.StartDate.ToShortDateString()} to {Input.EndDate.ToShortDateString()}.";

            // Clear the form
            ModelState.Clear();
            Input = new LeaveInput();

            return RedirectToPage();
        }

        // Handles logout from the form
        public async Task<IActionResult> OnPostLogoutAsync()
        {
            // Clear session and sign out
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync();

            return RedirectToPage("/Login");
        }

        public class LeaveInput
        {
            [Required(ErrorMessage = "Employee name is required")]
            [Display(Name = "Employee Name")]
            public string EmployeeName { get; set; }

            [Required(ErrorMessage = "Leave type is required")]
            [Display(Name = "Leave Type")]
            public string LeaveType { get; set; }

            [Required(ErrorMessage = "Start date is required")]
            [DataType(DataType.Date)]
            [Display(Name = "Start Date")]
            public DateTime StartDate { get; set; }

            [Required(ErrorMessage = "End date is required")]
            [DataType(DataType.Date)]
            [Display(Name = "End Date")]
            [DateGreaterThan("StartDate", ErrorMessage = "End date must be after start date")]
            public DateTime EndDate { get; set; }

            [Required(ErrorMessage = "Reason is required")]
            [StringLength(500, MinimumLength = 10, ErrorMessage = "Reason must be between 10-500 characters")]
            [Display(Name = "Reason for Leave")]
            public string Reason { get; set; }
        }

        public class LeaveRequest
        {
            public string EmployeeName { get; set; }
            public string LeaveType { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public string Reason { get; set; }
            public string AttachmentPath { get; set; }
        }


        // Custom validation attribute for date comparison
        public class DateGreaterThanAttribute : ValidationAttribute
        {
            private readonly string _comparisonProperty;

            public DateGreaterThanAttribute(string comparisonProperty)
            {
                _comparisonProperty = comparisonProperty;
            }

            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                var currentValue = (DateTime)value;
                var property = validationContext.ObjectType.GetProperty(_comparisonProperty);

                if (property == null)
                    throw new ArgumentException("Property with this name not found");

                var comparisonValue = (DateTime)property.GetValue(validationContext.ObjectInstance);

                if (currentValue <= comparisonValue)
                    return new ValidationResult(ErrorMessage);

                return ValidationResult.Success;
            }
        }
    }
}