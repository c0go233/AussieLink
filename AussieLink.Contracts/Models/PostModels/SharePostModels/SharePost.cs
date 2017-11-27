using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.Contracts.Models.PostModels.SharePostModels
{
    public class SharePost : BasePost
    {
        public decimal Price { get; set; }
        public DateTime AvailableFrom { get; set; }

        public ShareType ShareType { get; set; }
        public byte ShareTypeId { get; set; }

        public Gender Gender { get; set; }
        public byte GenderId { get; set; }

        public Address Address { get; set; }

        public ICollection<Picture> Pictures { get; set; }
    }
}
