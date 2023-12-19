using Microsoft.EntityFrameworkCore;
using TaskTrackerApi.Models;

namespace TaskTrackerApi.Data
{
    public class TaskRepository : IRepository<MyTask>
    {
        private readonly TaskApiContext db;

        public TaskRepository(TaskApiContext context)
        {
            db = context;
        }

        public async Task<MyTask> AddAsync(MyTask entity)
        {
           // if (entity.Date == null)
             //   entity.Date = DateTime.Now;

            var newTask = await db.Tasks.AddAsync(entity);
            await db.SaveChangesAsync();
            return newTask.Entity;
        }

        public async Task EditAsync(MyTask entity)
        {
            db.Entry(entity).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }

        public async Task<MyTask> GetAsync(int id)
        {
            return await db.Tasks.FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<IEnumerable<MyTask>> GetAllAsync()
        {
            return await db.Tasks.ToListAsync();
        }

        public async Task RemoveAsync(int id)
        {
            var task = await db.Tasks.FirstOrDefaultAsync(p => p.Id == id);
            if (task != null)
            {
                db.Tasks.Remove(task);
                await db.SaveChangesAsync();
            }
        }
    }
}
