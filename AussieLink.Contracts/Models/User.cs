using AussieLink.Contracts.Models.CommentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.Contracts.Models
{
    public class User
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsCanceled { get; set; }
        public string Salt { get; set; }
        public bool SocialLogin { get; set; }


        public User()
        { }

        public User(Guid userId, DateTime dateCreated, bool isCanceled, bool socialLogin)
        {
            this.UserId = userId;
            this.DateCreated = dateCreated;
            this.IsCanceled = isCanceled;
            this.SocialLogin = socialLogin;
        }
    }
}
