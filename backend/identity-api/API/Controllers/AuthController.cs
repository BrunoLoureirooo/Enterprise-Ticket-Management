using backend.Application.Services.Contracts;
using backend.Entities.DataTransferObjects.Users;
using backend.API.ActionFilters;
using Microsoft.AspNetCore.Mvc;

namespace backend.API.Controllers
{
    public class AuthController : BaseApiController
    {
        private readonly IServiceManager _service;
        public AuthController(IServiceManager service) => _service = service;

        [HttpPost("login")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDto user)
        {
            var userEntity = await _service.AuthenticationService.ValidateUser(user);
            if (userEntity == null)
                return Unauthorized();

            var tokenDto = await _service.AuthenticationService
                .CreateToken(userEntity, populateExp: true);

            return Ok(tokenDto);    
        }


        [HttpPost("register")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Register([FromBody] UserForRegistrationDto user)
        {
            var result = await _service.AuthenticationService.RegisterUser(user);
            if (!result.Succeeded)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("test-sentry")]
        public async Task<IActionResult> TestSentry()
        {
            SentrySdk.CaptureMessage("Hello Sentry");
            return Ok();
        }

        [HttpPost("authorize")]
        public async Task<IActionResult> Authorize([FromBody] AuthorizationRequestDto request)
        {
            var result = await _service.AuthenticationService.Authorize(request);
            if (!result)
                return StatusCode(403); // Forbidden

            return Ok();
        }

    }
}