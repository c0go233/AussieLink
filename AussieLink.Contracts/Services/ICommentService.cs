using AussieLink.Contracts.Dtos;
using AussieLink.Contracts.Responses;
using System.Collections.Generic;

namespace AussieLink.Contracts.Services
{
    public interface ICommentService
    {
        CommentSaveResponse SaveComment(CommentRequestDto dto, string userEmail);
        bool DeleteComment(string commentId, string userEmail);
        IEnumerable<CommentDto> GetComments(string postId, string postType, string userEmail);
    }
}