using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using HumanRepProj.Data;
using HumanRepProj.Models;

namespace HumanRepProj.Pages.Attendance
{
    [ApiController]
    [Route("/api/attendance")]
    public class AttendanceApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AttendanceApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("log")]
        public async Task<IActionResult> LogAttendance([FromBody] AttendanceDto dto)
        {
            var attendance = new AttendanceRecord
            {
                EmployeeID = dto.EmployeeId, // ✅ match your model
                AttendanceDate = DateTime.Today,
                Status = "Present",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _context.AttendanceRecords.Add(attendance);
            await _context.SaveChangesAsync();

            return Ok(new { success = true });
        }
        [HttpPost("verify-face")]
        public async Task<IActionResult> VerifyFace([FromBody] FaceVerifyDto dto)
        {
            var client = new HttpClient();
            var response = await client.PostAsJsonAsync("http://localhost:5000/api/attendance/verify-face", dto);
            var result = await response.Content.ReadFromJsonAsync<FaceVerifyResult>();

            return Ok(result);
        }
    }

        public class AttendanceDto
    {
        public int EmployeeId { get; set; }
    }

    public class FaceVerifyDto
    {
        public int EmployeeId { get; set; }
        public string Image { get; set; } // base64 string
    }

    public class FaceVerifyResult
    {
        public bool Match { get; set; }
        public string Error { get; set; }
    }


}
