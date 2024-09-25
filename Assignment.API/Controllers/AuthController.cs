using Assignment.API.Entity;
using Assignment.API.Model.Dto.Request;
using Assignment.API.Model.Dto.Response;
using Assignment.API.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Assignment.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthService _authService;
        protected ResponseDto<object> _response; 

        public AuthController(IAuthService authService, UserManager<ApplicationUser> userManager)
        {
            _authService = authService;
            _response = new ResponseDto<object>(); 
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto model)
        {
            var registrationResponse = await _authService.Register(model);

            if (!registrationResponse.IsSuccess)
            {
                return BadRequest(registrationResponse); // Return 400 with errors if not successful
            }

            return Ok(registrationResponse); // Return 200 if successful
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {
            var loginResponse = await _authService.Login(model);
            var response = new ResponseDto<LoginResponseDto>(); 

            if (loginResponse.User == null)
            {
                response.IsSuccess = false;
                response.Message = "Username or password is incorrect";
                response.Errors.Add("Invalid credentials");
                return BadRequest(response);
            }

            response.Result = loginResponse; 
            response.IsSuccess = true;
            response.Message = "Login Successful";
            return Ok(response);
        }

    }
}
