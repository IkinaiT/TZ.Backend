using Microsoft.EntityFrameworkCore;
using TZ.Backend.DataBase.Models;

namespace TZ.Backend.DataBase
{
    public class DataBaseContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }

        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options) => Database.EnsureCreated();
    }
}
