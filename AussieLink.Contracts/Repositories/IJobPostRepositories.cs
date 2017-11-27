using AussieLink.Contracts.Models.PostModels.JobPostModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.Contracts.Repositories
{
    public interface IJobPostRepository : IBaseRepository<JobPost>
    {
        JobPost GetFullJobPost(int postId);
        IEnumerable<JobPost> GetPagedPosts(int currentPageIndex, int pageSize, IEnumerable<JobPost> filteredPosts);
        int GetCountForJobPosts(IEnumerable<JobPost> jobPosts);
        IEnumerable<JobPost> GetFilteredPosts(int? placeId, int? contractTypeId, 
            decimal? minSalary, int salaryTypeId, byte? minDay, int? jobTypeId, string keyword);
        JobPost GetJobAdDetailedPostById(int postId);
        IEnumerable<JobPost> GetPostsForManageAd(Guid userId);
    }

    public interface IJobDayRepository : IBaseRepository<JobDay>
    {
    }

    public interface ISalaryRepository : IBaseRepository<Salary>
    {
    }
}
