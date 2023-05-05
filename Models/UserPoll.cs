namespace ForumProject.Models
{
    public class UserPoll
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public string Question { get; set; }
        public List<PollOption> Options { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
    }
}
