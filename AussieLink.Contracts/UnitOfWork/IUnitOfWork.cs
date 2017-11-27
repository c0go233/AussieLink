using AussieLink.Contracts.Repositories;
using System;

namespace AussieLink.Contracts.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IResetPasswordLinkRepository ResetPasswordLinks { get; }
        IPostTypeRepository PostTypes { get; }
        ICommentRepository Comments { get; }


        int Complete();
    }
}