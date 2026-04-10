namespace ConnectDB.Models
{
    public class LeaveRequest
    {
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public DateTime OffDate { get; set; }
        public string Reason { get; set; } = string.Empty; // Sửa lỗi Non-nullable
        public string Status { get; set; } = "Pending";
    }
}