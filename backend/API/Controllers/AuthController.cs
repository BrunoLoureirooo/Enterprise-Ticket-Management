using backend.Service.Contracts;
using backend.Entities.DataTransferObjects;
using backend.API.ActionFilters;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace backend.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
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

    }
}