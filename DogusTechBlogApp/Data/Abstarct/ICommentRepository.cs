using DogusTechBlogApp.Entity;

namespace DogusTechBlogApp.Data.Abstarct
{
    public interface ICommentRepository
    {
        IQueryable<Comment> Comments { get; }
       void DeleteComment(int id);
        void CreateComment(Comment comment);
        void UpdateComment(Comment comment);
    }
}
