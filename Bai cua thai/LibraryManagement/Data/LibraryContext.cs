using Microsoft.EntityFrameworkCore;
using LibraryManagement.Utils;
using System.Collections.Generic;

namespace LibraryManagement.Models
{
    public class LibraryContext : DbContext
    {
        // Constructor cho EF Core CLI
        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options)
        {
        }

        // Constructor mặc định cho ứng dụng
        public LibraryContext() { } 

        // Các DbSet
        public DbSet<Book> Books { get; set; }
        public DbSet<Reader> Readers { get; set; }
        public DbSet<Borrow> Borrows { get; set; } // Khớp tên Borrows
        public DbSet<User> Users { get; set; }

        // Cấu hình Kết nối (chỉ dùng khi không truyền options - tức là trong Form)
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Sử dụng SQL Express (đã kết nối thành công)
              optionsBuilder.UseSqlServer(
    "Server=localhost\\SQLEXPRESS;Database=LibraryDB;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;"
);


            }
        }

        // Khởi tạo Dữ liệu mẫu (Seeding)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Thêm User Admin mặc định
            modelBuilder.Entity<User>().HasData(new List<User>
            {
                new User {
                    UserId = 1,
                    Username = "admin",
                    PasswordHash = HashHelper.HashPassword("admin123"), 
                    Role = "admin"
                }
            });
        }
    }
}