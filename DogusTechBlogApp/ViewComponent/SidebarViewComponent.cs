using DogusTechBlogApp.Data.Abstarct;
using DogusTechBlogApp.Entity;
using Microsoft.AspNetCore.Mvc;

namespace DogusTechBlogApp.ViewComponents
{
    public class SidebarViewComponent : ViewComponent
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBlogRepository _blogRepository;

        public SidebarViewComponent(ICategoryRepository categoryRepository, IBlogRepository blogRepository)
        {
            _categoryRepository = categoryRepository;
            _blogRepository = blogRepository;
        }

        public IViewComponentResult Invoke()
        {
            var categories = _categoryRepository.Categories.ToList();
            var recentBlogs = _blogRepository.Blogs
                                .OrderByDescending(b => b.CreatedAt)
                                .Take(3)
                                .ToList();

            var model = new SidebarViewModel
            {
                Categories = categories,
                RecentBlogs = recentBlogs
            };

            return View(model);
        }
    }
    public class SidebarViewModel
    {
        public List<Category> Categories { get; set; }
        public List<Blog> RecentBlogs { get; set; }
    }
}
