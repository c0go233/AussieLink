using AussieLink.Contracts.Models.CommentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.Contracts.Models.PostModels
{
    public class PostType
    {
        public byte PostTypeId { get; set; }
        public string Name { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }
}
