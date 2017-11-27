using AussieLink.Contracts.Enums;
using AussieLink.Contracts.Models;
using AussieLink.Contracts.Responses;
using AussieLink.Contracts.Services;
using AussieLink.Contracts.UnitOfWork;
using AussieLink.Contracts.ViewModels;
using AussieLink.Services.HelperClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.Services.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork UnitOfWork;
        private const int SALTSIZE = 4;

        public AccountService(IUnitOfWork unitOfWork)
        {
            this.UnitOfWork = unitOfWork;
        }

        public SignUpResponse SignUpUser(UserSignupVM model, bool isSocialUser)
        {
            if (HasSameEmail(model.Email))
                return new SignUpResponse(false, ErrorCode.DUPLICATEEMAIL);

            var newUser = GetNewUser(model, isSocialUser);
            Mapper.SignupVMToUserForSignUp(model, newUser);

            UnitOfWork.Users.Add(newUser);
            UnitOfWork.Complete();

            return new SignUpResponse(true, newUser.UserId);
        }

        public bool HasSameEmail(string email)
        {
           return UnitOfWork.Users.SingleOrDefault(m => m.Email == email) == null ? false : true;
        }

        private User GetNewUser(UserSignupVM model, bool isSocialUser)
        {
            var newUser = new User(Guid.NewGuid(), DateTime.Now, false, isSocialUser);
            if (!isSocialUser)
            {
                SetPassword(newUser, model.Password);
            }
            return newUser;
        }

        public SignInResponse SignInUser(UserSigninVM model)
        {
            if (!HasSameEmail(model.Email))
                return new SignInResponse(false, ErrorCode.NOEMAIL);

            var userInDb = UnitOfWork.Users.SingleOrDefault(m => m.Email == model.Email);

            if (userInDb.IsCanceled)
                return new SignInResponse(false, ErrorCode.CANCELEDUSER);

            var salt = userInDb.Salt;
            var inputPassword = PasswordEncryptor.GenerateSHA256Hash(model.Password, salt);

            if (inputPassword != userInDb.Password)
                return new SignInResponse(false, ErrorCode.PASSWORDNOTMATCH);

            return new SignInResponse(true, userInDb.UserId, userInDb.Email);
        }

        public SignInResponse SignInSocialUser(string email)
        {
            var userInDb = UnitOfWork.Users.SingleOrDefault(m => m.Email == email);

            if (userInDb.IsCanceled)
                return new SignInResponse(false, ErrorCode.CANCELEDUSER);

            return new SignInResponse(true, userInDb.UserId, userInDb.Email);
        }

        public int CreateResetPasswordLink(string email)
        {
            var activeResetPasswordLink = GetActieResetPasswordLink(email);
            if (activeResetPasswordLink == null)
            {
                var user = UnitOfWork.Users.SingleOrDefault(u => u.Email == email);
                var resetPasswordLink = new ResetPasswordLink(user.UserId);
                UnitOfWork.ResetPasswordLinks.Add(resetPasswordLink);
                UnitOfWork.Complete();
                return resetPasswordLink.ResetPasswordLinkId;
            }
            return activeResetPasswordLink.ResetPasswordLinkId;
        }

        private ResetPasswordLink GetActieResetPasswordLink(string email)
        {
            var user = UnitOfWork.Users.SingleOrDefault(u => u.Email == email);
            var resetPasswordLink = UnitOfWork.ResetPasswordLinks.SingleOrDefault(r => r.UserId == user.UserId && r.Clicked == false);
            return resetPasswordLink;

        }

        public bool IsResetPasswordLinkValid(string linkCode, string email)
        {
            try
            {
                var decryptedEmail = Encryptor.GetDecryptedString(email);
                var decryptedLinkCode = Encryptor.GetDecryptedString(linkCode);

                int linkCodeInInt;
                var validLinkCode = int.TryParse(decryptedLinkCode, out linkCodeInInt);
                if (!validLinkCode)
                    return false;

                var user = UnitOfWork.Users.SingleOrDefault(u => u.Email == decryptedEmail);
                if (user == null)
                    return false;

                var link = UnitOfWork.ResetPasswordLinks.SingleOrDefault(r => r.ResetPasswordLinkId == linkCodeInInt);
                if (link == null)
                    return false;

                if (user.UserId != link.UserId)
                    return false;

                if (link.Clicked)
                    return false;
                else
                    link.Clicked = true;

                UnitOfWork.Complete();
                return true;
            }
            catch (Exception e)
            {
                //for the case where decrypted strings are changed
                return false;
            }
        }

        public bool ResetPassword(ResetPasswordVM model)
        {
            try
            {
                var decryptedEmail = Encryptor.GetDecryptedString(model.Email);

                var user = UnitOfWork.Users.SingleOrDefault(u => u.Email == decryptedEmail);
                if (user == null)
                    return false;

                SetPassword(user, model.Password);
                UnitOfWork.Complete();
                return true;
            }
            catch (Exception e)
            {
                //for exception from decrytion
                return false;
            }
        }

        private void SetPassword(User user, string inputPassword)
        {
            var salt = PasswordEncryptor.CreateSalt(SALTSIZE);
            var passowrd = PasswordEncryptor.GenerateSHA256Hash(inputPassword, salt);
            user.Salt = salt;
            user.Password = passowrd;
        }
    }
}
