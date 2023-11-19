using AutoMapper;
using JetStudyProject.Core.Entities;
using JetStudyProject.Infrastracture.Exceptions;
using JetStudyProject.Infrastracture.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JetStudyProject.Infrastracture.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;

        public UserService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<User> GetUser(ClaimsPrincipal claimsPrincipal)
        {
            var user = await _userManager.GetUserAsync(claimsPrincipal);

            if (user == null)
            {
                throw new HttpException("Invalid user id", HttpStatusCode.BadRequest);
            }

            return user;
        }

        public async Task<string> GetUserId(ClaimsPrincipal claimsPrincipal)
        {
            var user = await GetUser(claimsPrincipal);
            return user.Id;
        }
    }
}
