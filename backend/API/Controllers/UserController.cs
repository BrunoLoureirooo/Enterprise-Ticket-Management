using backend.Service.Contracts;
using Microsoft.AspNetCore.Mvc;
using backend.Entities.Models;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IServiceManager _service;
        public UserController(IServiceManager service) => _service = service;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetUsers()
        {
            var users = await _service.UserService.GetUsers();
            
            if(users == null || !users.Any())
                return NotFound();

            return Ok(users);
        }

    }
}