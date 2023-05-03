namespace ForumProject.Models
{
    public class RepliesModel
    {
        //This model is designed for the forum creation wiht question and eveything related to forum replies
        public int replyId { get; set; }
        public int forumId { get; set; }
        public string replyContent { get; set; }
        public DateTime replyCreated { get; set; } = System.DateTime.Now;

        public string Email { get; set; }
    }
}
