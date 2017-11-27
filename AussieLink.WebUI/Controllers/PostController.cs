using AussieLink.Contracts.Enums;
using AussieLink.Contracts.ExtensionMethods;
using AussieLink.Contracts.Services;
using AussieLink.Contracts.UnitOfWork;
using AussieLink.Contracts.ViewModels.PostViewModels;
using AussieLink.Contracts.ViewModels.PostViewModels.SharePostViewModels;
using AussieLink.Services.HelperClasses;
using AussieLink.WebUI.CustomAttributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace AussieLink.WebUI.Controllers
{
    public class PostController : Controller
    {
        private readonly IJobPostService JobPostService;
        private readonly ISharePostService SharePostService;

        public PostController(IJobPostService jobPostService, ISharePostService sharePostService)
        {
            this.JobPostService = jobPostService;
            this.SharePostService = sharePostService;
        }

        public ActionResult Post()
        {
            return View();
        }

        public ActionResult JobPost()
        {
            var model = new JobPostVM();
            JobPostService.PopulateCategories(model);
            return View(model);
        }

        public ActionResult SharePost()
        {
            var model = SharePostService.PopulateCategories(new SharePostVM());
            return View(model);
        }

        public ActionResult EditSharePost(string currentPostId, string returnUrl = null)
        {
            var sharePostVM = new SharePostVM();
            var valid = SharePostService.SetSharePostVMForEdit(sharePostVM, GetEmailFromAuthCookie());
            if (!valid)
                return View("Error");

            return View("SharePost", sharePostVM);
        }

        [HttpPost]
        [ValidateAntiForgeryTokenOnAllPosts]
        public ActionResult SharePost(SharePostVM vm, IEnumerable<HttpPostedFileBase> unsavedPictures, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = 400;
                return Json(GetErrorList(ModelState));
            }
            var response = SharePostService.SaveSharePost(vm, unsavedPictures, GetEmailFromAuthCookie());
            if (!response.Success)
            {
                var errorList = new List<ErrorVM> { new ErrorVM { Key = "general-error", ErrorMsg = response.ErrorCode.ToDescription() } };
                Response.StatusCode = 400;
                return Json(errorList);
            }
            return Json(new { placeId = 1 });
        }

        private IEnumerable<ErrorVM> GetErrorList(ModelStateDictionary modelState)
        {
            var errorList = new List<ErrorVM>();
            var values = modelState.Values.ToList();
            var keys = modelState.Keys.ToList();
            for (int i = 0; i < values.Count(); i++)
            {
                var error = values[i].Errors.FirstOrDefault();
                if (error != null)
                {
                    errorList.Add(new ErrorVM(keys[i], error.ErrorMessage));
                }
            }
            return errorList;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveJobPost(JobPostVM model, string returnUrl = null)
        {
            return SavePost(ModelState, JobPostService, model, GetEmailFromAuthCookie(), "JobPost", "JobAd", returnUrl);
        }

        private ActionResult SavePost(ModelStateDictionary model, IBasePostService service, 
            BasePostVM viewModel, string userEmail, string viewName, string redirectAction, string returnUrl)
        {
            if (!model.IsValid)
            {
                service.PopulateCategories(viewModel);
                return View(viewName, viewModel);
            }

            var postId = service.SavePost(viewModel, userEmail);
            if (postId == null)
                return View("Error");

            if (returnUrl != null)
                return Redirect(returnUrl);
            else
            {
                var encryptedPostId = Encryptor.GetEncryptedString(postId.ToString());
                return RedirectToAction(redirectAction, "Ad", new { _currentPostId = encryptedPostId });
            }
        }

        public ActionResult RepostManagePost(string currentPostId, string postType, string returnUrl)
        {
            if (postType == PostType.JOB.ToDescription())
                return RepostJobPost(currentPostId, returnUrl);

            //hrerer rent and secondhand
            return null;
        }

        public ActionResult RepostJobPost(string currentPostId, string returnUrl)
        {
            return RepostPost(JobPostService, currentPostId, returnUrl, GetEmailFromAuthCookie());
        }

        private ActionResult RepostPost(IBasePostService service, string currentPostId, string returnUrl, string userEmail)
        {
            if (!service.RepostPost(currentPostId, userEmail))
                return View("Error");

            if (!Url.IsLocalUrl(returnUrl))
                return View("Error");

            return ReturnTo(returnUrl);
        }

        public ActionResult CompleteManagePost(string currentPostId, string postType, string returnUrl)
        {
            if (postType == PostType.JOB.ToDescription())
                return CompleteJobPost(currentPostId, returnUrl);

            //herere rent and secondhand
            return null;
        }

        public ActionResult EditManagePost(string currentPostId, string postType, string returnUrl)
        {
            if (postType == PostType.JOB.ToDescription())
                return EditJobPost(currentPostId, returnUrl);

            //hrere rent and seconhand 
            return null;
        }

        public ActionResult DeleteManagePost(string currentPostId, string postType, string returnUrl)
        {
            if (postType == PostType.JOB.ToDescription())
                return DeleteJobPost(currentPostId, returnUrl);

            //hrere rent and secondhand
            return null;
        }

        public ActionResult EditJobPost(string currentPostId, string returnUrl = null)
        {
            var jobPostVM = new JobPostVM();
            return EditPost(JobPostService, currentPostId, GetEmailFromAuthCookie(), "JobPost", jobPostVM);
        }

        private ActionResult EditPost(IBasePostService service, string postId, string userEmail, string viewName, BasePostVM viewModel)
        {
            if (!service.GetPostVMById(postId, userEmail, viewModel))
                return View("Error");

            return View(viewName, viewModel);
        }

        public ActionResult CompleteJobPost(string currentPostId, string returnUrl)
        {
            return CompletePost(JobPostService, currentPostId, returnUrl, GetEmailFromAuthCookie());
        }

        public ActionResult DeleteJobPost(string currentPostId, string returnUrl)
        {
            return DeletePost(JobPostService, currentPostId, returnUrl, GetEmailFromAuthCookie());
        }

        private ActionResult CompletePost(IBasePostService service, string currentPostId, string returnUrl, string userEmail)
        {
            if (!service.CompletePost(currentPostId, userEmail))
                return View("Error");

            if (!Url.IsLocalUrl(returnUrl))
                return View("Error");

            return ReturnTo(returnUrl);
        }

        public ActionResult DeletePost(IBasePostService service, string currentPostId, string returnUrl, string userEmail)
        {
            if (!service.DeletePost(currentPostId, userEmail))
                return View("Error");

            if (!Url.IsLocalUrl(returnUrl))
                return View("Error");

            return ReturnTo(returnUrl);
        }

        private ActionResult ReturnTo(string returnUrl)
        {
            if (String.IsNullOrEmpty(returnUrl))
                return View("Error");

            if (!Url.IsLocalUrl(returnUrl))
                return View("Error");

            return Redirect(returnUrl);
        }

        private string GetEmailFromAuthCookie()
        {
            HttpCookie authoCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authoCookie == null)
                return null;

            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authoCookie.Value);
            return ticket.Name;
        }
    }
}










//how to convert to 
//byte[] image = new byte[model.File.ContentLength];
//model.File.InputStream.Read(image, 0, image.Length);

//byte[] thePictureAsBytes = new byte[vm.Pictures[0].ContentLength];
//using (BinaryReader theReader = new BinaryReader(vm.Pictures[0].InputStream))
//{
//    thePictureAsBytes = theReader.ReadBytes(vm.Pictures[0].ContentLength);
//}
//string thePictureDataAsString = Convert.ToBase64String(thePictureAsBytes);

//var bytess = Convert.FromBase64String(testPicture);