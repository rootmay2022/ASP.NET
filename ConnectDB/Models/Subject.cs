using System.ComponentModel.DataAnnotations;

namespace ConnectDB.Models
{
    public class Subject
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string SubjectName { get; set; } = string.Empty;

        public ICollection<Score>? Scores { get; set; }
    }
}