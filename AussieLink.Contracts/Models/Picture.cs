using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.Contracts.Models
{
    public class Picture
    {
        public int PictureId { get; set; }
        public int PostId { get; set; }
        public byte[] Data { get; set; }
        public string ImageName { get; set; }
        public int Size { get; set; }
        public string PictureType { get; set; }

        public Picture() { }

        public Picture(int postId, byte[] data, string imageName, int size, string pictureType)
        {
            this.PostId = postId;
            this.Data = data;
            this.ImageName = imageName;
            this.Size = size;
            this.PictureType = pictureType;
        }

    }
}
