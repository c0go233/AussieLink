using System;

namespace AussieLink.Contracts.Services
{
    public interface IEmailService
    {
        void SendResetPasswordLink(string userEmail, int linkCode, string returnUrl);
    }
}