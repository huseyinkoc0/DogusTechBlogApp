using DogusTechBlogApp.Data.Abstarct;
using DogusTechBlogApp.Entity;
using Microsoft.AspNetCore.Mvc;

namespace DogusTechBlogApp.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public IActionResult Add()
        {
            if (!User.IsInRole("Admin"))
                return Unauthorized();

            return View();
        }

        [HttpPost]
        public IActionResult Add(string name, string description)
        {
            if (!User.IsInRole("Admin"))
                return Unauthorized();

            if (string.IsNullOrWhiteSpace(name))
            {
                ViewBag.Error = "Kategori adı zorunludur.";
                return View();
            }

            var category = new Category
            {
                Name = name,
                Description = description
            };

            _categoryRepository.Add(category); 
            return RedirectToAction("Index", "Blog");
        }
    }

}
