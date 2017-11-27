using AussieLink.Contracts.Repositories;
using AussieLink.Contracts.UnitOfWork;
using AussieLink.DAL.Data;
using AussieLink.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.DAL.UnitOfWork
{
    public class SharePostUnitOfWork : ISharePostUnitOfWork
    {
        private readonly DataContext Context;
        public ISharePostRepository SharePosts { get; }
        public IAddressRepository Addresses { get; }
        public IShareTypeRepository ShareTypes { get; }
        public IGenderRepository Genders { get; }
        public IPlaceRepository Places { get; }
        public ISuburbRepository Suburbs { get; }
        public IPictureRepository Pictures { get; }

        public SharePostUnitOfWork(DataContext context)
        {
            this.Context = context;
            SharePosts = new SharePostRepository(Context);
            Addresses = new AddressRepository(Context);
            ShareTypes = new ShareTypeRepository(Context);
            Genders = new GenderRepository(Context);
            Places = new PlaceRepository(Context);
            Suburbs = new SuburbRepository(Context);
            Pictures = new PictureRepository(Context);
        }

        public int Complete()
        {
            return Context.SaveChanges();
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
