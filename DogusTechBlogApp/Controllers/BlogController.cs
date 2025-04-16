using DogusTechBlogApp.Data.Abstarct;
using DogusTechBlogApp.Data.Concrete;
using DogusTechBlogApp.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace DogusTechBlogApp.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogRepository _blogRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly ICategoryRepository _categoryRepository;

        public BlogController(IBlogRepository blogRepository, ICommentRepository commentRepository, ICategoryRepository categoryRepository)
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
                .Include(b => b.User) 
                .ToList();

            return View(blogs);
        }


       
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

    
        [HttpPost]
        [HttpPost]
        public IActionResult AddComment(int blogId, string text)
        {
           
            var userId = User.FindFirstValue("UserId");
            var username = User.FindFirstValue(ClaimTypes.Name);
            var avatar = User.FindFirstValue(ClaimTypes.UserData);

        
            if (string.IsNullOrEmpty(userId))
            {
                
                return Json(new { error = "Geçersiz kullanıcı ID." });
            }

            var comment = new Comment
            {
                BlogId = blogId,
                Content = text,
                UserId = int.Parse(userId), 
                CreatedAt = DateTime.Now
            };

            _commentRepository.CreateComment(comment);

            return RedirectToAction("Details", "Blog", new { id = blogId });
        }




        //blog olusturma
        [Authorize]
        public IActionResult Create()
        {
            ViewBag.Categories = _categoryRepository.Categories.ToList();
            return View();
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(Blog blog, IFormFile? ImageFile)
        {
            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim == null)
                return RedirectToAction("Login", "Account");

            blog.UserId = int.Parse(userIdClaim.Value);
            blog.CreatedAt = DateTime.Now;

            // Görsel yüklendiyse işle
            if (ImageFile != null && ImageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                blog.ImageUrl = "/uploads/" + uniqueFileName;
            }

            _blogRepository.CreateBlog(blog);

            return RedirectToAction("MyBlogs");
        }




     
        public IActionResult Edit(int id)
        {
            var blog = _blogRepository.GetBlogById(id);
            if (blog == null)
            {
                return NotFound(); 
            }

            ViewBag.Categories = _categoryRepository.GetCategories(); 

            return View(blog);  
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(Blog blog, IFormFile? ImageFile)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _categoryRepository.GetCategories();
                return View(blog);
            }

           
            if (ImageFile != null && ImageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                blog.ImageUrl = "/uploads/" + uniqueFileName;
            }

            blog.UpdatedAt = DateTime.Now;
            _blogRepository.UpdateBlog(blog);

            return RedirectToAction("MyBlogs");
        }












        public IActionResult CategoryBlogs(int categoryId)
        {
            
            var blogs = _blogRepository.Blogs
                .Where(b => b.CategoryId == categoryId)  
                .Include(b => b.User)  
                .Include(b => b.Category)  
                .ToList();

            if (!blogs.Any())  
            {
                return NotFound("Kategoriye ait blog bulunamadı.");
            }

            
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


        [Authorize]
        public IActionResult Delete(int id)
        {
            var blog = _blogRepository.Blogs.FirstOrDefault(b => b.Id == id);
            if (blog == null)
                return NotFound();

            _blogRepository.DeleteBlog(blog.Id);  

            return RedirectToAction("MyBlogs"); 
        }



    }
}
