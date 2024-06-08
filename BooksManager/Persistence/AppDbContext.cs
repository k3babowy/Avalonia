using BooksManager.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksManager.Persistence
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        public AppDbContext(string connectionString) : base(connectionString)
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<AppDbContext>());
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookCategory> BookCategories { get; set; }
        public DbSet<Checkout> Checkouts { get; set; }

        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }

        public DbEntityEntry Entry(object entity)
        {
            return base.Entry(entity);
        }
    }
}
