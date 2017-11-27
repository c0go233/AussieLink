using AussieLink.Contracts.Models.PostModels.SharePostModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.Contracts.Models.PostModels
{
    public class Place
    {
        public int PlaceId { get; set; }
        public string Name { get; set; }

        public ICollection<Suburb> Suburbs { get; set; }
    }
}
