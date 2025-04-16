using DogusTechBlogApp.Entity;

namespace DogusTechBlogApp.Data.Abstarct
{
    public interface ICategoryRepository
    {
        IQueryable<Category> Categories { get; }

        void Add(Category category);
        void Update(Category category);
        void Delete(int id);

        List<Category> GetCategories();

    }
}
