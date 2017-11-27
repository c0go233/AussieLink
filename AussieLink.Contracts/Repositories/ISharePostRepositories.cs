using AussieLink.Contracts.Models.PostModels.SharePostModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.Contracts.Repositories
{
    public interface ISharePostRepository : IBaseRepository<SharePost>
    {
        SharePost GetFullSharePost(int postId);
    }
    public interface IAddressRepository : IBaseRepository<Address> { }
    public interface IShareTypeRepository : IBaseRepository<ShareType> { }
    public interface IGenderRepository : IBaseRepository<Gender> { }
    public interface ISuburbRepository : IBaseRepository<Suburb> { }
}
