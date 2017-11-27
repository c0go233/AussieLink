using AussieLink.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.Contracts.Responses
{
    public class SignUpResponse : BaseResponse
    {
        public Guid UserId { get; private set; }

        public SignUpResponse(bool success, Guid userId) : base(success)
        {
            this.UserId = userId;
        }

        public SignUpResponse(bool success, ErrorCode errorCode) : base(success, errorCode)
        {}
    }
}
