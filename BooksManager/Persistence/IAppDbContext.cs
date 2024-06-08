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
    public interface IAppDbContext
    {

        DbSet<User> Users { get; set; }
        DbSet<Book> Books { get; set; }
        DbSet<BookCategory> BookCategories { get; set; }
        DbSet<Checkout> Checkouts { get; set; }
        Task<int> SaveChangesAsync();
        DbEntityEntry Entry(object entity);
    }
}
