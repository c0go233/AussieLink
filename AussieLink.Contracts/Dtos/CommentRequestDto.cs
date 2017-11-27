using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.Contracts.Dtos
{
    public class CommentRequestDto
    {
        public string CommentId { get; set; }
        public string PostId { get; set; }
        public string PostType { get; set; }
        public string Description { get; set; }
    }
}
