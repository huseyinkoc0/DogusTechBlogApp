using DogusTechBlogApp.Entity;

namespace DogusTechBlogApp.Data.Abstarct
{
    public interface IUserRepository
    {
        public IQueryable<User> Users { get; }

        User GetById(int id);
        void Create(User user);
        void Update(User user);
        void Delete(int id);
    }
}
