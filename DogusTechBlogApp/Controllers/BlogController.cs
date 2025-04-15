using DogusTechBlogApp.Data.Abstarct;
using DogusTechBlogApp.Data.Concrete;
using DogusTechBlogApp.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DogusTechBlogApp.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogRepository _blogRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly ICategoryRepository _categoryRepository;

        public BlogController(IBlogRepository blogRepository, ICommentRepository commentRepository,ICategoryRepository categoryRepository)
        {
            _blogRepository = blogRepository;
            _commentRepository = commentRepository;
            _categoryRepository = categoryRepository;
        }

        // Blog listeleme
        public IActionResult Index()
        {
            var blogs = _blogRepository.Blogs
                .Include(b => b.Category)
                .Include(b => b.User) // Bu kısmı eklemen gerek
                .ToList();

            return View(blogs);
        }


        // Blog detaylarını görüntüleme
        public IActionResult Details(int id)
        {
            var blog = _blogRepository.Blogs
     .Include(b => b.Category)
     .Include(b => b.User)
     .Include(b => b.Comments)
         .ThenInclude(c => c.User)
     .FirstOrDefault(b => b.Id == id);


            return View(blog);
        }

        // Yorum ekleme (Ajax veya form ile)
        [HttpPost]
        public IActionResult AddComment(int blogId, string text)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var username = User.FindFirstValue(ClaimTypes.Name);
            var avatar = User.FindFirstValue(ClaimTypes.UserData);

            var comment = new Comment
            {
                BlogId = blogId,
                Content = text,
                UserId = int.Parse(userId ?? ""),
                CreatedAt = DateTime.Now
            };

            _commentRepository.CreateComment(comment);

            return Json(new
            {
                username,
                text,
                publishedOn = comment.CreatedAt.ToString("g"),
                avatar
            });
        }

        // Blog oluşturma
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // Blog oluşturma (POST)
        [HttpPost]
        [Authorize]
 
        public IActionResult Create(Blog blog)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return RedirectToAction("Login", "Account");

            blog.UserId = int.Parse(userIdClaim.Value);
            blog.CreatedAt = DateTime.Now;
            _blogRepository.CreateBlog(blog);

            return RedirectToAction("MyBlogs");
        }


        // Blog düzenleme
        [Authorize]
        public IActionResult Edit(int id)
        {
            var blog = _blogRepository.Blogs.FirstOrDefault(b => b.Id == id);
            if (blog == null)
                return NotFound();

            return View(blog);
        }

        // Blog düzenleme (POST)
        [HttpPost]
        [Authorize]
        public IActionResult Edit(Blog model)
        {
            if (ModelState.IsValid)
            {
                _blogRepository.UpdateBlog(model);
                return RedirectToAction("Index");
            }

            return View(model);
        }


        public IActionResult CategoryBlogs(int categoryId)
        {
            // Kategoriyi al
            // Kategoriye ait blogları User ve Category dahil ederek al
            var blogs = _blogRepository.Blogs
                .Where(b => b.CategoryId == categoryId)  // CategoryId'ye göre filtrele
                .Include(b => b.User)  // User'ı dahil et
                .Include(b => b.Category)  // Category'yi dahil et
                .ToList();

            if (!blogs.Any())  // Eğer kategoriye ait blog yoksa
            {
                return NotFound("Kategoriye ait blog bulunamadı.");
            }

            // Blogları View'a gönder
            return View(blogs);
        }

        [Authorize]
        public IActionResult MyBlogs()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
            if (userIdClaim == null) return RedirectToAction("Login", "Account");

            int userId = int.Parse(userIdClaim.Value);
            var blogs = _blogRepository.Blogs
                .Where(b => b.UserId == userId)
                .ToList();

            return View(blogs);
        }


    }
}
