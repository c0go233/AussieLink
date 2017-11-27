using AussieLink.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.Contracts.Responses
{
    public class BaseResponse
    {
        public bool Success { get; private set; }
        public ErrorCode ErrorCode { get; private set; }
        
        public BaseResponse(bool success)
        {
            this.Success = success;
            this.ErrorCode = ErrorCode.NOERROR;
        }

        public BaseResponse(bool success, ErrorCode errorCode)
        {
            this.Success = success;
            this.ErrorCode = errorCode;
        }
    }
}
