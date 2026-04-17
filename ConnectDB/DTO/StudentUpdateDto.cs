namespace ConnectDB.DTO
{
    public class StudentUpdateDto
    {
        public string FullName { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public DateTime Birthday { get; set; }
    }
}