using AussieLink.Contracts.Models.CommentModels;
using AussieLink.Contracts.UnitOfWork;
using AussieLink.Services.HelperClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.Services.Services
{
    public class BaseAdService
    {
        protected readonly IUnitOfWork UnitOfWork;

        public BaseAdService(IUnitOfWork unitOfWork)
        {
            this.UnitOfWork = unitOfWork;
        }

        protected bool ConvertToDecryptedPostId(string postId, out int decryptedPostId)
        { 
            try
            {
                var decryptedPostIdInString = Encryptor.GetDecryptedString(postId);
                return int.TryParse(decryptedPostIdInString, out decryptedPostId);
            }
            catch (Exception)
            {
                decryptedPostId = 0;
                return false;
            }
        }

        protected bool IsPostOwnedBy(string userEmail, Guid postUserId)
        {
            var user = UnitOfWork.Users.SingleOrDefault(m => m.Email == userEmail);
            if (user == null)
                return false;

            if (user.UserId != postUserId)
                return false;

            return true;
        }

        protected bool SetCommnets(string postTypeName, string currentPostId, IEnumerable<Comment> comments)
        {
            var postType = UnitOfWork.PostTypes.SingleOrDefault(m => m.Name == postTypeName);
            if (postType == null)
                return false;

            int decryptedPostId;
            if (!ConvertToDecryptedPostId(currentPostId, out decryptedPostId))
                return false;

            comments = UnitOfWork.Comments.GetFullCommentsBy(m => (m.PostTypeId == postType.PostTypeId) && (m.PostId == decryptedPostId));
            return true;
        }
    }
}
