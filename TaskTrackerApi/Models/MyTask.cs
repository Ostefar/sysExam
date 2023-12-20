namespace TaskTrackerApi.Models
{
    public class MyTask
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public TaskStatus Status { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public enum TaskStatus
        {
            cancelled, 
            completed, 
            todo, 
            doing 
        }
    }
}
