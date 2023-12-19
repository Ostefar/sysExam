using TaskTrackerApi.Models;

namespace TaskTrackerApi.Data
{
    public class DbInitializer : IDbInitializer
    {
        // This method will create and seed the database.
        public void Initialize(TaskApiContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            // Look for any tasks
            if (context.Tasks.Any())
            {
                return;   // DB has been seeded
            }

            List<MyTask> tasks = new List<MyTask>
            {
                    new MyTask
                {
                    Title = "Setup database",
                    Description = "Create database schema and tables",
                    Status = MyTask.TaskStatus.doing,
                    Priority = "high",
                    DueDate = DateTime.Now.AddDays(7),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new MyTask
                {
                    Title = "Implement user authentication",
                    Description = "Add user authentication functionality",
                    Status = MyTask.TaskStatus.todo,
                    Priority = "medium",
                    DueDate = DateTime.Now.AddDays(14),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };

            context.Tasks.AddRange(tasks);
            context.SaveChanges();
        }
    }
}
