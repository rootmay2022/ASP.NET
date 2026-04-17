using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConnectDB.Models
{
    public class Schedule
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string? Note { get; set; }
        public DateTime LearnDate { get; set; } // Ngày học
        public int Slot { get; set; }           // Tiết học (1, 2, 3...)
        public string DayOfWeek { get; set; } = string.Empty; // Thứ 2, Thứ 3...
        public string Period { get; set; } = string.Empty;    // Tiết 1-3, 4-6...
        public string Room { get; set; } = string.Empty;      // Phòng học

        public int SubjectId { get; set; }
        [ForeignKey("SubjectId")]
        public Subject? Subject { get; set; }

        public int TeacherId { get; set; }
        [ForeignKey("TeacherId")]
        public Teacher? Teacher { get; set; }

        public int ClassId { get; set; }
        [ForeignKey("ClassId")]
        public Class? Class { get; set; }
    }
}