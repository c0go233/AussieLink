using AussieLink.Contracts.Models.PostModels.SharePostModels;
using AussieLink.Contracts.Repositories;
using AussieLink.DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace AussieLink.DAL.Repositories
{
    public class SharePostRepository : BaseRepository<SharePost>, ISharePostRepository
    {
        private DataContext DataContext
        {
            get { return Context as DataContext; } 
        }

        public SharePostRepository(DataContext dataContext) : base(dataContext) {}

        public SharePost GetFullSharePost(int postId)
        {
            return DataContext.SharePosts.Include(m => m.Gender)
                .Include(m => m.Pictures).Include(m => m.ShareType).Include(m => m.Address.Place)
                .Include(m => m.Address.Suburb)
                .SingleOrDefault(m => (m.PostId == postId) && (m.Cancel == false));
        }
    }

    public class AddressRepository : BaseRepository<Address>, IAddressRepository
    {
        private DataContext DataContext
        {
            get { return Context as DataContext; }
        }
        public AddressRepository(DataContext dataContext) : base(dataContext) { }
    }

    public class ShareTypeRepository : BaseRepository<ShareType>, IShareTypeRepository
    {
        private DataContext DataContext
        {
            get { return Context as DataContext; }
        }
        public ShareTypeRepository(DataContext dataContext) : base(dataContext) { }
    }

    public class GenderRepository : BaseRepository<Gender>, IGenderRepository
    {
        private DataContext DataContext
        {
            get { return Context as DataContext; }
        }
        public GenderRepository(DataContext dataContext) : base(dataContext) { }
    }

    public class SuburbRepository : BaseRepository<Suburb>, ISuburbRepository
    {
        private DataContext DataContext
        {
            get { return Context as DataContext; }
        }
        public SuburbRepository(DataContext dataContext) : base(dataContext) { }
    }


}
