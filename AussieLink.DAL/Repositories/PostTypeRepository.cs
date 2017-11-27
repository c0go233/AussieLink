using AussieLink.Contracts.Models.PostModels;
using AussieLink.Contracts.Repositories;
using AussieLink.DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.DAL.Repositories
{
    public class PostTypeRepository : BaseRepository<PostType>, IPostTypeRepository
    {
        public PostTypeRepository(DataContext context) : base(context) { }
    }
}
