using System.ComponentModel;

namespace ForumProject.Models
{
    public class ForumCreationModel
    {
        public string Email { get; set; }
        public string Category { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        [DisplayName("Forum Created")]
        public DateTime CreatedDate { get; set; }
            

    }
}
