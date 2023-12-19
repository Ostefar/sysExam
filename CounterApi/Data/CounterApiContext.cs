using CounterApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CounterApi.Data
{
    public class CounterApiContext : DbContext
    {
        public CounterApiContext(DbContextOptions<CounterApiContext> options)
            : base(options)
        {
        }

        public DbSet<MyCount> MyCounts { get; set; }
    }
}
