using AussieLink.Contracts.Models.CommentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.Contracts.Models.PostModels
{
    public class BasePost
    {
        public int PostId { get; set; }

        public User User { get; set; }
        public Guid UserId { get; set; }

        public Place Place { get; set; }
        public int PlaceId { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public bool Complete { get; set; }
        public bool Cancel { get; set; }


        public bool IsOwnedByUserId(Guid userId)
        {
            return this.UserId == userId;
        }
    }
}
