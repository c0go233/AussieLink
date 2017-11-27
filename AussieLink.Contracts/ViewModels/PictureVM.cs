using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.Contracts.ViewModels
{
    public class PictureVM
    {
        public string PictureId { get; set; }
        public string PictureName { get; set; }
        public int PictureSize { get; set; }
        public string ImageSrc { get; set; }
    }
}
