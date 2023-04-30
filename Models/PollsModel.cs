namespace ForumProject.Models
{
    public class PollsModel
    {
        //This model is designed for the forum creation wiht question and eveything related to poll
        public int pollId { get; set; }
        public int Id { get; set; }
        public string category { get; set; }
        public string pollTitle { get; set; }
        public DateTime pollCreated { get; set; } = System.DateTime.Now;
    }
}
