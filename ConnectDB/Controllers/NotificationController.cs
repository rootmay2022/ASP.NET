using ConnectDB.Data;
using ConnectDB.DTO;
using ConnectDB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Đã thêm: Quan trọng nhất
using System.Security.Claims;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class NotificationController : ControllerBase
{
    private readonly AppDbContext _context;
    public NotificationController(AppDbContext context) { _context = context; }

    // 1. API GỬI THÔNG BÁO
    [HttpPost("send")]
    public async Task<IActionResult> SendNotification([FromBody] NotificationDto dto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var roleClaim = User.FindFirst(ClaimTypes.Role)?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || string.IsNullOrEmpty(roleClaim))
        {
            return Unauthorized("Không xác định được người gửi!");
        }

        var notification = new Notification
        {
            SenderId = int.Parse(userIdClaim),
            SenderRole = roleClaim,
            TargetType = dto.TargetType,
            TargetId = dto.TargetId,
            ClassId = dto.ClassId,
            Title = dto.Title ?? "Không có tiêu đề",
            Content = dto.Content ?? "",
            CreatedAt = DateTime.Now
        };

        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();
        return Ok(new { message = "Gửi thành công!" });
    }

    // 2. API LẤY THÔNG BÁO (Fix lỗi đỏ ở ToListAsync và Null)
    [HttpGet("my-notifications")]
    public async Task<IActionResult> GetMyNotifications()
    {
        // Sử dụng dấu ? để tránh lỗi null reference
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var roleClaim = User.FindFirst(ClaimTypes.Role)?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || string.IsNullOrEmpty(roleClaim))
        {
            return Unauthorized("Vui lòng đăng nhập!");
        }

        int userId = int.Parse(userIdClaim);
        string role = roleClaim;

        // Truy vấn thông báo
        var notifications = await _context.Notifications
            .Where(n => (n.TargetType == role && (n.TargetId == userId || n.TargetId == 0))
                     || (role == "Student" && n.TargetType == "Class" && _context.Students.Any(s => s.UserId == userId && s.ClassId == n.ClassId)))
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync(); // Hết lỗi nhờ "using Microsoft.EntityFrameworkCore"

        return Ok(notifications);
    }
}