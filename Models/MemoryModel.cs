namespace ForumProject.Models
{
    public class MemoryModel
    {
        public int memoryId {  get; set; }
        public int Id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public DateTime memoryCreated { get; set; } = System.DateTime.Now;
    }
}
