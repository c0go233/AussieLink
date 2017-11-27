using AussieLink.Contracts.Models.CommentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.Contracts.Repositories
{
    public interface ICommentRepository : IBaseRepository<Comment>
    {
        IEnumerable<Comment> GetFullCommentsBy(Expression<Func<Comment, bool>> predicate);
    }
}
