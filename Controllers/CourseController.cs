using Classroom.API.Data;
using Classroom.API.Dtos;
using Classroom.API.Entities;
using Classroom.API.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Classroom.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CourseController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
    
        public CourseController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost("[controller]/Create")]
        public async Task<IActionResult> Create([FromForm] CreateCourseDto courseDto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Formani to'g'ri to'ldiring!");

            var user = await _userManager.GetUserAsync(User);

            var course = new Course()
            {
                Name = courseDto.Name,
                Key = Guid.NewGuid().ToString("N"),
                Users = new List<UserCourse>()
                {
                    new UserCourse()
                    {
                        UserId = user.Id,
                        IsAdmin = true
                    }
                }
            };

            await _context.Courses.AddAsync(course);
            await _context.SaveChangesAsync();

            course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == course.Id);

            return Ok(course.ToDto());
        }

        [HttpGet("[controller]/All")]
        public async Task<IActionResult> GetAll()
        {
            var cources = await _context.Courses.ToListAsync();
            
            if (cources is null)
                return NotFound("Curslarni olishda hatolik");

            return Ok(cources?.Select(c => c.ToDto()).ToList());
        }

        [HttpGet("[controller]/id")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == id);
            if (course is null)
                return NotFound();

            return Ok(course?.ToDto());
        }

        [HttpPut("[controller]/Update")]
        public async Task<IActionResult> Update(Guid id, UpdateCourseDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == id);
            if (course is null)
                NotFound();

            course.Name = updateDto.Name;

            _context.Courses.Update(course);
            await _context.SaveChangesAsync();

            return Ok(course?.ToDto());
        }

        [HttpDelete("[controller]/Delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == id);
            if (course is null)
                NotFound();

            _context.Courses.Remove(course);
            _context.SaveChangesAsync();

            return Ok("Olib Tashlandi");
        }
    }
}
