using System.ComponentModel;

namespace ForumProject.Models
{
    public class RepliesModel
    {
        //This model is designed for the forum creation wiht question and eveything related to forum replies
        
        public int replyId { get; set; }
        public int forumId { get; set; }
        [DisplayName("Reply")]
        public string replyContent { get; set; }
        [DisplayName("Reply Created On")]
        public DateTime replyCreated { get; set; } = System.DateTime.Now;

        public string Email { get; set; }
    }
}
