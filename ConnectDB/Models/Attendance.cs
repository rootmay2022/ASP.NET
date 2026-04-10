public class Attendance
{
    public int Id { get; set; }
    public int ScheduleId { get; set; } // Buổi học nào
    public int StudentId { get; set; }  // Sinh viên nào
    public bool IsPresent { get; set; } // Có mặt hay không
    public DateTime Date { get; set; }  // Ngày điểm danh
}