using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using LibraryManagement.Models;

namespace LibraryManagement.Data
{
    // Factory để EF CLI có thể khởi tạo LibraryContext
    public class LibraryContextFactory : IDesignTimeDbContextFactory<LibraryContext>
    {
        public LibraryContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<LibraryContext>();
            optionsBuilder.UseSqlServer(
                @"Server=.\SQLEXPRESS;Database=LibraryDB;Trusted_Connection=True;TrustServerCertificate=True;"
            );

            return new LibraryContext(optionsBuilder.Options);
        }
    }
}
