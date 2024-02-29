using JetStudyProject.Core.Entities;
using JetStudyProject.Infrastracture.DTOs.AuthDTOs;
using JetStudyProject.Infrastracture.DTOs.UserDTOs;
using JetStudyProject.Infrastracture.Exceptions;
using JetStudyProject.Infrastracture.Interfaces;
using JetStudyProject.Infrastracture.Resources;
using JetStudyProject.Infrastracture.Utilities;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace JetStudyProject.Infrastracture.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IUrlHelper _helper;
        public AuthService(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, IUrlHelper helper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _helper = helper;
        }

        public async Task Register(UserRegisterDto userRegisterDto)
        {
            var user = new User()
            {
                Email = userRegisterDto.Email,
                UserName = userRegisterDto.UserName
            };

            var result = await _userManager.CreateAsync(user, userRegisterDto.Password);

            if (!result.Succeeded)
            {
                string errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new HttpException(errors, HttpStatusCode.BadRequest);
            }
            var userInDb = await _userManager.FindByEmailAsync(userRegisterDto.Email);

            var role = _roleManager.Roles.FirstOrDefault(x => x.NormalizedName == "STUDENT");
            if (role == null)
            {
                await _roleManager.CreateAsync(new IdentityRole("Student"));
            }
            await _userManager.AddToRoleAsync(userInDb, "Student");
            var confirmationCode = await _userManager.GenerateEmailConfirmationTokenAsync(userInDb);
            var callbackUrl = _helper.Action(
                        "ConfirmEmail",
                        "Auth",
                        new { userId = user.Id, code = confirmationCode },
                        protocol: _helper.ActionContext.HttpContext.Request.Scheme);
            EmailService emailService = new EmailService(_configuration);
            await emailService.SendEmailAsync(userRegisterDto.Email, "Confirm your account",
                $"Перейдіть за посиланням, щоб підтвердити аккаунт: <a href='{callbackUrl}'>link</a>");
        }

        public async Task<LoginAnswerDto> Login(UserLoginDto userLoginDto)
        {
            var user = await _userManager.FindByEmailAsync(userLoginDto.Email);

            if (user != null)
            {
                if (!await _userManager.IsEmailConfirmedAsync(user))
                {
                    throw new HttpException("Ви не підтвердили свою електронну адресу", HttpStatusCode.BadRequest);
                }
            }

            if (user == null || !await _userManager.CheckPasswordAsync(user, userLoginDto.Password))
            {
                throw new HttpException(ErrorMessages.InvalidCredentials, HttpStatusCode.BadRequest);
            }

            await _signInManager.SignInAsync(user, userLoginDto.RememberMe);

            return new LoginAnswerDto { Key = await GenerateTokenAsync(user) };

        }

        private async Task<string> GenerateTokenAsync(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var roles = await _userManager.GetRolesAsync(user);
            string role;
            if (roles.Count > 0)
            {
                role = roles[0];
            }
            else
            {
                role = "null";
            }
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Role, role),
                new Claim("Name", user.UserName),
                new Claim("Email", user.Email)
            };

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(Int32.Parse(_configuration.GetSection("Jwt:Expires").Value)),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                throw new HttpException("Bad Request", HttpStatusCode.BadRequest);
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new HttpException("There is no such user", HttpStatusCode.BadRequest);
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (!result.Succeeded)
                throw new HttpException("The error has occured", HttpStatusCode.BadRequest);
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
