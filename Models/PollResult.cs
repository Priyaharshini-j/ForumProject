namespace ForumProject.Models
{
    public class PollResult
    {
        //This model is designed for the forum creation wiht question and eveything related to forum
        public int pollResId { get; set; }
        public int pollId { get; set; }
        public int Id { get; set; }
        public string VoteOtion { get; set; }
        public DateTime VotedDate { get; set; } = System.DateTime.Now;
    }
}
