using AussieLink.Contracts.Enums;
using AussieLink.Contracts.ExtensionMethods;
using AussieLink.Contracts.Responses;
using AussieLink.Contracts.Services;
using AussieLink.Contracts.UnitOfWork;
using AussieLink.Contracts.ViewModels.AdViewModels;
using AussieLink.Contracts.ViewModels.ManageAdViews;
using AussieLink.Services.HelperClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.Services.Services
{
    public class ManageService : IManageService
    {
        private readonly IJobPostUnitOfWork JobPostUnitOfWork;
        private readonly IUnitOfWork UnitOfWork;
        //here IRentPostUnitOfWork

        private readonly int ManageAdPageSize = 10;

        public ManageService(IJobPostUnitOfWork jobPostUnitOfWork, IUnitOfWork unitOfWork)
        {
            this.JobPostUnitOfWork = jobPostUnitOfWork;
            this.UnitOfWork = unitOfWork;
        }

        public GetManageAdVMResponse GetManageAdVM(string email, ManageAdVM vm, int pageIndex)
        {
            var user = UnitOfWork.Users.SingleOrDefault(m => m.Email == email);
            if (user == null)
                return new GetManageAdVMResponse(false, Contracts.Enums.ErrorCode.MANAGEADBADREQUEST);

            var jobPosts = JobPostUnitOfWork.JobPosts.GetPostsForManageAd(user.UserId);
            var pagedJobPosts = JobPostUnitOfWork.JobPosts.GetPagedPosts(pageIndex, ManageAdPageSize, jobPosts);
            var manageAdPosts = Mapper.GetManageAdPostsFrom(pagedJobPosts);

            var postTypes = UnitOfWork.PostTypes.GetAll();
            var postTypeCategories = Mapper.PostTypeToManageAdPostTypeCategory(postTypes, new List<ManageAdPostTypeCategory>());

            //should add the count of rent posts and second hand posts
            vm.TotalPostCount = jobPosts.Count();

            vm.Posts = manageAdPosts;
            vm.PostTypes = postTypeCategories;
            vm.CurrentMyAccountMenu = MyAccountMenu.MANAGEADS.ToDescription();
            vm.Pager = new Pager(jobPosts.Count(), pageIndex, ManageAdPageSize);

            return new GetManageAdVMResponse(true, vm);
        }

        public ManageProfileVM GetManageProfileVM(string email, ManageProfileVM vm)
        {
            var user = UnitOfWork.Users.SingleOrDefault(m => (m.Email == email) && (m.IsCanceled == false));
            if (user == null)
                return null;

            var manageProfileVM = new ManageProfileVM(MyAccountMenu.PROFILE.ToDescription(), user.Name, user.Email);
            return manageProfileVM;
        }

        public bool UpdateProfile(ManageProfileVM vm, string email)
        {
            var user = UnitOfWork.Users.SingleOrDefault(m => m.Email == email);
            if (user == null)
                return false;

            if (user.IsCanceled)
                return false;

            user.Name = vm.Name;
            UnitOfWork.Complete();
            return true;
        }

    }
}
