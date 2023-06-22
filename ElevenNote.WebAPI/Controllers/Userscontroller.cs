using ElevenNote.Models.Token;
using ElevenNote.Models.User;
using ElevenNote.Services.Token;
using ElevenNote.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElevenNote.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Userscontroller : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public Userscontroller(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegister model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var registerResult = await _userService.RegisterUserAsync(model);
            if (registerResult)
                return Ok("User was registered.");
            else
                return StatusCode(500, "Internal Server Error.");
        }

        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0) return BadRequest("Invalid id.");

            var userDetail = await _userService.GetUserByIdAsync(id);

            if (userDetail is null) return NotFound();

            return Ok(userDetail);
        }

        [HttpPost("~/api/Token")]
        public async Task<IActionResult> GetToken([FromBody] TokenRequest request)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tokenResponse = await _tokenService.GetTokenAsync(request);
            if(tokenResponse is null) return BadRequest("Invalid username or password.");

            return Ok(tokenResponse);
        }
    }
}