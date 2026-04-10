using System.ComponentModel.DataAnnotations;

namespace ConnectDB.Models
{
    public class Faculty
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string FacultyName { get; set; } = string.Empty;
    }
}