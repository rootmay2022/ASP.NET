using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConnectDB.Models
{
    public class Score
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int StudentId { get; set; }

        [Required]
        public int SubjectId { get; set; }

        public float KT1 { get; set; } = 0;
        public float KT2 { get; set; } = 0;
        public float DiemThi { get; set; } = 0;
        public float DiemTrungBinh { get; set; } = 0;

        [StringLength(50)]
        public string? KetQua { get; set; } = "Chưa có"; // Gán mặc định để tránh lỗi Null

        // Khai báo Foreign Key rõ ràng để EF Core không tạo thêm cột rác
        [ForeignKey("StudentId")]
        public virtual Student? Student { get; set; }

        [ForeignKey("SubjectId")]
        public virtual Subject? Subject { get; set; }
    }
}