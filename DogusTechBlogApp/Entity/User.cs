namespace DogusTechBlogApp.Entity
{
    public enum UserRole
    {
        Admin,
        User
    }
    public class User
    {
        public int Id { get; set; }
        public string FirstName {  get; set; }

        public string LastName { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public string Email { get; set; }
        public string Password { get; set; }
        public  List<Blog> BlogPosts { get; set; }=new ();

        public List<Comment> Comments { get; set; } = new();

        public UserRole Role { get; set; } = UserRole.User;





    }
}
