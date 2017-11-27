using AussieLink.Contracts.Models.PostModels.JobPostModels;
using AussieLink.Contracts.Repositories;
using AussieLink.DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using AussieLink.Contracts.Enums;
using AussieLink.Contracts.ExtensionMethods;

namespace AussieLink.DAL.Repositories
{
    public class JobPostRepository : BaseRepository<JobPost>, IJobPostRepository
    {
        private DataContext DataContext
        {
            get { return Context as DataContext; }
        }

        public JobPostRepository(DataContext context) : base(context) { }

        public JobPost GetFullJobPost(int postId)
        {
            var jobPost = DataContext.JobPosts
                .Include(p => p.Salaries)
                .Include(p => p.JobDays)
                .SingleOrDefault(m => (m.PostId == postId) && (m.Cancel == false));

            return jobPost;
        }

        public JobPost GetJobAdDetailedPostById(int postId)
        {
            return DataContext.JobPosts.Include(p => p.ContractType)
                .Include(p => p.JobType).Include(p => p.User).Include(p => p.Place)
                .Include(j => j.Salaries.Select(c => c.SalaryType)).Include(p => p.JobDays).SingleOrDefault(m => (m.PostId == postId) && (m.Cancel == false));
        }

        public IEnumerable<JobPost> GetFilteredPosts(int? placeId, int? contractTypeId, decimal? minSalary
            ,int salaryTypeId, byte? minDay, int? jobTypeId, string keyword)
        {
            var posts = DataContext.JobPosts.Include(p => p.ContractType)
                .Include(p => p.JobType).Include(p => p.User).Include(p => p.Place)
                .Include(j => j.Salaries.Select(c => c.SalaryType)).Include(p => p.JobDays).Where(m => (m.Complete == false) && (m.Cancel == false));

            if (placeId.HasValue)
                posts = posts.Where(m => m.PlaceId == placeId.Value);

            if (contractTypeId.HasValue)
                posts = posts.Where(m => m.ContractTypeId == contractTypeId.Value);

            if (jobTypeId.HasValue)
                posts = posts.Where(m => m.JobTypeId == jobTypeId.Value);

            if (!String.IsNullOrEmpty(keyword))
                posts = posts.Where(m => m.Title.Contains(keyword));

            var minimumSize = Size.MINIMUM.ToDescription();

            if (minDay.HasValue)
                posts = posts.Where(m => m.JobDays.Any(d => (d.Size == minimumSize) && (d.Amount >= minDay.Value)));

            if (minSalary.HasValue)
            {
                posts = posts.Where(m => m.Salaries.Any(s => s.SalaryTypeId == salaryTypeId));
                posts = posts.Where(m => m.Salaries.Any(s => (s.Size == minimumSize) && (s.Amount >= minSalary.Value)));
            }

            return posts;
        }


        public int GetCountForJobPosts(IEnumerable<JobPost> jobPosts)
        {
            return jobPosts.Count();
        }

        public IEnumerable<JobPost> GetPagedPosts(int currentPageIndex, int pageSize, IEnumerable<JobPost> filteredPosts)
        {
            var queryableFilteredPosts = filteredPosts.AsQueryable();
            var posts = queryableFilteredPosts.OrderByDescending(m => m.DateCreated).Skip(((currentPageIndex -1)  * pageSize)).Take(pageSize);
            return posts;
        }

        public IEnumerable<JobPost> GetPostsForManageAd(Guid userId)
        {
            return DataContext.JobPosts.Where(m => (m.UserId == userId) && (m.Cancel == false));
        }
    }
    




    public class JobDayRepository : BaseRepository<JobDay>, IJobDayRepository
    {
        private DataContext DataContext
        {
            get { return Context as DataContext; }
        }

        public JobDayRepository(DataContext context) : base(context) { }
    }

    public class SalaryRepository : BaseRepository<Salary>, ISalaryRepository
    {
        private DataContext DataContext
        {
            get { return Context as DataContext; }
        }

        public SalaryRepository(DataContext context) : base(context) { }
    }
}
