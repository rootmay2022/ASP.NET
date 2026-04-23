public class Notification
{
    public int Id { get; set; }
    public int SenderId { get; set; }

    // Thêm = string.Empty; để C# không mắng m nữa
    public string SenderRole { get; set; } = string.Empty;
    public string TargetType { get; set; } = string.Empty;

    // Thêm dấu ? vì TargetId và ClassId có thể không có (nếu gửi cho toàn bộ GV)
    public int? TargetId { get; set; }
    public int? ClassId { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? Type { get; set; }
    public bool IsRead { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}