using System.ComponentModel.DataAnnotations;

namespace ConnectDB.Models
{
    public class Teacher
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string TeacherCode { get; set; } = string.Empty;

        [Required]
        public string FullName { get; set; } = string.Empty;
        public int UserId { get; set; } // ID của tài khoản liên kết
        public User? User { get; set; } // Navigation property
        public string? Email { get; set; }
        public string? Phone { get; set; }
    }
}