namespace ForumProject.Models
{
    public class UserVote
    {
        public int Id { get; set; }
        public int PollId { get; set; }
        public string Email { get; set; }
        public int OptionId { get; set; }
    }
}
