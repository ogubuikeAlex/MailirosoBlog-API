namespace MalirosoBlog.Models.Entities
{
    public class BlogPost : BaseEntity
    {
        public long Id { get; set; }

        public string Title { get; set; }
        public string Content { get; set; }
        public string AuthorId { get; set; }
        public Author Author { get; set; }
    }
}
