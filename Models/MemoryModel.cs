using System.ComponentModel;

namespace ForumProject.Models
{
    public class MemoryModel
    {
        [DisplayName("Memeory Id")]
        public int memoryId {  get; set; }
        public int Id { get; set; }
        [DisplayName("Title")]
        public string title { get; set; }
        [DisplayName("Description")]
        public string description { get; set; }
        [DisplayName("Memeory Created")]
        public DateTime memoryCreated { get; set; } = System.DateTime.Now;
    }
}
