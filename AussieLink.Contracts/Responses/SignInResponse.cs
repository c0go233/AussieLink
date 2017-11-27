using AussieLink.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.Contracts.Responses
{
    public class SignInResponse : BaseResponse
    {
        public Guid UserId { get; set; }
        public string UserEmail { get; set; }

        public SignInResponse(bool success, Guid userId, string userEmail) : base(success)
        {
            this.UserId = userId;
            this.UserEmail = userEmail;
        }

        public SignInResponse(bool success, ErrorCode errorCode) : base(success, errorCode)
        { }
    }
}
