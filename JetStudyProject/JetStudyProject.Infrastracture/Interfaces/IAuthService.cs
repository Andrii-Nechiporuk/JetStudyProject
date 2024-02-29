using JetStudyProject.Infrastracture.DTOs.AuthDTOs;
using JetStudyProject.Infrastracture.DTOs.UserDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetStudyProject.Infrastracture.Interfaces
{
    public interface IAuthService
    {
        Task Register(UserRegisterDto userRegisterDto);
        Task<LoginAnswerDto> Login(UserLoginDto userLoginDto);
        Task<LoginAnswerDto> LoginWithGoogle(string credential);
        Task ConfirmEmail(string userId, string code);
        Task Logout();
    }
}
