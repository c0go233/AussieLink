using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Facebook;
using Newtonsoft.Json;
using AussieLink.Contracts.Services;
using AussieLink.Contracts.ViewModels;
using AussieLink.Contracts.Enums;
using AussieLink.Contracts.ExtensionMethods;

namespace AussieLink.WebUI.Controllers
{
    public class FacebookAccountController : BaseAccountController
    {
        private readonly string facebookApiId = "1084498531695886";
        private readonly string facebookApiSecret = "3a66c1ee92ecf4b2f2fa5b08ed84f549";

        public FacebookAccountController(IAccountService accountService) : base(accountService) {}

        [AllowAnonymous]
        public ActionResult SignUpWithFacebook(string returnUrl)
        {
            var loginUrl = GetFacebookLoginUri(SocialAccount.FACEBOOKSIGNUPCALLBACK);
            SetReturnUrlToTemp(returnUrl);
            return Redirect(loginUrl.AbsoluteUri);
        }

        [AllowAnonymous]
        public ActionResult SignUpFacebookCallBack(string code)
        {
            var returnUrl = base.GetTempData(SocialAccount.FACEBOOKRETURNURL.ToDescription());
            return FacebookCallback(code, SocialAccount.FACEBOOKSIGNUPCALLBACK.ToDescription(), "SignUp", returnUrl);
        }

        [AllowAnonymous]
        public ActionResult SignInWithFacebook(string returnUrl)
        {
            var loginUrl = GetFacebookLoginUri(SocialAccount.FACEBOOKSIGNINCALLBACK);
            SetReturnUrlToTemp(returnUrl);
            return Redirect(loginUrl.AbsoluteUri);
        }

        [AllowAnonymous]
        public ActionResult SignInFacebookCallBack(string code)
        {
            var returnUrl = base.GetTempData(SocialAccount.FACEBOOKRETURNURL.ToDescription());
            return FacebookCallback(code, SocialAccount.FACEBOOKSIGNINCALLBACK.ToDescription() ,"SignIn", returnUrl);
        }

        private ActionResult FacebookCallback(string code, string callBackAction, string errorReturnToAction, object returnUrl)
        {
            try
            {
                var fb = GetTokenedFbClient(Request, code, callBackAction);
                var userSignupVM = GetSignupVMFromFbGet(fb);
                return base.SignUpWithSocialAccount(userSignupVM, returnUrl);
            }
            catch (Exception)
            {
                //since we don't know what error will be thrown from fabebook side
                TempData[SocialAccount.SOCIALSIGNUPERROR.ToDescription()] = ErrorCode.SOCIALSIGNUPERROR.ToDescription();
                var returnUrlInString = returnUrl == null ? "" : returnUrl.ToString();
                return RedirectToAction(errorReturnToAction, "Account", new { returnUrl = returnUrl });
            }
        }

        private FacebookClient GetTokenedFbClient(HttpRequestBase request, string code, string callBackAction)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = facebookApiId,
                client_secret = facebookApiSecret,
                redirect_uri = GetReturnUrl(Request, callBackAction),
                code = code
            });

            if (result.access_token == null)
                throw new Exception("null token");

            fb.AccessToken = result.access_token;
            return fb;
        }

        private void SetReturnUrlToTemp(string returnUrl)
        {
            //tempdata only for facebook login return url
            if (!String.IsNullOrEmpty(returnUrl))
                TempData[SocialAccount.FACEBOOKRETURNURL.ToDescription()] = returnUrl;
        }

        private UserSignupVM GetSignupVMFromFbGet(FacebookClient fb)
        {
            dynamic me = fb.Get("me?fields=first_name,last_name,email");
            var userSignupVM = new UserSignupVM
            {
                Email = me.email,
                Name = me.first_name + " " + me.last_name
            };
            return userSignupVM;
        }

        private Uri GetFacebookLoginUri(SocialAccount callBackAction)
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = facebookApiId,
                client_secret = facebookApiSecret,
                redirect_uri = GetReturnUrl(Request, callBackAction.ToDescription()),
                reponse_type = "code",
                scope = "email"
            });
            return loginUrl;
        }

        private string GetReturnUrl(HttpRequestBase request, string callBackAction)
        {
            var baseUrl = request.Url.GetLeftPart(UriPartial.Authority);
            var returnUrl = baseUrl + Url.Action(callBackAction);
            return returnUrl;
        }
    }
}