using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using CounterApi.Models;

namespace CounterApi.Data
{
    public class CounterRepository : IRepository<MyCount>
    {
        private readonly CounterApiContext db;

        public CounterRepository(CounterApiContext context)
        {
            db = context;
        }

        async Task<MyCount> IRepository<MyCount>.AddAsync(MyCount entity)
        {
            var newCount = db.MyCounts.Add(entity).Entity;
            await db.SaveChangesAsync();
            return newCount;
        }

        async Task IRepository<MyCount>.EditAsync(MyCount entity)
        {
            db.Entry(entity).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }

        async Task<MyCount> IRepository<MyCount>.GetAsync(int id)
        {
            return await db.MyCounts.FirstOrDefaultAsync(p => p.Id == id);
        }

        async Task<IEnumerable<MyCount>> IRepository<MyCount>.GetAllAsync()
        {
            return await db.MyCounts.ToListAsync();
        }

        async Task IRepository<MyCount>.RemoveAsync(int id)
        {
            var count = await db.MyCounts.FirstOrDefaultAsync(p => p.Id == id);
            if (count != null)
            {
                db.MyCounts.Remove(count);
                await db.SaveChangesAsync();
            }
        }
    }
}
