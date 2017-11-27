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
    public class ResetPasswordLinkRepository : BaseRepository<ResetPasswordLink>, IResetPasswordLinkRepository
    {
        private DataContext DataContext
        {
            get
            { return Context as DataContext; }
        }

        public ResetPasswordLinkRepository(DataContext context) : base(context)
        {}
    }
}
