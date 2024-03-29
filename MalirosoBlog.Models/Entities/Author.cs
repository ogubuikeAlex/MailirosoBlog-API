namespace MalirosoBlog.Models.Entities
{
    public class Author : BaseEntity
    {
        public string Id {  get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public ICollection<BlogPost> BlogPosts { get; set; }
    }
}
