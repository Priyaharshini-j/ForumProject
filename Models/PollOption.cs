using Microsoft.Build.Framework;

namespace ForumProject.Models
{
    public class PollOption
    {
        [Required]
        public int Id { get; set; }
        [Required] public int OptionId { get; set; }
        [Required]
        public string OptionText { get; set; }
        [Required]
        public int VoteCount { get; set; }
        public double VotePercentage { get; set; }
    }
}
