using AussieLink.Contracts.Models.CommentModels;
using AussieLink.Contracts.Repositories;
using AussieLink.DAL.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.DAL.Repositories
{
    public class CommentRepository : BaseRepository<Comment>, ICommentRepository
    {
        private DataContext DataContext
        {
            get { return Context as DataContext; }
        }

        public CommentRepository(DataContext context) : base(context) { }

        public IEnumerable<Comment> GetFullCommentsBy(Expression<Func<Comment, bool>> predicate)
        {
            return DataContext.Comments.Include(m => m.User).Where(predicate).OrderByDescending(m => m.DateCreated);
        }
    }
}
