using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.Contracts.Dtos
{
    public class CommentDto
    {
        public string CommentId { get; set; }
        public string UserName { get; set; }
        public string Description { get; set; }
        public string DateCreated { get; set; }
        public bool IsOwned { get; set; }

        public CommentDto() { }

        public CommentDto(string commentId, string userName, string description, DateTime timeCreated, bool isOwned)
        {
            this.CommentId = commentId;
            this.UserName = userName;
            this.Description = description;
            this.DateCreated = timeCreated.ToShortDateString();
            this.IsOwned = isOwned;
        }
    }
}
