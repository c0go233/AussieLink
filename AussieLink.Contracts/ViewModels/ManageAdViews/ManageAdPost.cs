using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.Contracts.ViewModels.ManageAdViews
{
    public class ManageAdPost
    {
        public string PostId { get; set; }
        public string Title { get; set; }
        public bool Complete { get; set; }
        public DateTime DateCreated { get; set; }
        public string PostType { get; set; }

        public ManageAdPost() { }

        public ManageAdPost(string postId, string title, bool complete, DateTime dateCreated, string postType)
        {
            this.PostId = postId;
            this.Title = title;
            this.Complete = complete;
            this.DateCreated = dateCreated;
            this.PostType = postType;
        }
    }
}
