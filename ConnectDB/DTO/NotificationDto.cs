namespace ConnectDB.DTO
{
    public class NotificationDto
    {
        public string TargetType { get; set; } = string.Empty;
        public int? TargetId { get; set; }
        public int? ClassId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string? Type { get; set; }
    }
}