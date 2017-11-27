using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.Contracts.Models.PostModels.SharePostModels
{
    public class Address
    {
        public SharePost SharePost { get; set; }
        public int PostId { get; set; }

        public Place Place { get; set; }
        public int PlaceId { get; set; }

        public Suburb Suburb { get; set; }
        public int SuburbId { get; set; }
    }
}
