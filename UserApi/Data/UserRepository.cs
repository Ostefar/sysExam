using Microsoft.EntityFrameworkCore;
using UserApi.Models;

namespace UserApi.Data
{
    public class UserRepository : IRepository<MyUser>
    {
        private readonly UserApiContext db;

        public UserRepository(UserApiContext context)
        {
            db = context;
        }

        public async Task<MyUser> AddAsync(MyUser entity)
        {
           // if (entity.Date == null)
             //   entity.Date = DateTime.Now;

            var newUser = await db.Users.AddAsync(entity);
            await db.SaveChangesAsync();
            return newUser.Entity;
        }

        public async Task EditAsync(MyUser entity)
        {
            db.Entry(entity).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }

        public async Task<MyUser> GetAsync(int id)
        {
            return await db.Users.FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<IEnumerable<MyUser>> GetAllAsync()
        {
            return await db.Users.ToListAsync();
        }

        public async Task RemoveAsync(int id)
        {
            var user = await db.Users.FirstOrDefaultAsync(p => p.Id == id);
            if (user != null)
            {
                db.Users.Remove(user);
                await db.SaveChangesAsync();
            }
        }
    }
}
