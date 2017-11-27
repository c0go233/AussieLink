using AussieLink.Contracts.Enums;
using AussieLink.Contracts.ExtensionMethods;
using AussieLink.Contracts.Services;
using AussieLink.Contracts.ViewModels.ManageAdViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace AussieLink.WebUI.Controllers
{
    //[Authorize]
    public class ManageController : Controller
    {
        private readonly IManageService ManageService;
        private readonly string PageIndexQueryKey = "PageIndex";
        private readonly string PostTypeQueryKey = "PostType";

        public ManageController(IManageService manageService)
        {
            this.ManageService = manageService;
        }

        public ActionResult MyAds(int pageIndex = 1, string postType = null)
        {
            var response = ManageService.GetManageAdVM(GetEmailFromAuthCookie(), new ManageAdVM(), pageIndex);
            if (!response.Success)
                return View("Error");

            response.ManageAdVM.Pager.PageUrl = GetUrlForPager(postType);
            return View(response.ManageAdVM);
        }

        public ActionResult MyProfile()
        {
            //if you allow users to change email, you should check 
            //if it is social account or not because social account does not have
            //password.......

            var manageProfileVM = ManageService.GetManageProfileVM(GetEmailFromAuthCookie(), new ManageProfileVM());
            if (manageProfileVM == null)
                return View("Error");

            return View(manageProfileVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MyProfile(ManageProfileVM vm)
        {
            vm.CurrentMyAccountMenu = MyAccountMenu.PROFILE.ToDescription();
            if (!ModelState.IsValid)
                return View(vm);

            if (!ManageService.UpdateProfile(vm, GetEmailFromAuthCookie()))
                return View("Error");

            return View(vm);
        }

        private string GetEmailFromAuthCookie()
        {
            //cookie is always there because this controller is [authorize]
            HttpCookie authoCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authoCookie.Value);
            return ticket.Name;
        }

        private string GetUrlForPager(string postType)
        {
            var localUrl = HttpContext.Request.Url.LocalPath + "?";
            if (postType != null)
                localUrl += PostTypeQueryKey + "=" + postType + "&" + PageIndexQueryKey + "=";
            else
                localUrl += PageIndexQueryKey + "=";

            return localUrl;
        }
    }
}





