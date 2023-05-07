using System.ComponentModel;

namespace ForumProject.Models
{
    public class PollResult
    {
        //This model is designed for the forum creation wiht question and eveything related to forum
        [DisplayName("Poll Vote Id")]
        public int pollResId { get; set; }
        [DisplayName("Poll Id")]
        public int pollId { get; set; }
        public int Id { get; set; }
        public string VoteOtion { get; set; }
        public DateTime VotedDate { get; set; } = System.DateTime.Now;
    }
}
