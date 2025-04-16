    using DogusTechBlogApp.Data.Abstarct;
    using DogusTechBlogApp.Entity;
using Microsoft.EntityFrameworkCore;

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

        public Blog GetBlogById(int id)
        {
            return _blogContext.Blogs
                .Include(b => b.Category)  
                .FirstOrDefault(b => b.Id == id);
        }


        public void UpdateBlog(Blog blog)
        {
            var existingBlog = _blogContext.Blogs.Find(blog.Id);
            if (existingBlog != null)
            {
                existingBlog.Title = blog.Title;
                existingBlog.Content = blog.Content;
                existingBlog.CategoryId = blog.CategoryId;
                existingBlog.ImageUrl = blog.ImageUrl;  // Eğer görsel değişmişse, yeni görseli al
                _blogContext.Blogs.Update(existingBlog);
                _blogContext.SaveChanges();
            }
        }
    }
    }
