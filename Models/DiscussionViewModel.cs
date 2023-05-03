namespace ForumProject.Models
{
    public class DiscussionViewModel
    {
        //This model is used in order to retrive the replies and forum question to display
        public ForumModel forum { get; set; }
        public List<RepliesModel> replies { get; set; }
        public RepliesModel newReply { get; set; }

    }
}
