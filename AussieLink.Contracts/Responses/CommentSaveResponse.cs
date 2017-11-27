using AussieLink.Contracts.Dtos;
using AussieLink.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.Contracts.Responses
{
    public class CommentSaveResponse : BaseResponse
    {
        public CommentDto Comment { get; set; }

        public CommentSaveResponse(bool success, CommentDto comment) : base(success)
        {
            this.Comment = comment;
        }

        public CommentSaveResponse(bool success, ErrorCode errorCode) : base(success, errorCode)
        { }
    }
}
