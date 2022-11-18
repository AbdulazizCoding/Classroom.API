using Classroom.API.Dtos;
using Classroom.API.Entities;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Classroom.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("[controller]/Register")]
        public async Task<IActionResult> Register([FromForm]SignUpUserDto userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Iltimos Fo'rmani to'g'ri kiriting!");

            if (userDto.Password != userDto.ConfirmPassword)
                return BadRequest("Passwordni to'g'ri kiriting!");

            if (await _userManager.Users.AnyAsync(u => u.UserName == userDto.UserName))
                return BadRequest("Uchbu username band.");

            var user = userDto.Adapt<AppUser>();

            await _userManager.CreateAsync(user);

            await _signInManager.SignInAsync(user, isPersistent: true);

            return Ok("Tabriklayman siz muvofaqqiyatli ro'yxatdan o'tdingiz! ");
        }

        [HttpPost("[controller]/SignIn")]
        public async Task<IActionResult> SignIn([FromForm] SignInUserDto userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Iltimos Fo'rmani to'g'ri kiriting!");

            if (!await _userManager.Users.AnyAsync(u => u.UserName == userDto.UserName))
                return NotFound("Username topilmadi.");

            var result = await _signInManager.PasswordSignInAsync(userDto.UserName, userDto.Password, true, false);

            if (!result.Succeeded)
                return BadRequest();

            return Ok("Siz muvafaqqiyatli akkauntingizga kirdingiz!");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);

            var userDto = user.Adapt<UserDto>();

            return Ok(userDto);
        }
    }
}
