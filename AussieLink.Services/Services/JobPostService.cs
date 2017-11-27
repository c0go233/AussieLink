using AussieLink.Contracts.Enums;
using AussieLink.Contracts.ExtensionMethods;
using AussieLink.Contracts.Models.PostModels.JobPostModels;
using AussieLink.Contracts.Services;
using AussieLink.Contracts.UnitOfWork;
using AussieLink.Contracts.ViewModels.PostViewModels;
using AussieLink.Services.HelperClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.Services.Services
{
    public class JobPostService : BasePostService, IJobPostService
    {
        private readonly IJobPostUnitOfWork JobPostUOW;
        private readonly IJobPostCategoryUnitOfWork JobPostCategoryUOW;

        public JobPostService(IJobPostUnitOfWork jobPostUOW, IJobPostCategoryUnitOfWork jobPostCategoryUOW, IUnitOfWork unitOfWork) 
            : base(unitOfWork)
        {
            this.JobPostUOW = jobPostUOW;
            this.JobPostCategoryUOW = jobPostCategoryUOW;
        }

        public bool DeletePost(string currentPostId, string userEmail)
        {
            var userId = base.GetUserIdFromUserEmail(userEmail);
            if (userId == Guid.Empty)
                return false;

            var jobPost = GetJobPostForUserId(currentPostId, userId);
            if (jobPost == null)
                return false;

            jobPost.Cancel = true;
            JobPostUOW.Complete();

            return true;
        }

        public bool CompletePost(string currentPostId, string userEmail)
        {
            var userId = base.GetUserIdFromUserEmail(userEmail);
            if (userId == Guid.Empty)
                return false;

            var jobPost = GetJobPostForUserId(currentPostId, userId);
            if (jobPost == null)
                return false;

            jobPost.Complete = true;
            JobPostUOW.Complete();

            return true;
        }

        public bool RepostPost(string currentPostId, string userEmail)
        {
            var userId = base.GetUserIdFromUserEmail(userEmail);
            if (userId == Guid.Empty)
                return false;

            var jobPost = GetJobPostForUserId(currentPostId, userId);
            if (jobPost == null)
                return false;

            jobPost.Complete = false;
            jobPost.DateCreated = DateTime.Now;
            JobPostUOW.Complete();

            return true;
        }

        private int? UpdateJobPost(JobPostVM jobPostVM, Guid userIdInGuid)
        {
            var jobPost = GetJobPostForUserId(jobPostVM.PostId, userIdInGuid);
            if (jobPost == null)
                return null;

            Mapper.JobPostVMToJobPost(jobPostVM, jobPost, false);
            SaveSalariesAndDays(jobPostVM, jobPost.PostId);

            return jobPost.PostId;
        }

        private JobPost GetJobPostForUserId(string postId, Guid userIdInGuid)
        {
            int decryptedPostId;
            if (!base.GetDecryptedPostId(postId, out decryptedPostId))
                return null;

            var post = JobPostUOW.JobPosts.GetFullJobPost(decryptedPostId);

            if (post == null)
                return null;

            if (!post.IsOwnedByUserId(userIdInGuid))
                return null;

            if (post.Cancel)
                return null;

            return post;
        }

        public bool GetPostVMById(string postId, string userEmail, BasePostVM viewModel)
        {
            var userIdInGuid = base.GetUserIdFromUserEmail(userEmail);
            if (userIdInGuid == Guid.Empty)
                return false;

            var jobPost = GetJobPostForUserId(postId, userIdInGuid);
            if (jobPost == null)
                return false;

            var jobPostVM = (JobPostVM)viewModel;
            Mapper.JobPostToJobPostVM(jobPost, jobPostVM);
            PopulateCategories(jobPostVM);

            return true;
        }

        public int? SavePost(BasePostVM model, string userEmail)
        {
            var userIdInGuid = base.GetUserIdFromUserEmail(userEmail);
            if (userIdInGuid == Guid.Empty)
                return null;

            JobPostVM jobPostVM = (JobPostVM)model;

            if (model.PostId == null)
                return CreateJobPost(jobPostVM, userIdInGuid);
            else
                return UpdateJobPost(jobPostVM, userIdInGuid);
        }

        private int CreateJobPost(JobPostVM jobPostVM, Guid userIdInGuid)
        {
            var jobPost = new JobPost { UserId = userIdInGuid };
            Mapper.JobPostVMToJobPost(jobPostVM, jobPost, true);
            JobPostUOW.JobPosts.Add(jobPost);
            JobPostUOW.Complete();
            SaveSalariesAndDays(jobPostVM, jobPost.PostId);
            return jobPost.PostId;
        }

        private void SaveSalariesAndDays(JobPostVM jobPostVM, int jobPostId)
        {
            SaveDays(jobPostVM.JobPostDayVM, jobPostId);
            SaveSalaries(jobPostVM.JobPostSalaryVM, jobPostId);
            JobPostUOW.Complete();
        }

        private void SaveSalaries(JobPostSalaryVM model, int postId)
        {
            if (model.SalaryTypeId == null)
                DeleteSalaries(postId);
            else
            {
                SaveSalary(postId, model.MinSalary, Size.MINIMUM.ToDescription(), model.SalaryTypeId.Value);
                SaveSalary(postId, model.MaxSalary, Size.MAXIMUM.ToDescription(), model.SalaryTypeId.Value);
            }
        }

        private void SaveSalary(int postId, decimal? amount, string size, int salaryTypeId)
        {
            if (amount == null)
                DeleteSalary(postId, size);
            else
                UpdateSalary(postId, amount.Value, size, salaryTypeId);
        }

        private void UpdateSalary(int postId, decimal amount, string size, int salaryTypeId)
        {
            var salary = JobPostUOW.Salaries.SingleOrDefault(m => (m.PostId == postId) && (m.Size == size));
            if (salary == null)
                CreateSalary(postId, amount, size, salaryTypeId);
            else
            {
                salary.Amount = amount;
                salary.SalaryTypeId = salaryTypeId;
            }
        }

        private void CreateSalary(int postId, decimal amount, string size, int salaryTypeId)
        {
            var salary = new Salary { PostId = postId, Amount = amount, Size = size, SalaryTypeId = salaryTypeId };
            JobPostUOW.Salaries.Add(salary);
        }

        private void DeleteSalaries(int postId)
        {
            DeleteSalary(postId, Size.MINIMUM.ToDescription());
            DeleteSalary(postId, Size.MAXIMUM.ToDescription());
        }

        private void DeleteSalary(int postId, string size)
        {
            var salary = JobPostUOW.Salaries.SingleOrDefault(m => (m.PostId == postId) && (m.Size == size));
            if (salary != null)
                JobPostUOW.Salaries.Remove(salary);
        }

        private void SaveDays(JobPostDayVM model, int postId)
        {
            SaveDay(postId, model.MinDay, Size.MINIMUM.ToDescription());
            SaveDay(postId, model.MaxDay, Size.MAXIMUM.ToDescription());
        }

        private void SaveDay(int postId, byte? amount, string size)
        {
            if (amount == null)
                DeleteDay(postId, size);
            else
                UpdateDay(postId, amount.Value, size);
        }

        private void UpdateDay(int postId, byte amount, string size)
        {
            var day = JobPostUOW.JobDays.SingleOrDefault(m => (m.PostId == postId) && (m.Size == size));
            if (day == null)
                CreateDay(postId, amount, size);
            else
                day.Amount = amount;
        }

        private void CreateDay(int postId, byte amount, string size)
        {
            var day = new JobDay { PostId = postId, Amount = amount, Size = size };
            JobPostUOW.JobDays.Add(day);
        }

        private void DeleteDay(int postId, string size)
        {
            var day = JobPostUOW.JobDays.SingleOrDefault(m => (m.PostId == postId) && (m.Size == size));
            if (day != null)
                JobPostUOW.JobDays.Remove(day);
        }

        public void PopulateCategories(BasePostVM model)
        {
            var jobPostVM = (JobPostVM)model;
            PopulateCategories(jobPostVM.JobCategories);
        }

        public void PopulateCategories(JobCategoriesVM jobCategories)
        {
            Mapper.PopulateJobPostCategories(JobPostCategoryUOW, jobCategories);
        }

    }
}
