using KutuphaneDataAccess.DTOs;
using KutuphaneServis.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KutuphaneApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("CreateUser")]
        public IActionResult CreateUser([FromBody] UserCreateDto user)
        {
            if (user == null)
            {
                return BadRequest("Kullanıcı bilgileri boş olamaz.");
            }
            var response = _userService.CreateUser(user);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response.Message);
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] UserLoginDto user)
        {
            if (user == null || (string.IsNullOrEmpty(user.Username) && string.IsNullOrEmpty(user.Email)) || string.IsNullOrEmpty(user.Password))
            {
                return BadRequest("Kullanıcı adı, e-posta adresi veya şifre boş olamaz.");
            }
            var response = _userService.Login(user);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response.Message);
        }
    }
}
