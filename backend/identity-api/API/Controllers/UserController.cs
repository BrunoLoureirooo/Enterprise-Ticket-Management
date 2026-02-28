using backend.Application.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using backend.Entities.Enums;
using backend.Entities.DataTransferObjects.Users;
using AutoMapper;

namespace backend.API.Controllers
{
    [Authorize]
    public class UserController : BaseApiController
    {
        private readonly IServiceManager _service;
        private readonly IMapper _mapper;
        public UserController(IServiceManager service, IMapper mapper) 
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = nameof(Roles.Administrador))]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await _service.UserService.GetUsers();
            
            if(users == null || !users.Any())
                return NotFound();

            var results = _mapper.Map<IEnumerable<UserDto>>(users);

            return Ok(results);
        }

    }
}