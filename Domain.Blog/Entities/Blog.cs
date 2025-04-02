namespace BlogApp.Model
{
    public class Blog
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string content { get; set; }
        public string Tags { get; set; }
        public string AuthorID {  get; set; }
        public ApplicationUser Author { get; set; }
        public List<MainComment> comments { get; set; } = new List<MainComment>();
        public List<Like> Likes { get; set; } = new List<Like>();

    }
}
