using AussieLink.Contracts.ViewModels.PostViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.Contracts.Services
{
    public interface IBasePostService
    {
        int? SavePost(BasePostVM viewModel, string userEmail);
        void PopulateCategories(BasePostVM viewModel);
        bool GetPostVMById(string postId, string userEmail, BasePostVM viewModel);
        void PopulateCategories(JobCategoriesVM jobCategories);
        bool CompletePost(string currentPostId, string userEmail);
        bool DeletePost(string currentPostId, string userEmail);
        bool RepostPost(string currentPostId, string userEmail);
    }
}
