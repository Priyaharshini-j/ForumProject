using Microsoft.Build.Framework;
using System.ComponentModel;

namespace ForumProject.Models
{
    public class ForumModel
    {
        //This model is designed for the forum creation wiht question and eveything related to forum
        [Required]
        [DisplayName("Forum Id")]
        public int forumId { get; set; }
        [Required]
        [DisplayName("Forum created by")]
        public int Id { get; set; }
        [Required]
        [DisplayName("Category")]
        public string category { get; set; }
        [Required]
        [DisplayName("Title")]
        public string title { get; set; }
        [Required]
        [DisplayName("Content")]
        public string content { get; set; }
        [Required]
        [DisplayName("Created on")]
        public DateTime forumCreated { get; set; } = System.DateTime.Now;
    }
}
