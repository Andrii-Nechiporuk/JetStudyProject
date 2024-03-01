using AutoMapper;
using JetStudyProject.Core.Entities;
using JetStudyProject.Infrastracture.DataAccess;
using JetStudyProject.Infrastracture.Exceptions;
using JetStudyProject.Infrastracture.Interfaces;
using Microsoft.AspNetCore.Hosting;
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
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;

        public UserService(UserManager<User> userManager, IWebHostEnvironment webHostEnvironment, IMapper mapper)
        {
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
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

        public async Task ChangeUserName(ClaimsPrincipal claimsPrincipal, string name)
        {
            var user = await GetUser(claimsPrincipal);
            var result = await _userManager.SetUserNameAsync(user, name);
            HandleResult(result);
        }

        public async Task ChangeName(ClaimsPrincipal claimsPrincipal, string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new HttpException("The name field is required.", HttpStatusCode.BadRequest);
            }

            var user = await GetUser(claimsPrincipal);
            user.Name = name;
            var result = await _userManager.UpdateAsync(user);
            HandleResult(result);
        }

        public async Task ChangeSurname(ClaimsPrincipal claimsPrincipal, string surname)
        {
            if (string.IsNullOrEmpty(surname))
            {
                throw new HttpException("The name field is required.", HttpStatusCode.BadRequest);
            }

            var user = await GetUser(claimsPrincipal);
            user.Surname = surname;
            var result = await _userManager.UpdateAsync(user);
            HandleResult(result);
        }

        public async Task ChangePassword(ClaimsPrincipal claimsPrincipal, string currentPassword, string newPassword)
        {
            var user = await GetUser(claimsPrincipal);

            if (!await _userManager.CheckPasswordAsync(user, currentPassword))
            {
                throw new HttpException("Invalid password.", HttpStatusCode.BadRequest);
            }

            if (currentPassword == newPassword)
            {
                throw new HttpException("New password cannot be the same as current password", HttpStatusCode.BadRequest);
            }

            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            HandleResult(result);
        }

        public async Task ChangeEmail(ClaimsPrincipal claimsPrincipal, string currentPassword, string newEmail)
        {
            var user = await GetUser(claimsPrincipal);

            if (!await _userManager.CheckPasswordAsync(user, currentPassword))
            {
                throw new HttpException("Invalid password", HttpStatusCode.BadRequest);
            }

            var token = await _userManager.GenerateChangeEmailTokenAsync(user, newEmail);
            var result = await _userManager.ChangeEmailAsync(user, newEmail, token);
            HandleResult(result);
        }

        private void HandleResult(IdentityResult result)
        {
            if (!result.Succeeded)
            {
                string errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new HttpException(errors, HttpStatusCode.BadRequest);
            }
        }
    }
}
