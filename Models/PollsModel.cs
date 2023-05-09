using System.ComponentModel;

namespace ForumProject.Models
{
    public class PollsModel
    {
        //This model is designed for the forum creation wiht question and eveything related to poll
        [DisplayName("Poll Id")]
        public int pollId { get; set; }
        [DisplayName("User Id")]
        public int Id { get; set; }
        [DisplayName("Category")]
        public string category { get; set; }
        public string pollTitle { get; set; }
        public DateTime pollCreated { get; set; } = System.DateTime.Now;
    }
}
