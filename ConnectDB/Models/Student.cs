using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
namespace ConnectDB.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string StudentCode { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        // --- CÁC THÔNG TIN BỔ SUNG ĐỂ SINH VIÊN TỰ CẬP NHẬT ---

        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }

        [StringLength(10)]
        public string? Gender { get; set; } // Nam / Nữ / Khác

        [StringLength(15)]
        [Phone]
        public string? Phone { get; set; }

        [StringLength(100)]
        [EmailAddress]
        public string? Email { get; set; }

        [StringLength(255)]
        public string? Address { get; set; }

        // Trạng thái: Đang học, Bảo lưu, Đã tốt nghiệp
        public string Status { get; set; } = "Active";

        // ----------------------------------------------------

        // Liên kết với Lớp học
        public int ClassId { get; set; }
        [JsonIgnore]
        [ForeignKey("ClassId")]
        public Class? Class { get; set; }

        // Liên kết với tài khoản User (Để đăng nhập)
        public int? UserId { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }

        // Danh sách điểm số (Liên kết 1-Nhiều với bảng Score)
        // Dùng để hiển thị bảng điểm và tính GPA
        public ICollection<Score>? Scores { get; set; }
    }
}