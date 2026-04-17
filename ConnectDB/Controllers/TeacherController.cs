using ConnectDB.Data;
using ConnectDB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace ConnectDB.Controllers
{
    [Authorize(Roles = "Admin,Teacher")] // <-- Admin và Teacher đều được vào
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]

    public class TeacherController : ControllerBase
    {
        private readonly AppDbContext _context;
        public TeacherController(AppDbContext context) { _context = context; }

        private int GetCurrentTeacherId()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            return _context.Teachers.FirstOrDefault(t => t.UserId == userId)?.Id ?? 0;
        }

        // ================= 1. XEM LỊCH DẠY & ĐƠN NGHỈ =================
        [HttpGet("my-schedules")]
        public async Task<IActionResult> GetSchedules()
        {
            var teacherId = GetCurrentTeacherId();
            var data = await _context.Schedules
                .Include(s => s.Subject).Include(s => s.Class)
                .Where(s => s.TeacherId == teacherId).ToListAsync();
            return Ok(data);
        }

        [HttpPost("request-leave")]
        public async Task<IActionResult> RequestLeave([FromBody] LeaveRequest lr)
        {
            lr.TeacherId = GetCurrentTeacherId();
            lr.Status = "Pending";
            _context.LeaveRequests.Add(lr);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Đã gửi đơn xin nghỉ chờ Admin duyệt" });
        }

        // ================= 2. QUẢN LÝ SINH VIÊN & ĐIỂM DANH =================
        [HttpGet("class-students/{classId}")]
        public async Task<IActionResult> GetStudentsByClass(int classId)
        {
            // Lấy danh sách sinh viên trong lớp mà mình dạy
            var students = await _context.Students
                .Where(s => s.ClassId == classId)
                .Select(s => new { s.Id, s.StudentCode, s.FullName })
                .ToListAsync();
            return Ok(students);
        }

        [HttpPost("take-attendance")]
        public async Task<IActionResult> TakeAttendance([FromBody] List<AttendanceDto> list)
        {
            foreach (var item in list)
            {
                var att = new Attendance
                {
                    ScheduleId = item.ScheduleId,
                    StudentId = item.StudentId,
                    IsPresent = item.IsPresent,
                    Date = DateTime.Now
                };
                _context.Attendances.Add(att);
            }
            await _context.SaveChangesAsync();
            return Ok(new { message = "Điểm danh thành công!" });
        }

        // ================= 3. NHẬP & SỬA ĐIỂM =================
        [HttpPost("submit-scores")]
        public async Task<IActionResult> SubmitScore([FromBody] Score score)
        {
            var teacherId = GetCurrentTeacherId();
            // Kiểm tra xem giảng viên có dạy môn này cho lớp này không (Bảo mật)
            var canGrade = await _context.Schedules.AnyAsync(s => s.TeacherId == teacherId && s.SubjectId == score.SubjectId);
            if (!canGrade) return Forbid("Bạn không có quyền nhập điểm cho môn này");

            var exist = await _context.Scores
                .FirstOrDefaultAsync(s => s.StudentId == score.StudentId && s.SubjectId == score.SubjectId);

            if (exist != null)
            {
                exist.Value = score.Value; // Cập nhật nếu đã có
            }
            else
            {
                _context.Scores.Add(score); // Thêm mới nếu chưa có
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Lưu điểm thành công" });
        }
    }
    public class SubmitScoreDto
    {
        public int StudentId { get; set; }
        public int SubjectId { get; set; }
        public double Value { get; set; }
    }
    // DTO cho điểm danh
    public class AttendanceDto
    {
        public int ScheduleId { get; set; }
        public int StudentId { get; set; }
        public bool IsPresent { get; set; }
    }
}