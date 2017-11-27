using AussieLink.Contracts.Models.PostModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.Contracts.Models.CommentModels
{
    public class Comment
    {
        public int CommentId { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public int PostId { get; set; }

        public byte PostTypeId { get; set; }

        public string Description { get; set; }
        public DateTime DateCreated { get; set; }

        public Comment() { }

        public Comment(Guid userId, int postId, byte postTypeId, string description)
        {
            this.UserId = userId;
            this.PostId = postId;
            this.PostTypeId = postTypeId;
            this.Description = description;
            this.DateCreated = DateTime.Now;
        }
    }
}
