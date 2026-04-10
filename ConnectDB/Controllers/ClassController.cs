using ConnectDB.Data;
using ConnectDB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
namespace ConnectDB.Controllers
{
    [Authorize]
    [ApiController]
	[Route("api/[controller]")]
	public class ClassController : ControllerBase
	{
		private readonly AppDbContext _context;
		public ClassController(AppDbContext context) { _context = context; }

		[HttpGet]
		public IActionResult GetAll() => Ok(_context.Classes.ToList());

		[HttpPost]
		public IActionResult Create(Class c)
		{
			_context.Classes.Add(c);
			_context.SaveChanges();
			return Ok(c);
		}
	}
}
