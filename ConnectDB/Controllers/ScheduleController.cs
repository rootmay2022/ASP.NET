using ConnectDB.Data;
using ConnectDB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConnectDB.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class ScheduleController : ControllerBase
    {
        private readonly AppDbContext _context;

        // Constructor này sẽ sửa lỗi "The name '_context' does not exist"
        public ScheduleController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("create-schedule")]
        public async Task<IActionResult> CreateSchedule([FromBody] CreateScheduleDto dto)
        {
            // Kiểm tra trùng phòng học
            var isRoomBusy = await _context.Schedules.AnyAsync(s =>
                s.Room == dto.Room &&
                s.LearnDate.Date == dto.LearnDate.Date &&
                s.Slot == dto.Slot);

            if (isRoomBusy) return BadRequest(new { message = "Phòng học này đã có lớp sử dụng vào tiết này." });

            // Kiểm tra trùng lịch giảng viên
            var isTeacherBusy = await _context.Schedules.AnyAsync(s =>
                s.TeacherId == dto.TeacherId &&
                s.LearnDate.Date == dto.LearnDate.Date &&
                s.Slot == dto.Slot);

            if (isTeacherBusy) return BadRequest(new { message = "Giảng viên đã có lịch dạy vào thời gian này." });

            var schedule = new Schedule
            {
                ClassId = dto.ClassId,
                SubjectId = dto.SubjectId,
                TeacherId = dto.TeacherId,
                LearnDate = dto.LearnDate,
                Slot = dto.Slot,
                Room = dto.Room
            };

            _context.Schedules.Add(schedule);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Xếp lịch thành công!", data = schedule });
        }
    }

    // Khai báo DTO ở đây để sửa lỗi "CreateScheduleDto could not be found"
    public class CreateScheduleDto
    {
        public int ClassId { get; set; }
        public int SubjectId { get; set; }
        public int TeacherId { get; set; }
        public DateTime LearnDate { get; set; }
        public int Slot { get; set; }
        public string Room { get; set; } = string.Empty;
    }
}