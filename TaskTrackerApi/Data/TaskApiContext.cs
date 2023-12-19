using Microsoft.EntityFrameworkCore;
using TaskTrackerApi.Models;

namespace TaskTrackerApi.Data
{
    public class TaskApiContext : DbContext
    {
        public TaskApiContext(DbContextOptions<TaskApiContext> options)
            : base(options)
        {
        }

        public DbSet<MyTask> Tasks { get; set; }
    }
}
