using backend.Application.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using backend.Entities.Models;

namespace backend.API.Controllers
{
    public class UserController : BaseApiController
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