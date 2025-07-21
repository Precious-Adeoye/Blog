using System.ComponentModel.DataAnnotations.Schema;

namespace BlogApp.Model
{
    public class Blog
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Tags { get; set; }
        public string AuthorID {  get; set; }
        [ForeignKey("AuthorID")]
        public ApplicationUser Author { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<MainComment> Comments { get; set; } = new List<MainComment>();
        public List<Like> Likes { get; set; } = new List<Like>();
        public bool IsDelete { get; set; }

    }
}
