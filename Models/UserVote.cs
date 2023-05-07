using System.ComponentModel;

namespace ForumProject.Models
{
    public class UserVote
    {
        public int Id { get; set; }
        [DisplayName("Poll Id")]
        public int PollId { get; set; }
        public string Email { get; set; }
        [DisplayName("Option Id")]
        public int OptionId { get; set; }
    }
}
