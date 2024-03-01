using AutoMapper;
using JetStudyProject.Core.Entities;
using JetStudyProject.Infrastracture.DataAccess;
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
        private readonly IUnitOfWork _unitOfWork;

        public UserService(UserManager<User> userManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
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

        public async Task BecomeInstructor(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (!await _userManager.IsInRoleAsync(user, "Instructor")) 
            {
                await _userManager.AddToRoleAsync(user, "Instructor");
            }
            else
                throw new HttpException("You are already registered as instructor", HttpStatusCode.BadRequest);
        }
    }
}
