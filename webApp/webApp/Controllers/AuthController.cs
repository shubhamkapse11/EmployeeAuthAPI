using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webApp.Dto;
using webApp.GenericResponse;
using webApp.Iservices;

namespace webApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody]UserDto userDto)
        {
            try
            {
                var result = await _authService.LoginUser(userDto);
                if (result.Item1 == 0) {
                return NotFound(ResponseResult<string>.Failure(null,result.Item2));
                }
                if (result.Item1 == 1) { 
                return BadRequest(ResponseResult<string>.Failure(null,result.Item2));
                }

                if (result.Item1 == 2) { 
                   return Ok(ResponseResult<string>.Success(null,result.Item2));
                }
                // Fallback in case the auth service returns an unexpected code
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected response from authentication service.");

                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody]UserDto userdto)
        {
            try
            {
                var result = await _authService.RegisterUser(userdto);

                if(result.Item1 == 0)
                {
                    return NotFound(ResponseResult<string>.Failure(null, result.Item2));
                }
                return Ok(ResponseResult<string>.Success(null, result.Item2));
            }
            catch (Exception ex)
            {
               return BadRequest(ex.Message);
            }
        }
    }


}
