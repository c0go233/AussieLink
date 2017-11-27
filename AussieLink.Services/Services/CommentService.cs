using AussieLink.Contracts.Dtos;
using AussieLink.Contracts.Enums;
using AussieLink.Contracts.Models.CommentModels;
using AussieLink.Contracts.Responses;
using AussieLink.Contracts.Services;
using AussieLink.Contracts.UnitOfWork;
using AussieLink.Services.HelperClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.Services.Services
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork UnitOfWork;
        public readonly IJobPostUnitOfWork JobPostUnitOfWork;

        public CommentService(IUnitOfWork unitOfWork, IJobPostUnitOfWork jobPostUnitOfWork)
        {
            this.UnitOfWork = unitOfWork;
            this.JobPostUnitOfWork = jobPostUnitOfWork;
        }

        public IEnumerable<CommentDto> GetComments(string postId, string postType, string userEmail)
        {
            int decryptedPostId;
            if (!GetDecryptedId(postId, out decryptedPostId))
                return null;

            var postTypeInDb = UnitOfWork.PostTypes.SingleOrDefault(m => m.Name == postType);
            if (postTypeInDb == null)
                return null;

            var user = UnitOfWork.Users.SingleOrDefault(m => m.Email == userEmail);
            Guid userId = Guid.Empty;
            if (user != null)
                userId = user.UserId;

            var comments = UnitOfWork.Comments.GetFullCommentsBy(m => (m.PostId == decryptedPostId)
            && (m.PostTypeId == postTypeInDb.PostTypeId));

            return Mapper.CommentsToCommentDtos(comments, new List<CommentDto>(), userId);
        }

        public bool DeleteComment(string commentId, string userEmail)
        {
            int decryptedCommentId;
            if (!GetDecryptedId(commentId, out decryptedCommentId))
                return false;

            var comment = UnitOfWork.Comments.FindById(decryptedCommentId);
            if (comment == null)
                return false;

            var user = UnitOfWork.Users.SingleOrDefault(m => m.Email == userEmail);
            if (user == null)
                return false;

            if (comment.UserId != user.UserId)
                return false;

            UnitOfWork.Comments.Remove(comment);
            UnitOfWork.Complete();
            return true;
        }

        public CommentSaveResponse SaveComment(CommentRequestDto dto, string userEmail)
        {
            var user = UnitOfWork.Users.SingleOrDefault(m => m.Email == userEmail);
            if (user == null)
                return new CommentSaveResponse(false, ErrorCode.COMMENTBADREQUEST);

            int decryptedPostId;
            if (!GetDecryptedId(dto.PostId, out decryptedPostId))
                return new CommentSaveResponse(false, ErrorCode.COMMENTBADREQUEST);

            var post = JobPostUnitOfWork.JobPosts.FindById(decryptedPostId);
            if (post == null || post.Cancel == true)
                return new CommentSaveResponse(false, ErrorCode.COMMENTBADREQUEST);

            var postType = UnitOfWork.PostTypes.SingleOrDefault(m => m.Name == dto.PostType);
            if (postType == null)
                return new CommentSaveResponse(false, ErrorCode.COMMENTBADREQUEST);

            if (String.IsNullOrEmpty(dto.CommentId))
                return SaveNewComment(user.UserId, post.PostId, postType.PostTypeId, dto.Description, user.Name);
            else
                return UpdateComment(dto.CommentId, decryptedPostId, user.UserId, postType.PostTypeId, dto.Description);
        }

        private CommentSaveResponse UpdateComment(string commentId, int postId, Guid userId, byte postTypeId, string description)
        {
            int decryptedCommentId;
            if (!GetDecryptedId(commentId, out decryptedCommentId))
                return new CommentSaveResponse(false, ErrorCode.COMMENTBADREQUEST);

            var commentInDb = UnitOfWork.Comments.SingleOrDefault(m =>
            (m.CommentId == decryptedCommentId) && (m.PostTypeId == postTypeId)
            && (m.PostId == postId) && (m.UserId == userId));

            if (commentInDb == null)
                return new CommentSaveResponse(false, ErrorCode.COMMENTBADREQUEST);

            commentInDb.Description = description;
            UnitOfWork.Complete();

            return new CommentSaveResponse(true, new CommentDto { Description = commentInDb.Description });
        }

        private CommentSaveResponse SaveNewComment(Guid userId, int postId, byte postTypeId, string description, string userName)
        {
            var comment = CreateComment(userId, postId, postTypeId, description);
            var commentDto = GetCommentDto(comment, userName);
            return new CommentSaveResponse(true, commentDto);
        }

        private CommentDto GetCommentDto(Comment comment, string userName)
        {
            var encryptedCommentId = Encryptor.GetEncryptedString(comment.CommentId.ToString());
            var commentDto = new CommentDto(encryptedCommentId, userName, comment.Description, comment.DateCreated, true);
            return commentDto;
        }

        private Comment CreateComment(Guid userId, int postId, byte postTypeId, string description)
        {
            var comment = new Comment(userId, postId, postTypeId, description);
            UnitOfWork.Comments.Add(comment);
            UnitOfWork.Complete();
            return comment;
        }

        private bool GetDecryptedId(string stringId, out int decryptedPostId)
        {
            try
            {
                var decryptedPostIdInString = Encryptor.GetDecryptedString(stringId);
                return int.TryParse(decryptedPostIdInString, out decryptedPostId);
            }
            catch (Exception)
            {
                decryptedPostId = 0;
                return false;
            }
        }
    }
}
