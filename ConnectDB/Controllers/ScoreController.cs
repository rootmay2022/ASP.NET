using ConnectDB.Data;
using ConnectDB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace ConnectDB.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,Teacher")] // <-- Admin và Teacher đều được vào
    public class ScoreController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ScoreController(AppDbContext context) { _context = context; }

        // Nhập điểm mới
        [HttpPost]
        public IActionResult AddScore(Score score)
        {
            _context.Scores.Add(score);
            _context.SaveChanges();
            return Ok(score);
        }

        // Xem bảng điểm của 1 sinh viên cụ thể
        [HttpGet("student/{studentId}")]
        public IActionResult GetStudentScores(int studentId)
        {
            var scores = _context.Scores
                .Include(s => s.Subject)
                .Where(s => s.StudentId == studentId)
                .ToList();
            return Ok(scores);
        }
    }
}