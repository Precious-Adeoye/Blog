namespace BlogApp.Model
{
    public class SubComment
    {
        public int Id { get; set; }
        public int MainCommentId { get; set; }
        public MainComment MainComment { get; set; }
        public string UserName { get; set; }
        public string Content { get; set; }
    }
}
