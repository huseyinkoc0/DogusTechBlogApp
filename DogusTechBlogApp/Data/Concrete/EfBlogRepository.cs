using DogusTechBlogApp.Data.Abstarct;
using DogusTechBlogApp.Entity;

namespace DogusTechBlogApp.Data.Concrete
{
    public class EfBlogRepository : IBlogRepository
    {
        private readonly BlogContext _blogContext;
        public EfBlogRepository(BlogContext context) 
        {
                _blogContext = context;
                
        }
        public IQueryable<Blog> Blogs => _blogContext.Blogs;

        public void CreateBlog(Blog blog)
        {
            _blogContext.Blogs.Add(blog);
            _blogContext.SaveChanges();
        }

        public void DeleteBlog(int id)
        {
            var deleteBlog = _blogContext.Blogs.Find(id);
            if (deleteBlog != null) { 
            
                _blogContext.Blogs.Remove(deleteBlog);
                _blogContext.SaveChanges();
            }
        }

        public void UpdateBlog(Blog blog)
        {
            _blogContext.Blogs.Update(blog);
            _blogContext.SaveChanges();
        }
    }
}
