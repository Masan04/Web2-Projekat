using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Projekat.Dto;
using Projekat.Interfaces;

namespace Projekat.Controllers
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

        [HttpPost("register")]
        public IActionResult RegisterUser([FromBody]UserRegisterDto userRegisterDto)
        {
            UserRegisterDto user = _userService.AddUser(userRegisterDto);

            if (user != null)
            {
                return Ok(user);
            }
            else
            {
                return BadRequest("User with the given email already exists!");
            }

        }

        [HttpPost("login")]
        public IActionResult LoginUser([FromBody]UserLoginDto userLoginDto)
        {
            var token = _userService.LoginUser(userLoginDto);
            return token == null ? BadRequest(token) : Ok(token);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin,buyer,seller")]
        public IActionResult UpdateUser(long id,[FromBody]UserRegisterDto userRegisterDto)
        {
            UserRegisterDto user = _userService.UpdateUser(id, userRegisterDto);
            if (user != null)
            {
                return Ok(user);
            }
            else
            {
                return BadRequest("User with the same email address already exists!");
            }
        }

        [HttpGet("{email}")]
        [Authorize(Roles ="admin,buyer,seller")]
        public IActionResult GetByEmail(string email)
        {
            UserRegisterDto userRegisterDto = _userService.GetByEmail(email);
            if (userRegisterDto == null)
                return BadRequest("Desila se greska prilikom preuzimanja korisnika!");
            return Ok(userRegisterDto);
        }

        [HttpGet("GetById/{id}")]
        [Authorize(Roles = "admin,buyer,seller")]
        public IActionResult GetById(long id)
        {
            UserRegisterDto user = _userService.GetUserById(id);
            if (user == null)
                 return BadRequest("Error occurred while trying to get the user!");
            return Ok(user);
            
        }

        [HttpGet("all")]
        [Authorize(Roles = "admin")]
        public IActionResult GetAll()
        {
            List<UserRegisterDto> users = new List<UserRegisterDto>();
            users = _userService.GetAll();
            if (users == null)
                return BadRequest("An errror occurred while trying to get all users!");
            return Ok(users);
        }

        [HttpPost("loginGoogle")]
        public IActionResult LoginGoogle([FromBody] UserLoginDto userLoginDto)
        {
            var token = _userService.LoginUser(userLoginDto);
            return token == null ? BadRequest(token) : Ok(token);
        }
    }
}
