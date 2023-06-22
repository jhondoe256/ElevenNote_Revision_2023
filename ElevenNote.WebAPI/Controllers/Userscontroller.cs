using ElevenNote.Models.User;
using ElevenNote.Services.User;
using Microsoft.AspNetCore.Mvc;

namespace ElevenNote.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Userscontroller : ControllerBase
    {
        private readonly IUserService _userService;

        public Userscontroller(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegister model)
        {
            if(!ModelState.IsValid)
            return BadRequest(ModelState);

            var registerResult= await _userService.RegisterUserAsync(model);
            if(registerResult)
            return Ok("User was registered.");
            else
            return StatusCode(500,"Internal Server Error.");
        }
    }
}