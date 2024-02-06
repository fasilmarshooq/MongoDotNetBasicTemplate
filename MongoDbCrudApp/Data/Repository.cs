using Microsoft.EntityFrameworkCore;

namespace MongoDbCrudApp.Data
{
    public class Repository(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Student> Students { get; set; }

    }
}
