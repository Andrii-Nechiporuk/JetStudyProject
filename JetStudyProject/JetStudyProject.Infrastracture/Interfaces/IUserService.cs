using JetStudyProject.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
