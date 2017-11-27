using AussieLink.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.Contracts.Responses
{
    public class SaveSharePostResponse : BaseResponse
    {
        public string PostId { get; set; }

        public SaveSharePostResponse(bool success, ErrorCode errorCode) : base(success, errorCode) { }
        public SaveSharePostResponse(bool success, string postId) : base(success)
        {
            this.PostId = postId;
        }
    }
}
