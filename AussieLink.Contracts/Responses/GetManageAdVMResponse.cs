using AussieLink.Contracts.Enums;
using AussieLink.Contracts.ViewModels.ManageAdViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.Contracts.Responses
{
    public class GetManageAdVMResponse : BaseResponse
    {
        public ManageAdVM ManageAdVM { get; set; }

        public GetManageAdVMResponse(bool success, ErrorCode errorCode) : base(success, errorCode) { }

        public GetManageAdVMResponse(bool success, ManageAdVM manageAdVM) : base(success)
        {
            this.ManageAdVM = manageAdVM;
        }

    }
}
