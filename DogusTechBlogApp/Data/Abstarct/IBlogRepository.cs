using DogusTechBlogApp.Entity;

namespace DogusTechBlogApp.Data.Abstarct
{
    public interface IBlogRepository
    {
        IQueryable<Blog> Blogs { get; }
        void CreateBlog(Blog blog);
        void UpdateBlog(Blog blog);
        void DeleteBlog(int id);

    }
}
