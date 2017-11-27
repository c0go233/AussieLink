using AussieLink.Contracts.Models;
using AussieLink.Contracts.Repositories;
using AussieLink.DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.DAL.Repositories
{
    public class PictureRepository : BaseRepository<Picture>, IPictureRepository
    {
        private DataContext DataContext
        {
            get { return Context as DataContext; }
        }

        public PictureRepository(DataContext context) : base(context) { }
    }
}
