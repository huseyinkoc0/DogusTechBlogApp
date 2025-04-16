using DogusTechBlogApp.Entity;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using DogusTechBlogApp.Data.Concrete;

namespace DogusTechBlogApp.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new BlogContext(
                serviceProvider.GetRequiredService<DbContextOptions<BlogContext>>());

            if (context.Users.Any() || context.Categories.Any() || context.Blogs.Any() || context.Comments.Any())
            {
                
                return;
            }

            var user = new User
            {
                FirstName = "Admin",
                LastName = "User",
                Email = "admin@example.com",
                Password = "123456",
                Role = UserRole.Admin
            };
            context.Users.Add(user);
            context.SaveChanges();

            var category = new Category
            {
                Name = "Genel",
                Description = "Genel konular hakkında yazılar"
            };
            context.Categories.Add(category);
            context.SaveChanges();

       
            var blog = new Blog
            {
                Title = "İlk Blog Yazısı",
                Content = "Bu, sistemdeki ilk blog yazısıdır.",
                UserId = user.Id,
                CategoryId = category.Id,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                ImageUrl = null
            };

            context.Blogs.Add(blog);
            context.SaveChanges();

            
            var comment = new Comment
            {
                Content = "Harika bir yazı olmuş!",
                BlogId = blog.Id,
                UserId = user.Id,
                CreatedAt = DateTime.Now
            };
            context.Comments.Add(comment);

            context.SaveChanges();
           
            var blog2 = new Blog
            {
                Title = "Teknolojide Son Gelişmeler",
                Content = "2025 yılı itibariyle yapay zeka ve blockchain alanında birçok yenilik yaşanıyor.",
                UserId = user.Id,
                CategoryId = category.Id,
                CreatedAt = DateTime.Now.AddDays(-2),
                UpdatedAt = DateTime.Now.AddDays(-1),
                ImageUrl = null
            };
            context.Blogs.Add(blog2);
            context.SaveChanges(); 

            var comment2 = new Comment
            {
                Content = "Çok bilgilendirici olmuş, teşekkürler!",
                BlogId = blog2.Id, 
                UserId = user.Id,
                CreatedAt = DateTime.Now
            };
            context.Comments.Add(comment2);
            context.SaveChanges();

            var blog3 = new Blog
            {
                Title = "Sağlıklı Yaşam İpuçları",
                Content = "Günde en az 30 dakika yürüyüş yapmak, hem fiziksel hem zihinsel sağlığa iyi gelir.",
                UserId = user.Id,
                CategoryId = category.Id,
                CreatedAt = DateTime.Now.AddDays(-3),
                UpdatedAt = DateTime.Now.AddDays(-2),
                ImageUrl = null
            };
            context.Blogs.Add(blog3);
            context.SaveChanges();

            
            var comment3 = new Comment
            {
                Content = "Kesinlikle katılıyorum. Spor şart!",
                BlogId = blog3.Id,
                UserId = user.Id,
                CreatedAt = DateTime.Now
            };
            context.Comments.Add(comment3);

            context.SaveChanges();
        }
    }
}
