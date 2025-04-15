using DogusTechBlogApp.Data.Abstarct;
using DogusTechBlogApp.Entity;

namespace DogusTechBlogApp.Data.Concrete
{
    public class EfUserRepository : IUserRepository
    {
        private readonly BlogContext _blogContext;
        public EfUserRepository(BlogContext blogContext)
        {
            _blogContext = blogContext;
        }
        public IQueryable<User> Users => _blogContext.Users;

        public void Create(User user)
        {
            _blogContext.Users.Add(user);
            _blogContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var deleteUser = _blogContext.Users.Find(id);
            if (deleteUser != null) { 
            _blogContext.Users.Remove(deleteUser);
                _blogContext.SaveChanges();
            
            }
        }

        public User GetById(int id)
        {
            return _blogContext.Users.Find(id);
        }

        public void Update(User user)
        {
            _blogContext.Users.Update(user);
            _blogContext.SaveChanges();
        }
    }
}
