using AussieLink.Contracts.Enums;
using AussieLink.Contracts.ExtensionMethods;
using AussieLink.Contracts.Services;
using AussieLink.Contracts.ViewModels.AdViewModels;
using AussieLink.Contracts.ViewModels.PostViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace AussieLink.WebUI.Controllers
{
    public class AdController : Controller
    {
        private readonly IJobAdService JobAdService;

        public AdController(IJobAdService jobAdService)
        {
            this.JobAdService = jobAdService;
        }

        [AllowAnonymous]
        public ActionResult JobAd(string _place = null, string _contractType = null, 
            string _salaryType = null, decimal? _salary = null, byte? _day = null, 
            string _jobType = null, string _keyword = null, int _pageIndex = 1, string _currentPostId = null)
        {
            JobAdVM jobAdVM = new JobAdVM(_place, _contractType, _salaryType, _salary, _day, _jobType, _keyword, _pageIndex, _currentPostId);
            JobAdService.PopulateCategories(jobAdVM.JobAdFilterVM.JobAdCategories);
            return View(jobAdVM);
        }

        [AllowAnonymous]
        public ActionResult GetJobAd(JobAdFilterVM filterVM, int pageIndex, string selectedPostId)
        {
            var getJobAdVM = JobAdService.GetAds(filterVM, pageIndex ,new GetJobAdVM(), selectedPostId);
            return Json(getJobAdVM, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public ActionResult JobAdDetail(string currentPostId, string returnUrl, bool needBackToList = true)
        {
            var jobAdDetailVM = new JobAdDetailVM(returnUrl, needBackToList);

            //temporal fix 
            jobAdDetailVM.CurrentPostId = currentPostId;

            if (!JobAdService.SetJobAdDetailVM(currentPostId, GetUserEmailFromAuthCookie(), jobAdDetailVM))
            {
                if (Request.IsAjaxRequest())
                {
                    Response.StatusCode = 400;
                    return PartialView("~/Views/Shared/Ad/_AdDetailPageError.cshtml");
                }
                return View("JobAdDetailError");

            }
            else if (Request.IsAjaxRequest())
                return PartialView("~/Views/Shared/Ad/_JobAdDetailAsPartial.cshtml", jobAdDetailVM);
            else
                return View(jobAdDetailVM);
        }

        //it is called from manage ad so it does not need back to list btn
        public ActionResult AdDetail(string currentPostId, string returnUrl, string postType)
        {
            if (postType == PostType.JOB.ToDescription())
                return RedirectToAction("JobAdDetail", new { currentPostId = currentPostId, returnUrl = returnUrl, needBackToList = false });

            //here rent and secondhand

            return View();
        }

        private string GetUserEmailFromAuthCookie()
        {
            HttpCookie authoCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authoCookie == null)
                return null;
             
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authoCookie.Value);
            return ticket.Name;
        }
    }
}