using AussieLink.Contracts.Enums;
using AussieLink.Contracts.ExtensionMethods;
using AussieLink.Contracts.Services;
using AussieLink.Contracts.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace AussieLink.WebUI.Controllers
{
    public class BaseAccountController : Controller
    {
        protected readonly IAccountService AccountService;

        public BaseAccountController(IAccountService accountService)
        {
            this.AccountService = accountService;
        }

        protected ActionResult SignUpWithSocialAccount(UserSignupVM model, object returnUrl)
        {
            if (model == null || model.Email == null)
                throw new Exception("Invalid data from social account");

            var response = AccountService.SignUpUser(model, true);

            //duplicate error is only error thrown from signup user
            if (!response.Success && response.ErrorCode == ErrorCode.DUPLICATEEMAIL)
                SignInWithSocialAccount(model.Email);
            else
                SignUserIn(response.UserId, model.Email, true);

            return ReturnToEither(returnUrl);
        }

        private void SignInWithSocialAccount(string email)
        {
            var signinResponse = AccountService.SignInSocialUser(email);

            if (!signinResponse.Success)
                throw new Exception("user is canceled");

            SignUserIn(signinResponse.UserId, signinResponse.UserEmail, true);
        }

        private ActionResult ReturnToEither(object returnUrl)
        {
            if (returnUrl == null)
                return RedirectToAction("Index", "Home");

            if (Url.IsLocalUrl(returnUrl.ToString()))
                return Redirect(returnUrl.ToString());

            return RedirectToAction("Index", "Home");
        }

        protected void SignUserIn(Guid userId,  string userEmail, bool remember)
        {
            FormsAuthentication.SetAuthCookie(userEmail, remember);
            Session[Account.USERID.ToDescription()] = userId.ToString();
        }

        protected object GetTempData(string key)
        {
            var data = TempData[key];
            TempData.Remove(key);
            return data;
        }
    }
}