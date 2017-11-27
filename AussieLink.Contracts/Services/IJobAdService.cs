using System.Collections.Generic;
using AussieLink.Contracts.ViewModels.AdViewModels;

namespace AussieLink.Contracts.Services
{
    public interface IJobAdService
    {
        GetJobAdVM GetAds(JobAdFilterVM vm, int pageIndex ,GetJobAdVM getJobAdVM, string selectedPostId);
        bool SetJobAdDetailVM(string currentPostId, string userEmail, JobAdDetailVM vModel);
        void PopulateCategories(JobAdCategoriesVM jobAdCategoriesVM);
    }
}