using ConnectDB.Models;

public class LeaveRequest
{
    public int Id { get; set; }
    public int TeacherId { get; set; }

    // THÊM DÒNG NÀY VÀO NGAY: Đây gọi là Navigation Property
    // Nó giúp C# hiểu là từ bảng LeaveRequest có thể nhảy sang bảng Teacher
    public virtual Teacher? Teacher { get; set; }

    public DateTime OffDate { get; set; }
    public string Reason { get; set; }
    public string Status { get; set; } = "Pending";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}