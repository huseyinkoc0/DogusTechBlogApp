using DogusTechBlogApp.Data.Abstarct;
using DogusTechBlogApp.Entity;

namespace DogusTechBlogApp.Data.Concrete
{
    public class EfCommentRepository : ICommentRepository
    {
        private readonly BlogContext _context;
        public EfCommentRepository(BlogContext blogContext) 
        {
        _context = blogContext;
        }
        public IQueryable<Comment> Comments => _context.Comments;

        public void CreateComment(Comment comment)
        {
            _context.Comments.Add(comment);
            _context.SaveChanges();
        }

        public void DeleteComment(int id)
        {
            var deleteComment = _context.Comments.Find(id);
            if (deleteComment != null) { 
            
                _context.Comments.Remove(deleteComment);
                _context.SaveChanges() ;
            
            }
        }

        public void UpdateComment(Comment comment)
        {
            _context.Comments.Update(comment);
            _context.SaveChanges();
        }
    }
}
