using DogusTechBlogApp.Data.Abstarct;
using DogusTechBlogApp.Entity;

namespace DogusTechBlogApp.Data.Concrete
{
    public class EfCategoryRepository:ICategoryRepository
    {
        private readonly BlogContext _context;
        public EfCategoryRepository(BlogContext context)
        {
            
        _context = context;
        }

       public IQueryable<Category> Categories => _context.Categories;

        public void Add(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
        }

       public void Delete(int id)
        {
            var deleteCategory = _context.Categories.Find(id);

            if (deleteCategory != null)
            {
               _context.Categories.Remove(deleteCategory);
                _context.SaveChanges();
            }
        }

        public List<Category> GetCategories()
        {
            return _context.Categories.ToList();
        }

        public void Update(Category category)
        {
          _context.Categories.Update(category);
            _context.SaveChanges();
        }
    }
}
