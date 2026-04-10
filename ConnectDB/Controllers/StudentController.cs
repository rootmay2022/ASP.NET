using ConnectDB.Data;
using ConnectDB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ConnectDB.Controllers
{
    [Authorize(Roles = "Student")] // Chỉ cho phép Sinh viên truy cập Controller này
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly AppDbContext _context;
        public StudentController(AppDbContext context) { _context = context; }

        private int GetCurrentUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            return claim != null ? int.Parse(claim.Value) : 0;
        }

        // 1. Lấy hồ sơ cá nhân
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = GetCurrentUserId();
            var student = await _context.Students
                .Include(s => s.Class)
                .FirstOrDefaultAsync(s => s.UserId == userId);

            if (student == null) return NotFound(new { message = "Không tìm thấy hồ sơ sinh viên" });

            return Ok(student);
        }

        // 2. Xem thời khóa biểu cá nhân
        [HttpGet("my-schedules")]
        public async Task<IActionResult> GetMySchedules()
        {
            var userId = GetCurrentUserId();

            // Tìm thông tin sinh viên để lấy ClassId
            var student = await _context.Students.FirstOrDefaultAsync(s => s.UserId == userId);
            if (student == null) return NotFound("Không tìm thấy thông tin sinh viên.");

            // Lấy lịch học dựa trên ClassId
            var schedules = await _context.Schedules
                .Where(s => s.ClassId == student.ClassId)
                .Include(s => s.Subject)
                .Include(s => s.Teacher)
                .OrderBy(s => s.LearnDate)
                .ThenBy(s => s.Slot)
                .Select(s => new {
                    s.LearnDate,
                    s.Slot,
                    s.Room,
                    SubjectName = s.Subject != null ? s.Subject.SubjectName : "N/A",
                    TeacherName = s.Teacher != null ? s.Teacher.FullName : "N/A"
                })
                .ToListAsync();

            if (!schedules.Any()) return Ok(new { message = "Bạn chưa có lịch học nào." });

            return Ok(schedules);
        }

        // 3. Đổi mật khẩu
        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            var userId = GetCurrentUserId();
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return NotFound();

            if (user.Password?.Trim() != dto.OldPassword.Trim())
                return BadRequest(new { message = "Mật khẩu cũ không đúng" });

            user.Password = dto.NewPassword.Trim();
            await _context.SaveChangesAsync();
            return Ok(new { message = "Đổi mật khẩu thành công" });
        }

        // 4. Cập nhật hồ sơ
        [HttpPut("update-profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] StudentUpdateDto dto)
        {
            var userId = GetCurrentUserId();
            var student = await _context.Students.FirstOrDefaultAsync(s => s.UserId == userId);
            if (student == null) return NotFound();

            student.FullName = dto.FullName;
            student.Phone = dto.Phone;
            student.Address = dto.Address;
            student.Email = dto.Email;
            student.Birthday = dto.Birthday;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Cập nhật thành công" });
        }

        // 5. Bảng điểm và Xếp loại
        [HttpGet("academic-summary")]
        public async Task<IActionResult> GetSummary()
        {
            var userId = GetCurrentUserId();
            var student = await _context.Students
                .Include(s => s.Class)
                .Include(s => s.Scores!)
                .ThenInclude(sc => sc.Subject)
                .FirstOrDefaultAsync(s => s.UserId == userId);

            if (student == null) return NotFound();
            if (student.Scores == null || !student.Scores.Any())
                return Ok(new { message = "Chưa có điểm học tập" });

            var subjectDetails = student.Scores.Select(s => new SubjectGradeDto
            {
                SubjectName = s.Subject?.SubjectName ?? "N/A",
                Score = s.Value,
                Grade = s.Value >= 8.5 ? "A" : s.Value >= 7.0 ? "B" : s.Value >= 5.5 ? "C" : s.Value >= 4.0 ? "D" : "F",
                Status = s.Value >= 4.0 ? "Đạt" : "Học lại"
            }).ToList();

            double avg = student.Scores.Average(x => x.Value);
            string ranking = avg >= 8.0 ? "Giỏi" : avg >= 6.5 ? "Khá" : avg >= 5.0 ? "Trung bình" : "Yếu";

            return Ok(new AcademicSummaryDto
            {
                StudentName = student.FullName,
                ClassName = student.Class?.ClassName ?? "N/A",
                AverageGPA = Math.Round(avg, 2),
                Ranking = ranking,
                SubjectDetails = subjectDetails
            });
        }
    }

    // --- DTOs (Data Transfer Objects) ---
    public class ChangePasswordDto { public string OldPassword { get; set; } = ""; public string NewPassword { get; set; } = ""; }
    public class StudentUpdateDto { public string FullName { get; set; } = ""; public string? Phone { get; set; } public string? Address { get; set; } public string? Email { get; set; } public DateTime Birthday { get; set; } }
    public class AcademicSummaryDto
    {
        public string StudentName { get; set; } = "";
        public string ClassName { get; set; } = "";
        public double AverageGPA { get; set; }
        public string Ranking { get; set; } = "";
        public List<SubjectGradeDto> SubjectDetails { get; set; } = new();
    }
    public class SubjectGradeDto { public string SubjectName { get; set; } = ""; public double Score { get; set; } public string Grade { get; set; } = ""; public string Status { get; set; } = ""; }
}