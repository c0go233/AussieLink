using AussieLink.Contracts.Enums;
using AussieLink.Contracts.ExtensionMethods;
using AussieLink.Contracts.Models.PostModels;
using AussieLink.Contracts.Models.PostModels.JobPostModels;
using AussieLink.Contracts.Responses;
using AussieLink.Contracts.Services;
using AussieLink.Contracts.UnitOfWork;
using AussieLink.Contracts.ViewModels.AdViewModels;
using AussieLink.Services.HelperClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.Services.Services
{
    public class JobAdService : BaseAdService, IJobAdService
    {
        private static readonly int PAGESIZE = 10;
        private static readonly int DESCRIPTIONSIZE = 100;

        private readonly IJobPostUnitOfWork JobPostUnitOfWork;
        private readonly IJobPostCategoryUnitOfWork JobCategoryUnitOfWork;

        public JobAdService(IJobPostUnitOfWork jobPostUnitOfWork, IJobPostCategoryUnitOfWork jobCategoryUnitOfWork, IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            this.JobPostUnitOfWork = jobPostUnitOfWork;
            this.JobCategoryUnitOfWork = jobCategoryUnitOfWork;
        }

        public GetJobAdVM GetAds(JobAdFilterVM vm, int pageIndex, GetJobAdVM getJobAdVM, string selectedPostId)
        {
            var filteredPosts = GetFilteredPosts(vm);
            var totalPostsCount = JobPostUnitOfWork.JobPosts.GetCountForJobPosts(filteredPosts);
            var pagedPosts = JobPostUnitOfWork.JobPosts.GetPagedPosts(pageIndex, PAGESIZE, filteredPosts);


            //postId starts from 1 so if this fails, it will return 0 then no match 
            int decryptedSelectedPostId;
            base.ConvertToDecryptedPostId(selectedPostId, out decryptedSelectedPostId);

            getJobAdVM.PagedPosts = Mapper.JobPostsToJobAdPostVMs(pagedPosts, new List<JobAdPostVM>(), DESCRIPTIONSIZE, decryptedSelectedPostId);
            getJobAdVM.Pager = new Pager(totalPostsCount, pageIndex, PAGESIZE);

            return getJobAdVM;
        }

        private IEnumerable<JobPost> GetFilteredPosts(JobAdFilterVM vm)
        {
            var placeId = JobCategoryUnitOfWork.GetPrimaryKeyValueByName<Place>(JobCategoryUnitOfWork.Places, m => m.Name == vm.Place, vm.Place);
            var contractId = JobCategoryUnitOfWork.GetPrimaryKeyValueByName<ContractType>(JobCategoryUnitOfWork.ContractTypes, m => m.Name == vm.ContractType, vm.ContractType);
            var salaryType = JobCategoryUnitOfWork.GetPrimaryKeyValueByName<SalaryType>(JobCategoryUnitOfWork.SalaryTypes, m => m.Name == vm.SalaryType, vm.SalaryType);
            var jobTypeId = JobCategoryUnitOfWork.GetPrimaryKeyValueByName<JobType>(JobCategoryUnitOfWork.JobTypes, m => m.Name == vm.JobType, vm.JobType);

            //default values for pageindex, and salarytypeId
            var salaryTypeId = salaryType == null ? 2 : salaryType.Value;

            var filteredPosts = JobPostUnitOfWork.JobPosts.GetFilteredPosts(placeId, contractId, vm.Salary, salaryTypeId, vm.Day, jobTypeId, vm.Keyword);
            return filteredPosts;
        }

        private JobPost GetJobpostByPostId(string currentPostId)
        {
            int decryptedPostId;
            if (!base.ConvertToDecryptedPostId(currentPostId, out decryptedPostId))
                return null;
            
            var jobPost = JobPostUnitOfWork.JobPosts.GetJobAdDetailedPostById(decryptedPostId);

            return jobPost == null ? null : jobPost;
        }


        public void PopulateCategories(JobAdCategoriesVM jobAdCategoriesVM)
        {
            Mapper.PopulateJobAdCategories(JobCategoryUnitOfWork, jobAdCategoriesVM);
        }


        public bool SetJobAdDetailVM(string currentPostId, string userEmail, JobAdDetailVM vModel)
        {
            
            var jobPost = GetJobpostByPostId(currentPostId);
            if (jobPost == null)
                return false;

            if (jobPost.Complete == true)
            {
                var user = UnitOfWork.Users.SingleOrDefault(m => m.Email == userEmail);
                if (user == null)
                    return false;
                else if (jobPost.UserId != user.UserId)
                    return false;
            }

            Mapper.JobPostToJobAdPostVM(jobPost, vModel.JobAdPostVM, true);
            vModel.IsPostOwned = base.IsPostOwnedBy(userEmail, jobPost.UserId);

            return true;
        }


    }
}
