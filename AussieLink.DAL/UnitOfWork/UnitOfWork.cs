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
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext Context;
        public IUserRepository Users { get; }
        public IResetPasswordLinkRepository ResetPasswordLinks { get; }
        public IPostTypeRepository PostTypes { get; }
        public ICommentRepository Comments { get; }

        public UnitOfWork(DataContext context)
        {
            this.Context = context;
            Users = new UserRepository(Context);
            ResetPasswordLinks = new ResetPasswordLinkRepository(Context);
            PostTypes = new PostTypeRepository(Context);
            Comments = new CommentRepository(Context);
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
