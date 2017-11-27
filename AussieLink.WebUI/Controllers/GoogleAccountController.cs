using AussieLink.Contracts.Enums;
using AussieLink.Contracts.ExtensionMethods;
using AussieLink.Contracts.Services;
using AussieLink.Contracts.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AussieLink.WebUI.Controllers
{
    public class GoogleAccountController : BaseAccountController
    {
        private readonly string GoogleApiId = "1062337681869-apnsoovuu4q4q556tl22hsueetch7k0j.apps.googleusercontent.com";
        private readonly string GoogleSecret = "KaahZ7bK9li_PRAFeiWGAHat";

        public GoogleAccountController(IAccountService accountService) : base(accountService)
        {}

        [AllowAnonymous]
        public ActionResult SignInWithGoogle(string returnUrl)
        {
            SetReturnUrlToTemp(returnUrl);
            return Redirect(GetGoogleAuthUrl(SocialAccount.GOOGLESIGNINCALLBACK));
        }

        [AllowAnonymous]
        public async Task<ActionResult> SignInGoogleCallBack(string code)
        {
            var returnUrl = base.GetTempData(SocialAccount.GOOGLERETURNURL.ToDescription());
            return await GoogleCallback(code, SocialAccount.GOOGLESIGNINCALLBACK.ToDescription(), "SignIn", returnUrl);
        }

        [AllowAnonymous]
        public ActionResult SignUpWithGoogle(string returnUrl)
        {
            SetReturnUrlToTemp(returnUrl);
            return Redirect(GetGoogleAuthUrl(SocialAccount.GOOGLESIGNUPCALLBACK));
        }

        [AllowAnonymous]
        public async Task<ActionResult> SignUpGoogleCallBack(string code)
        {
            var returnUrl = base.GetTempData(SocialAccount.GOOGLERETURNURL.ToDescription());
            return await GoogleCallback(code, SocialAccount.GOOGLESIGNUPCALLBACK.ToDescription(), "SignUp", returnUrl);
        }

        [AllowAnonymous]
        public async Task<ActionResult> GoogleCallback(string code, string callBackAction, string errorReturnToAction, object returnUrl)
        {
            try
            {
                HttpWebRequest webRequest = GetHttpWebRequest(Request, code, callBackAction);
                GoogleAccessToken accessToken = GetAccessToken(webRequest);

                if (!IsAccessTokenValid(accessToken))
                    throw new Exception("Invalid access token");

                UserSignupVM model = await GetGoogleUserData(accessToken.access_token);

                return base.SignUpWithSocialAccount(model, returnUrl);
            }
            catch (Exception)
            {
                TempData[SocialAccount.SOCIALSIGNUPERROR.ToDescription()] = ErrorCode.SOCIALSIGNUPERROR.ToDescription();
                return RedirectToAction(errorReturnToAction, "Account", new { returnUrl = returnUrl });
            }
        }

        private HttpWebRequest GetHttpWebRequest(HttpRequestBase request, string code, string callBackAction)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create("https://accounts.google.com/o/oauth2/token");
            webRequest.Method = "POST";
            var parameters = "code=" + code
                + "&client_id=" + GoogleApiId
                + "&client_secret=" + GoogleSecret
                + "&redirect_uri=" + GetReturnUrl(request, callBackAction)
                + "&grant_type=authorization_code";

            byte[] byteArray = Encoding.UTF8.GetBytes(parameters);
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.ContentLength = byteArray.Length;
            Stream postStream = webRequest.GetRequestStream();
            postStream.Write(byteArray, 0, byteArray.Length);
            postStream.Close();

            return webRequest;
        }

        private async Task<UserSignupVM> GetGoogleUserData(string accessToken)
        {
            HttpClient httpClient = new HttpClient();
            var url = "https://www.googleapis.com/oauth2/v1/userinfo?access_token=" + accessToken;
            httpClient.CancelPendingRequests();
            HttpResponseMessage output = await httpClient.GetAsync(url);

            if (output.IsSuccessStatusCode)
            {
                string outputData = await output.Content.ReadAsStringAsync();
                UserSignupVM vm = JsonConvert.DeserializeObject<UserSignupVM>(outputData);
                return vm;
            }
            return null;
        }

        private bool IsAccessTokenValid(GoogleAccessToken accessToken)
        {
            if (accessToken == null || string.IsNullOrEmpty(accessToken.access_token))
                return false;

            return true;
        }

        private GoogleAccessToken GetAccessToken(HttpWebRequest webRequest)
        {
            WebResponse response = webRequest.GetResponse();
            Stream postStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(postStream);
            string responseFromServer = reader.ReadToEnd();
            postStream.Close();

            GoogleAccessToken accessToken = JsonConvert.DeserializeObject<GoogleAccessToken>(responseFromServer);
            return accessToken;
        }

        private string GetGoogleAuthUrl(SocialAccount callBackAction)
        {
            var googleAuthUrl = "https://accounts.google.com/o/oauth2/auth?response_type=code&redirect_uri="
                + GetReturnUrl(Request, callBackAction.ToDescription())
                + "&scope=https://www.googleapis.com/auth/userinfo.email%20https://www.googleapis.com/auth/userinfo.profile&client_id="
                + GoogleApiId;

            return googleAuthUrl;
        }

        private string GetReturnUrl(HttpRequestBase request, string callBackAction)
        {
            var baseUrl = request.Url.GetLeftPart(UriPartial.Authority);
            var returnUrl = baseUrl + Url.Action(callBackAction);
            return returnUrl;
        }

        private void SetReturnUrlToTemp(string returnUrl)
        {
            //tempdata only for facebook login return url
            if (!String.IsNullOrEmpty(returnUrl))
                TempData[SocialAccount.GOOGLERETURNURL.ToDescription()] = returnUrl;
        }

    }
}
