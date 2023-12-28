using UserApi.Models;

namespace UserApi.Data
{
    public class DbInitializer : IDbInitializer
    {
        // This method will create and seed the database.
        public void Initialize(UserApiContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            // Look for any users
            if (context.Users.Any())
            {
                return;   // DB has been seeded
            }

            List<MyUser> users = new List<MyUser>
            {
                    new MyUser
                {
                    FirstName = "Emil",
                    LastName = "Witt",
                    UserName = "sysInt",
                    Password = "1234",
                    Email = "Emil@email.dk",
                    Phone = "45454545"
                },
                new MyUser
                {
                    FirstName = "Emil",
                    LastName = "Ghidotti",
                    UserName = "sysMas",
                    Password = "5678",
                    Email = "Ghidotti@mail.dk",
                    Phone = "46464646"
                }
            };

            context.Users.AddRange(users);
            context.SaveChanges();
        }
    }
}
