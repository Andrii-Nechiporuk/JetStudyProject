using JetStudyProject.Core.Entities;
using JetStudyProject.Infrastracture.Exceptions;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JetStudyProject.Infrastracture.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUser(ClaimsPrincipal claimsPrincipal);
        Task<string> GetUserId(ClaimsPrincipal user);
        Task BecomeInstructor(string userId);
        Task ChangeUserName(ClaimsPrincipal claimsPrincipal, string name);
        Task ChangeName(ClaimsPrincipal claimsPrincipal, string name);
        Task ChangeSurname(ClaimsPrincipal claimsPrincipal, string surname);
        Task ChangePassword(ClaimsPrincipal claimsPrincipal, string currentPassword, string newPassword);
        Task ChangeEmail(ClaimsPrincipal claimsPrincipal, string currentPassword, string newEmail);
    }
}
