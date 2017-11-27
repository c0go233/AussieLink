using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.Contracts.Models
{
    public class ResetPasswordLink
    {
        public int ResetPasswordLinkId { get; set; }

        public User User { get; set; }
        public Guid UserId { get; set; }

        public bool Clicked { get; set; }
        public DateTime DateCreated { get; set; }

        public ResetPasswordLink()
        {}

        public ResetPasswordLink(Guid userId)
        {
            this.UserId = userId;
            Clicked = false;
            DateCreated = DateTime.Now;
        }
    }
}
