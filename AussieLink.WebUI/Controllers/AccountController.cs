using AussieLink.Contracts.Enums;
using AussieLink.Contracts.Services;
using AussieLink.Contracts.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Facebook;
using Newtonsoft.Json;
using AussieLink.Services.HelperClasses;
using AussieLink.Contracts.ExtensionMethods;
using AussieLink.Contracts.ViewModels.ManageAdViews;

namespace AussieLink.WebUI.Controllers
{
    public class AccountController : BaseAccountController
    {
        IEmailService EmailService;

        public AccountController(IAccountService accountService, IEmailService emailService) : base(accountService)
        {
            EmailService = emailService;
        }

        [AllowAnonymous]
        public ActionResult SignUp()
        {
            var socialSignupErrorMsg = base.GetTempData(SocialAccount.SOCIALSIGNUPERROR.ToDescription());
            if (socialSignupErrorMsg != null)
                ModelState.AddModelError("", socialSignupErrorMsg.ToString());

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult SignUp(UserSignupVM model, string returnUrl)
        {
            if (!ModelState.IsValid)
                return View(model);

            var response = AccountService.SignUpUser(model, false);

            if (response.Success)
            {
                base.SignUserIn(response.UserId, model.Email, true);
                return ReturnTo(returnUrl);
            }
            else
            {
                ModelState.AddModelError("", response.ErrorCode.ToDescription());
                return View(model);
            }
        }

        [AllowAnonymous]
        public ActionResult SignIn()
        {
            var socialSignupErrorMsg = base.GetTempData(SocialAccount.SOCIALSIGNUPERROR.ToDescription());
            if (socialSignupErrorMsg != null)
                ModelState.AddModelError("", socialSignupErrorMsg.ToString());

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult SignIn(UserSigninVM model, string returnUrl)
        {
            if (!ModelState.IsValid)
                return View(model);

            var response = AccountService.SignInUser(model);

            if (response.Success)
            {
                base.SignUserIn(response.UserId, response.UserEmail, model.RememberMe);
                return ReturnTo(returnUrl);
            }

            ModelState.AddModelError("", response.ErrorCode.ToDescription());
            return View(model);
        }

        private ActionResult ReturnTo(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPasswordVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (!AccountService.HasSameEmail(model.Email))
            {
                ModelState.AddModelError("", ErrorCode.FORGOTPWDNOEMAIL.ToDescription());
                return View(model);
            }

            SendResetPasswordLink(model.Email);
            return RedirectToAction("ForgotPasswordConfirm");
        }

        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirm()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult ResetPassword(string linkCode, string email)
        {
            var valid = AccountService.IsResetPasswordLinkValid(linkCode, email);
            if (!valid)
                return View("Error");

            var vModel = new ResetPasswordVM() { Email = email };
            return View(vModel);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (!AccountService.ResetPassword(model))
                return View("Error");

            return RedirectToAction("SignIn", "Account");
        }

        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            Session[Account.USERID.ToDescription()] = null;
            return RedirectToAction("SignIn", "Account");
        }

        private string GetUrlFor(string actionName)
        {
            var baseUrl = Request.Url.GetLeftPart(UriPartial.Authority);
            var returnUrl = baseUrl + Url.Action(actionName);
            return returnUrl;
        }

        private void SendResetPasswordLink(string email)
        {
            int linkCode = AccountService.CreateResetPasswordLink(email);
            EmailService.SendResetPasswordLink(email, linkCode, GetUrlFor("ResetPassword"));
        }
    }
}