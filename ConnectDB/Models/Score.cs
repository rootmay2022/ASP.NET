using ConnectDB.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
public class Score
{
    [Key]
    public int Id { get; set; }
    public double Value { get; set; }

    public int StudentId { get; set; }
    // Thêm dấu ? ở đây để hết lỗi "Non-nullable"
    [JsonIgnore]
    public Student? Student { get; set; }

    public int SubjectId { get; set; }
    // Thêm dấu ? ở đây
    public Subject? Subject { get; set; }
}