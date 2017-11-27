using AussieLink.Contracts.Dtos;
using AussieLink.Contracts.Enums;
using AussieLink.Contracts.ExtensionMethods;
using AussieLink.Contracts.Models;
using AussieLink.Contracts.Models.CommentModels;
using AussieLink.Contracts.Models.PostModels;
using AussieLink.Contracts.Models.PostModels.JobPostModels;
using AussieLink.Contracts.Models.PostModels.SharePostModels;
using AussieLink.Contracts.UnitOfWork;
using AussieLink.Contracts.ViewModels;
using AussieLink.Contracts.ViewModels.AdViewModels;
using AussieLink.Contracts.ViewModels.ManageAdViews;
using AussieLink.Contracts.ViewModels.PostViewModels;
using AussieLink.Contracts.ViewModels.PostViewModels.SharePostViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.Services.HelperClasses
{
    public class Mapper
    {
        public static SharePostVM SharePostToSharePostVM(SharePost sharePost, SharePostVM vm)
        {
            vm.DateAvailableFrom = sharePost.AvailableFrom.ToShortDateString();
            vm.Description = sharePost.Description;
            vm.Gender = sharePost.Gender.Name;
            vm.PlaceId = sharePost.PlaceId;
            vm.PostId = Encryptor.GetEncryptedString(sharePost.PostId.ToString());
            vm.ShareType= sharePost.ShareType.Name;
            vm.Price = sharePost.Price;
            vm.Suburb = sharePost.Address.Suburb.Name;
            vm.Title = sharePost.Title;
            
            return vm;
        }

        public static IEnumerable<PictureVM> PicturesToPictureVMs(IEnumerable<Picture> pictures, List<PictureVM> pictureVMs)
        {
            foreach(var picture in pictures)
            {
                var pictureVM = new PictureVM() { PictureId = Encryptor.GetEncryptedString(picture.PictureId.ToString()),
                PictureName = picture.ImageName, PictureSize = picture.Size, ImageSrc = GetImageSrcFrom(picture)};
                pictureVMs.Add(pictureVM);
            }
            return pictureVMs;
        }

        private static string GetImageSrcFrom(Picture picture)
        {
            var prefix = "data:" + picture.PictureType + ";base64,";
            var pictureData = Convert.ToBase64String(picture.Data);
            return prefix + pictureData;
        }


        public static SharePost SharePostVMToSharePost(SharePostVM vm, SharePost sharePost, byte shareTypeId, byte genderId, bool isCreation)
        {
            sharePost.AvailableFrom = GetDateTimeFrom(vm.DateAvailableFrom);

            if (isCreation)
                sharePost.DateCreated = DateTime.Now;

            sharePost.Description = vm.Description;
            sharePost.GenderId = genderId;
            sharePost.Price = vm.Price.Value;
            sharePost.ShareTypeId = shareTypeId;
            sharePost.Title = vm.Title;
            sharePost.PlaceId = vm.PlaceId;
            
            return null;
        }

        private static DateTime GetDateTimeFrom(string date)
        {
            IFormatProvider culture = new System.Globalization.CultureInfo("en-AU", true);
            return DateTime.Parse(date, culture, System.Globalization.DateTimeStyles.AssumeLocal);
        }

        public static IEnumerable<SuburbVM> SuburbToSuburbVM(IEnumerable<Suburb> suburbs, List<SuburbVM> suburbVMs)
        {
            foreach(var suburb in suburbs)
            {
                var suburbVm = new SuburbVM { Name = suburb.Name };
                suburbVMs.Add(suburbVm);
            }
            return suburbVMs;
        }

        public static IEnumerable<ManageAdPostTypeCategory> PostTypeToManageAdPostTypeCategory(
            IEnumerable<Contracts.Models.PostModels.PostType> postTypes, List<ManageAdPostTypeCategory> postTypeCategories)
        {
            foreach(var postType in postTypes)
            {
                var postTypeCategory = new ManageAdPostTypeCategory { Name = postType.Name };
                postTypeCategories.Add(postTypeCategory);
            }
            return postTypeCategories;
        }

        public static IEnumerable<ManageAdPost> GetManageAdPostsFrom(IEnumerable<JobPost> jobPosts)
        {
            //this method takes all the posts, job, rent, secondhand and make them into IEnumerable<ManageAdPost>

            var manageAdPosts = new List<ManageAdPost>();
            AddToManageAdPosts(manageAdPosts, jobPosts, Contracts.Enums.PostType.JOB.ToDescription());

            manageAdPosts.Sort((x, y) => y.DateCreated.CompareTo(x.DateCreated));
            return manageAdPosts;
        }

        private static void AddToManageAdPosts(List<ManageAdPost> manageAdPosts, IEnumerable<BasePost> posts, string type)
        {
            foreach(var post in posts)
            {
                var encryptedPostId = Encryptor.GetEncryptedString(post.PostId.ToString());
                var manageAdPost = new ManageAdPost(encryptedPostId, post.Title, post.Complete, post.DateCreated, type);
                manageAdPosts.Add(manageAdPost);
            }
        }

        public static IEnumerable<CommentDto> CommentsToCommentDtos(IEnumerable<Comment> comments, 
            List<CommentDto> commentDtos, Guid userId)
        {
            foreach(var comment in comments)
            {
                var encryptedCommentId = Encryptor.GetEncryptedString(comment.CommentId.ToString());
                var isOwned = comment.User.UserId == userId;
                var commentDto = new CommentDto(encryptedCommentId, comment.User.Name,
                    comment.Description, comment.DateCreated, isOwned);
                commentDtos.Add(commentDto);
            }

            return commentDtos;
        }

        public static void SignupVMToUserForSignUp(UserSignupVM vModel, User domain)
        {
            domain.Name = vModel.Name;
            domain.Email = vModel.Email;
        }

        public static IEnumerable<JobAdPostVM> JobPostsToJobAdPostVMs(IEnumerable<JobPost> jobPosts, 
            List<JobAdPostVM> jobAdPostVMs, int descriptionSize, int selectedPostId)
        {
            var currentDate = DateTime.Now;
            foreach(var jobPost in jobPosts)
            {
                var jobAdPostVM = new JobAdPostVM();
                JobPostToJobAdPostVM(jobPost, jobAdPostVM, false, descriptionSize, selectedPostId);
                jobAdPostVMs.Add(jobAdPostVM);
            }
            return jobAdPostVMs;
        }

        public static void JobPostToJobAdPostVM(JobPost jobPost, JobAdPostVM jobAdPostVM, 
            bool isDetailed, int? descriptionSize = null, int? selectedPostId = null)
        {
            BasePostToBaseAdVM(jobPost, jobAdPostVM, isDetailed, descriptionSize, selectedPostId);
            jobAdPostVM.ContractType = jobPost.ContractType.Name;
            jobAdPostVM.JobType = jobPost.JobType.Name;
            jobAdPostVM.Salary = JobRenderingTextConverter.GetSalaryRenderingText(jobPost.Salaries);
            jobAdPostVM.JobDay = JobRenderingTextConverter.GetJobDayRenderingText(jobPost.JobDays);
        }

        private static void BasePostToBaseAdVM(BasePost basePost, BaseAdPostVM baseAdPostVM, 
            bool isDetailed, int? descriptionSize = null, int? selectedPostId = null)
        {
            baseAdPostVM.DateCreated = DateConverter.GetAdDate(basePost.DateCreated);

            if (!isDetailed && basePost.Description.Length > descriptionSize)
                baseAdPostVM.Description = basePost.Description.Substring(0, descriptionSize.Value);
            else
                baseAdPostVM.Description = basePost.Description;

            baseAdPostVM.Place = basePost.Place.Name;
            baseAdPostVM.PostId = Encryptor.GetEncryptedString(basePost.PostId.ToString());
            baseAdPostVM.Title = basePost.Title;
            baseAdPostVM.UerName = basePost.User.Name;
            baseAdPostVM.Complete = basePost.Complete;

            //if selectedPostId is null, then selected is set to default boolean value false;
            if (selectedPostId != null)
                baseAdPostVM.Selected = basePost.PostId == selectedPostId.Value;

        }

        public static void JobPostVMToJobPost(JobPostVM vModel, JobPost jobPost, bool isCreation)
        {
            BasePostVMToBasePost(vModel, jobPost);
            jobPost.ContractTypeId = vModel.ContractTypeId;
            jobPost.JobTypeId = vModel.JobTypeId;

            if (isCreation)
                jobPost.DateCreated = DateTime.Now;
        }


        private static void BasePostVMToBasePost(BasePostVM vModel, BasePost basePost)
        {
            basePost.PlaceId = vModel.PlaceId;
            basePost.Title = vModel.Title;
            basePost.Description = vModel.Description;
        }

        public static void JobPostToJobPostVM(JobPost jobPost, JobPostVM vModel)
        {
            BasePostToBasePostVM(jobPost, vModel);
            vModel.ContractTypeId = jobPost.ContractTypeId;
            vModel.JobTypeId = jobPost.JobTypeId;
            PopulateDays(jobPost, vModel.JobPostDayVM);
            PopulateSalaries(jobPost, vModel.JobPostSalaryVM);
        }

        private static void BasePostToBasePostVM(BasePost basePost, BasePostVM vModel)
        {
            vModel.PostId = Encryptor.GetEncryptedString(basePost.PostId.ToString()); 
            vModel.PlaceId = basePost.PlaceId;
            vModel.Title = basePost.Title;
            vModel.Description = basePost.Description;
        }

        private static void PopulateSalaries(JobPost jobPost, JobPostSalaryVM vModel)
        {
            var count = jobPost.Salaries.Count;

            if (count == 0) { }
            else
            {
                var salary = jobPost.Salaries.Single(s => s.Size == Size.MINIMUM.ToDescription());
                vModel.MinSalary = salary.Amount;
                vModel.SalaryTypeId = salary.SalaryTypeId;
            }

            if (count == 2)
                vModel.MaxSalary = jobPost.Salaries.Single(s => s.Size == Size.MAXIMUM.ToDescription()).Amount;
        }

        private static void PopulateDays(JobPost jobPost, JobPostDayVM vModel)
        {
            var count = jobPost.JobDays.Count;

            if (count == 0) { }
            else
                vModel.MinDay = jobPost.JobDays.Single(d => d.Size == Size.MINIMUM.ToDescription()).Amount;

            if (count == 2)
                vModel.MaxDay = jobPost.JobDays.Single(d => d.Size == Size.MAXIMUM.ToDescription()).Amount;
        }


        public static void PopulateJobPostCategories(IJobPostCategoryUnitOfWork jobPostCategoryUOW, JobCategoriesVM jobCategories)
        {
            jobCategories.ContractTypes = jobPostCategoryUOW.ContractTypes.GetAll();
            jobCategories.DayCategories = jobPostCategoryUOW.DayCategories.GetAll();
            jobCategories.JobTypes = jobPostCategoryUOW.JobTypes.GetAll();
            jobCategories.Places = jobPostCategoryUOW.Places.GetAll();
            jobCategories.SalaryTypes = jobPostCategoryUOW.SalaryTypes.GetAll();
        }

        public static void PopulateJobAdCategories(IJobPostCategoryUnitOfWork jobPostCategoryUOW, JobAdCategoriesVM jobCategories)
        {
            PopulateJobPostCategories(jobPostCategoryUOW, jobCategories);
            jobCategories.HourlySalaryCategories = jobPostCategoryUOW.HourlySalaryCategories.GetAll();
            jobCategories.WeeklySalaryCategories = jobPostCategoryUOW.WeeklySalaryCategories.GetAll();
        }
    }
}
