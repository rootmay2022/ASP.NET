namespace ConnectDB.DTO
{
    public class AcademicSummaryDto
    {
        public string StudentName { get; set; } = string.Empty;
        public string ClassName { get; set; } = string.Empty;
        public double AverageGPA { get; set; }

        // Danh sách chi tiết từng môn học
        public List<SubjectGradeDto> SubjectDetails { get; set; } = new();
    }

    // Class phụ để chứa thông tin từng môn
    public class SubjectGradeDto
    {
        public string SubjectName { get; set; } = string.Empty;
        public double Score { get; set; }
        public string Grade { get; set; } = string.Empty; // A, B, C, D...
        public string Status { get; set; } = string.Empty; // Đạt hoặc Học lại
    }
}