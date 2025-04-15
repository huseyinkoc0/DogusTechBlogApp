using System.ComponentModel.DataAnnotations;

namespace DogusTechBlogApp.Entity
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Kategori adı zorunludur.")]
        public string Name { get; set; }


        public string Description { get; set; }

        public  List<Blog> BlogPosts { get; set; } = new();
    }
}
