using AussieLink.Contracts.Repositories;

namespace AussieLink.Contracts.UnitOfWork
{
    public interface ISharePostUnitOfWork
    {
        IAddressRepository Addresses { get; }
        IGenderRepository Genders { get; }
        ISharePostRepository SharePosts { get; }
        IShareTypeRepository ShareTypes { get; }
        IPlaceRepository Places { get; }
        ISuburbRepository Suburbs { get; }
        IPictureRepository Pictures { get; }

        int Complete();
    }
}