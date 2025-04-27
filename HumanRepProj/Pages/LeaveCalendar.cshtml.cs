using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace HumanRepProj.Pages
{
    public class LeaveCalendarModel : PageModel
    {
        public List<CalendarEvent> LeaveEvents { get; set; }

        public void OnGet()
        {
            LeaveEvents = new List<CalendarEvent>
            {
                new CalendarEvent
                {
                    Title = "John Doe - Vacation",
                    Start = "2025-04-28",
                    End = "2025-05-02",  // Adding a day to make the end date inclusive
                    AllDay = true
                },
                new CalendarEvent
                {
                    Title = "Jane Smith - Sick Leave",
                    Start = "2025-04-30",
                    End = "2025-05-03",  // Adding a day to make the end date inclusive
                    AllDay = true
                }
            };
        }

        public class CalendarEvent
        {
            public string Title { get; set; }
            public string Start { get; set; }
            public string End { get; set; }
            public bool AllDay { get; set; }
        }
    }
}