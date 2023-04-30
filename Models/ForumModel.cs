namespace ForumProject.Models
{
    public class ForumModel
    {
        //This model is designed for the forum creation wiht question and eveything related to forum
        public int forumId { get; set; }
        public int Id { get; set; }
        public string category { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public DateTime forumCreated { get; set; } = System.DateTime.Now;
    }
}
