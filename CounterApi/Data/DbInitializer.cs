using CounterApi.Models;
using System.Collections.Generic;
using System.Linq;

namespace CounterApi.Data
{
    public class DbInitializer : IDbInitializer
    {
        // This method will create and seed the database.
        public void Initialize(CounterApiContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            // Look for counts
            if (context.MyCounts.Any())
            {
                return;   // DB has been seeded
            }

            List<MyCount> counts = new List<MyCount>
            {
                new MyCount { GetCount = 0, PostCount = 0, PutCount = 0, DeleteCount = 0 },
            };

            context.MyCounts.AddRange(counts);
            context.SaveChanges();
        }
    }
}
