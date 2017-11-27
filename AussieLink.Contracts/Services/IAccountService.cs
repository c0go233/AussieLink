using AussieLink.Contracts.Responses;
using AussieLink.Contracts.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.Contracts.Services
{
    public interface IAccountService
    {
        SignUpResponse SignUpUser(UserSignupVM model, bool isSocialUser);
        bool HasSameEmail(string email);
        SignInResponse SignInUser(UserSigninVM model);
        SignInResponse SignInSocialUser(string email);
        int CreateResetPasswordLink(string email);
        bool ResetPassword(ResetPasswordVM model);
        bool IsResetPasswordLinkValid(string linkCode, string email);
    }
}
