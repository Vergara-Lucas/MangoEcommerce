using Mango.Services.AuthAPI.Models.DTO;
using Mango.Services.AuthAPI.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.AuthAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ResponseDTO _responseDTO;
        public AuthAPIController(IAuthService authService)
        {
            _authService = authService;
            _responseDTO = new ();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequest) 
        {
            var loginResponse = await _authService.Login(loginRequest);
            if (loginResponse.User == null) { 
                _responseDTO.IsSuccess = false;
                _responseDTO.Message = "Username or password is incorrect";
                return BadRequest(_responseDTO);
            }
            _responseDTO.Result = loginResponse;
            return Ok(_responseDTO);
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDTO registration)
        {
            var errorMesage = await _authService.Register(registration);
            if (!string.IsNullOrEmpty(errorMesage)) { 
                _responseDTO.IsSuccess = false;
                _responseDTO.Message = errorMesage;
                return BadRequest(_responseDTO);
            }
            return Ok(_responseDTO);
        }
        [HttpPost("assignRole")]
        public async Task<IActionResult> AssignRole([FromBody] RegistrationRequestDTO modelRole)
        {
            var assignRoleIsSuccesful = await _authService.AssignRole(modelRole.Email, modelRole.Role);
            if (!assignRoleIsSuccesful)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Message = "Error Encountered";
                return BadRequest(_responseDTO);
            }
            return Ok(_responseDTO);
        }
    }
}
