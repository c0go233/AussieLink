using AussieLink.Contracts.Models.PostModels;
using AussieLink.Contracts.UnitOfWork;
using AussieLink.Services.HelperClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.Services.Services
{
    public class BasePostService
    {
        protected readonly IUnitOfWork UnitOfWork;

        public BasePostService(IUnitOfWork unitOfWork)
        {
            this.UnitOfWork = unitOfWork;
        }

        protected Guid GetUserIdFromUserEmail(string email)
        {
            if (email == null)
                return Guid.Empty;

            var user = UnitOfWork.Users.SingleOrDefault(u => u.Email == email);
            return user == null ? Guid.Empty : user.UserId;
        }

        protected bool GetDecryptedPostId(string postId, out int decryptedPostId)
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
    }
}
