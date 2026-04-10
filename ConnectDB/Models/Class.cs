using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConnectDB.Models
{
    public class Class
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string ClassName { get; set; } = string.Empty;

        // --- THÊM 2 DÒNG NÀY ĐỂ KẾT NỐI VỚI KHOA ---
        public int FacultyId { get; set; } // Khóa ngoại

        [ForeignKey("FacultyId")]
        public virtual Faculty? Faculty { get; set; } // Thuộc tính điều hướng
        // -------------------------------------------

        public ICollection<Student>? Students { get; set; }
    }
}