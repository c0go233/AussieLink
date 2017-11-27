using AussieLink.Contracts.Responses;
using AussieLink.Contracts.ViewModels.PostViewModels.SharePostViewModels;
using System.Collections.Generic;
using System.Web;

namespace AussieLink.Contracts.Services
{
    public interface ISharePostService
    {
        SharePostVM PopulateCategories(SharePostVM vm);
        SaveSharePostResponse SaveSharePost(SharePostVM vm, IEnumerable<HttpPostedFileBase> unsavedPictures, string userEmail);
        bool SetSharePostVMForEdit(SharePostVM vm, string userEmail);
    }
}