namespace BlogApp.Model
{
    public class MainComment
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Content { get; set; }
        public int BlogId{ get; set; }
        public Blog Blog { get; set; }
        public List<SubComment> subComments { get; set; } = new List<SubComment>();
    }
}
