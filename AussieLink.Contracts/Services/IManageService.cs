using AussieLink.Contracts.Responses;
using AussieLink.Contracts.ViewModels.ManageAdViews;

namespace AussieLink.Contracts.Services
{
    public interface IManageService
    {
        GetManageAdVMResponse GetManageAdVM(string email, ManageAdVM vm, int pageIndex);
        ManageProfileVM GetManageProfileVM(string email, ManageProfileVM vm);
        bool UpdateProfile(ManageProfileVM vm, string email);
    }
}