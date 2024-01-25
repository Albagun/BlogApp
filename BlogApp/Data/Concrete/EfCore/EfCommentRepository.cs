using BlogApp.Data.Abstract;
using BlogApp.Entity;

namespace BlogApp.Data.Concrete.EfCore 
{

    public class EfCommentRepository : ICommentRepository
    {   
        private BlogContext _context;
        public EfCommentRepository(BlogContext context)
        {
            _context = context;
        }
        public IQueryable<Comment> Comments => _context.Comments;

        public void CreateComment(Comment comment)
        {
            _context.Comments.Add(comment);
            _context.SaveChanges();
        }

        public void DeleteComment(int commentId)
        {
            var commentToDelete = _context.Comments.FirstOrDefault(c => c.CommentId == commentId);
            
            if (commentToDelete != null)
            {
                _context.Comments.Remove(commentToDelete);
                _context.SaveChanges();
            }
        }
    }
}