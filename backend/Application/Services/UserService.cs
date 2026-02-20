using AutoMapper;
using backend.Entities;
using backend.Entities.Models;
using backend.Service.Contracts;
using backend.Entities.DataTransferObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Reflection.Metadata;

namespace backend.Application.Services
{
    internal class UserService : IUserService
    {
        //Properties
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;


        //Contructors
        public UserService(ILoggerManager logger, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
        }


        public async Task<IEnumerable<ApplicationUser>?> GetUsers()
        {
            var userList = _userManager.Users.ToList();

            if (userList != null || !userList.Any())
                _logger.LogWarn($"No users found.");

            return userList;
        }
    }
}